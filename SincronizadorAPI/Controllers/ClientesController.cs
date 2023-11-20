using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SincronizadorAPI.Models;
using SincronizadorAPI.Repository;
using SincronizadorAPI.Repository.IRepository;

namespace SincronizadorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ClientesController> _logger;
        public ClientesController(IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _config = config;
            _scopeFactory = scopeFactory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var clienteRepository = scope.ServiceProvider.GetRequiredService<IClienteRepository>();


                    var respuesta = clienteRepository.GetClientes();

                    return Ok(respuesta);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ocurrió un error al obtener clientes");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("/api/clientes/GetUltimaHora")]
        public IActionResult GetUltimaHora()
        {
            try
            {
                List<clientes> clientesModificados = new List<clientes>();
                using (var scope = _scopeFactory.CreateScope())
                {
                    var clienteRepository = scope.ServiceProvider.GetRequiredService<IClienteRepository>();

                    var respuesta = clienteRepository.GetClientesAll();

                    return Ok(respuesta);
                    //foreach (var cliente in respuesta)
                    //{
                    //    cliente.CLI_PROCESADO = "N";
                    //    clienteRepository.UpdateCliProcesado(cliente);

                    //    var clienteCis = clienteRepository.GetCisClienteByCedula(cliente.CLI_NUMTARJETAACTIVA);
                    //    if (clienteCis != null)
                    //    {
                    //        if (cliente.CLI_PUNTOSDISPONIBLES != decimal.Parse(clienteCis.Saldo.ToString()))
                    //            clienteRepository.UpdateSaldoCisCliente(clienteCis.Cedula, cliente.CLI_PUNTOSDISPONIBLES);
                    //    }
                    //    else
                    //    {
                    //        clientes cli = new clientes();
                    //        cli.Cia = "1";
                    //        cli.Codigo = !string.IsNullOrEmpty(cliente.CLI_CODIGOENCLIENTE) ? cliente.CLI_CODIGOENCLIENTE : _config.GetValue<string>("ValoresDefectoCliCliente:Codigo");
                    //        cli.Renta = _config.GetValue<int>("ValoresDefectoCliCliente:Renta");
                    //        cli.Nombre = cliente.CLI_NOMBRE;
                    //        cli.Telefono = !string.IsNullOrEmpty(cliente.CLI_TELEFONOCELULAR) ? cliente.CLI_TELEFONOCELULAR : _config.GetValue<string>("ValoresDefectoCliCliente:Telefono");
                    //        cli.Cedula = !string.IsNullOrEmpty(cliente.CLI_CEDULA) ? cliente.CLI_CEDULA : _config.GetValue<string>("ValoresDefectoCliCliente:Cedula");
                    //        cli.Apartado = _config.GetValue<string>("ValoresDefectoCliCliente:Apartado");
                    //        cli.F_ult_compra = DateTime.Now;
                    //        cli.Fax = !string.IsNullOrEmpty(cliente.CLI_FAX) ? cliente.CLI_FAX : _config.GetValue<string>("ValoresDefectoCliCliente:Fax");
                    //        cli.Plazo = _config.GetValue<short>("ValoresDefectoCliCliente:Plazo");
                    //        cli.C_contable = _config.GetValue<string>("ValoresDefectoCliCliente:C_contable");
                    //        cli.Dia_cobro = _config.GetValue<short>("ValoresDefectoCliCliente:Dia_cobro");
                    //        cli.Exento = _config.GetValue<int>("ValoresDefectoCliCliente:Exento");
                    //        cli.IV = _config.GetValue<float>("ValoresDefectoCliCliente:IV");
                    //        cli.Saldo = float.Parse(cliente.CLI_PUNTOSDISPONIBLES.ToString());
                    //        cli.Linea1 = _config.GetValue<string>("ValoresDefectoCliCliente:Linea1");
                    //        cli.Saldo_Ant = _config.GetValue<float>("ValoresDefectoCliCliente:Saldo_Ant");
                    //        cli.Linea2 = _config.GetValue<string>("ValoresDefectoCliCliente:Linea2");
                    //        cli.Aplica = _config.GetValue<decimal>("ValoresDefectoCliCliente:Aplica");
                    //        cli.Adelantos = _config.GetValue<float>("ValoresDefectoCliCliente:Adelantos");
                    //        cli.email = !string.IsNullOrEmpty(cliente.CLI_EMAIL) ? cliente.CLI_EMAIL : _config.GetValue<string>("ValoresDefectoCliCliente:Email");
                    //        cli.Limi_cred = _config.GetValue<float>("ValoresDefectoCliCliente:Limi_cred");
                    //        cli.Zona = !string.IsNullOrEmpty(cliente.CLI_CODIGOPOSTAL) ? cliente.CLI_CODIGOPOSTAL : _config.GetValue<string>("ValoresDefectoCliCliente:Zona");
                    //        cli.Agente = !string.IsNullOrEmpty(cliente.CREATEUSERID) ? cliente.CREATEUSERID.Substring(0, 2) : _config.GetValue<string>("ValoresDefectoCliCliente:Agente");
                    //        cli.Contacto = _config.GetValue<string>("ValoresDefectoCliCliente:Contacto");
                    //        cli.Moneda = _config.GetValue<string>("ValoresDefectoCliCliente:Moneda");
                    //        cli.Direccion = _config.GetValue<string>("ValoresDefectoCliCliente:Direccion");
                    //        cli.Dia_Tramit = _config.GetValue<short>("ValoresDefectoCliCliente:Dia_Tramit");
                    //        cli.Descuento = _config.GetValue<float>("ValoresDefectoCliCliente:Descuento");
                    //        cli.NumPrecio = _config.GetValue<short>("ValoresDefectoCliCliente:NumPrecio");
                    //        cli.ExentoRenta = _config.GetValue<short>("ValoresDefectoCliCliente:ExentoRenta");
                    //        cli.Credito = _config.GetValue<short>("ValoresDefectoCliCliente:Credito");
                    //        cli.Comision = _config.GetValue<float>("ValoresDefectoCliCliente:Comision");

                    //        clientesModificados.Add(cli);
                    //        //var response2 = clienteRepository.InsertCisCliente(cli);
                    //    }
                    //}

                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
