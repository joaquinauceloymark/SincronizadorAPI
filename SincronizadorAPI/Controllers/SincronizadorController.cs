using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SincronizadorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SincronizadorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _filePath;
        private readonly bool _uploadFilesFtp;
        private readonly string _ftpServer;
        private readonly string _ftpUser;
        private readonly string _ftpPassword;
        public SincronizadorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _filePath = _configuration.GetValue<string>("FileSettings:Path");
            _uploadFilesFtp = _configuration.GetValue<bool>("FileSettings:UploadFileFtp");
            _ftpServer = _configuration.GetValue<string>("FtpConfigs:Server");
            _ftpUser = _configuration.GetValue<string>("FtpConfigs:User");
            _ftpPassword = _configuration.GetValue<string>("FtpConfigs:Password");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<Model> datos)
        {
            if (datos == null || datos.Count == 0)
            {
                return BadRequest("No hay datos para procesar.");
            }

            var tienda = string.Empty;

            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (var dato in datos)
                {
                    string lineaEncabezado = CreaLineaEncabezado(dato);
                    sb.AppendLine(lineaEncabezado);
                    foreach (var item in dato.Detalle)
                    {
                        string lineaDetalle = CreaLineaDetalle(dato, item.Codigo_Producto, item.Linea_Producto, item.Cantidad_Producto, item.Precio_Producto);
                        sb.AppendLine(lineaDetalle);

                    }
                    tienda = dato.Tienda;
                }

                await System.IO.File.WriteAllTextAsync(_filePath, sb.ToString());

                if (_uploadFilesFtp)
                {
                    UploadFilesToFtp(_filePath, tienda);
                }

                return Ok($"Archivo guardado en {_filePath}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar el archivo: {ex.Message}");
            }
        }

        private string CreaLineaEncabezado(Model enc)
        {

            DateTime fecha = DateTime.ParseExact(enc.F_FACTURA, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string fechaSoloDia = fecha.ToString("dd/MM/yyyy");

            const string SEPARADOR = ",";
            string encabezado = string.Empty;
            encabezado += "E" + SEPARADOR;
            encabezado += enc.Cia + SEPARADOR;
            encabezado += enc.Caja + SEPARADOR;
            encabezado += enc.N_factura + SEPARADOR;
            encabezado += Convert.ToString(enc.ORDEN) + SEPARADOR;
            encabezado += Convert.ToString(enc.C_SALONERO) + SEPARADOR;
            encabezado += this.formatear(enc.MONTO) + SEPARADOR;
            encabezado += this.formatear(enc.PROPINA) + SEPARADOR;
            encabezado += this.formatear(enc.DESCUENTO) + SEPARADOR;
            encabezado += fechaSoloDia.ToString() + SEPARADOR;
            encabezado += this.formatear(enc.IMPUESTO) + SEPARADOR;
            encabezado += Convert.ToString(enc.TIPO_FACT) + SEPARADOR;
            encabezado += this.formatear(enc.EXTRAS) + SEPARADOR;
            encabezado += Convert.ToString(enc.N_HABITACI) + SEPARADOR;
            encabezado += Convert.ToString(enc.H_PEDI) + SEPARADOR;
            encabezado += Convert.ToString(enc.H_FACT) + SEPARADOR;
            encabezado += this.formatear(enc.N_PERSONAS) + SEPARADOR;
            encabezado += Convert.ToString(enc.Hora) + SEPARADOR;
            encabezado += Convert.ToString(enc.CTF_1) + SEPARADOR;
            encabezado += Convert.ToString(enc.CTF_2) + SEPARADOR;
            encabezado += Convert.ToString(enc.CTF_3) + SEPARADOR;
            encabezado += this.formatear(enc.PAGO_1) + SEPARADOR;
            encabezado += this.formatear(enc.PAGO_2) + SEPARADOR;
            encabezado += this.formatear(enc.PAGO_3) + SEPARADOR;
            encabezado += Convert.ToString(enc.C_MESA) + SEPARADOR;
            encabezado += $"{enc.estado}{SEPARADOR}";
            encabezado += enc.Cajero + SEPARADOR;
            encabezado += enc.C_CLIENTE + SEPARADOR;
            encabezado += Convert.ToString(enc.LeidoFreq) + SEPARADOR;
            encabezado += Convert.ToString(enc.leidofecha) + SEPARADOR;
            encabezado += Convert.ToString(enc.CUPONDIGITAL);
            return encabezado;
        }

        private string CreaLineaDetalle(Model det, string codigoProducto, int lineaProducto, int cantidadProducto, int precioProducto)
        {
            const string SEPARADOR = ",";
            string detalle = "D" + SEPARADOR;
            detalle += det.Cia + SEPARADOR;
            detalle += det.Caja + SEPARADOR;
            detalle += det.N_factura + SEPARADOR;
            detalle += codigoProducto + SEPARADOR;
            detalle += Convert.ToString(lineaProducto) + SEPARADOR;
            detalle += this.formatear(cantidadProducto) + SEPARADOR;
            detalle += this.formatear(precioProducto) + SEPARADOR;
            detalle += this.formatear(precioProducto) + SEPARADOR;
            detalle += SEPARADOR;
            detalle += this.formatear(det.DESCUENTO);
            return detalle;
        }

        private string formatear<T>(T Numero)
        {
            string numeroStr = Numero.ToString();
            numeroStr = numeroStr.Replace(",", ".");

            if (numeroStr.EndsWith(".0"))
            {
                numeroStr = numeroStr.Substring(0, numeroStr.Length - 2);
            }

            return numeroStr;
        }

        private async Task UploadFilesToFtp(string _filePath, string tienda)
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);
                client.BaseAddress = _ftpServer;
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(_filePath);

                try
                {

                    string timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH_mm_ss");
                    string newFileName = $"{tienda}_{timestamp}.dat";
                    string uri = $"{_ftpServer}/{newFileName}";

                    await client.UploadDataTaskAsync(new Uri(uri), "STOR", fileBytes);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cargar el archivo al FTP", ex);
                }
            }
        }
    }
}
