using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SincronizadorAPI.Data;
using SincronizadorAPI.Models;
using SincronizadorAPI.Repository.IRepository;

namespace SincronizadorAPI.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        ApplicationDbContext _db;



        public ClienteRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public ICollection<cli_clientes> GetClientes()
        {
            var clientes = _db.Set<cli_clientes>().FromSqlRaw("EXEC sp_GetClientesProcesados").ToList();

            return clientes;

            //return _db.cli_clientes
            //    .Where(c => c.CLI_CEDULA == "114770929" && c.CLI_PROCESADO == "S")
            //    .Select(c => new cli_clientes
            //    {
            //        CLI_IDENTIFICACION = c.CLI_IDENTIFICACION,
            //        CLI_NOMBRE = c.CLI_NOMBRE ?? string.Empty,
            //        CLI_CEDULA = c.CLI_CEDULA ?? string.Empty,
            //        CLI_PROCESADO = c.CLI_PROCESADO ?? string.Empty,
            //        CLI_CATEGORIACLIENTE = c.CLI_CATEGORIACLIENTE ?? string.Empty,
            //        CLI_CODIGOENCLIENTE = c.CLI_CODIGOENCLIENTE ?? string.Empty,
            //        CLI_TELEFONOCELULAR = c.CLI_TELEFONOCELULAR ?? string.Empty,
            //        CLI_FAX = c.CLI_FAX ?? string.Empty,
            //        CLI_EMAIL = c.CLI_EMAIL ?? string.Empty,
            //        CLI_CODIGOPOSTAL = c.CLI_CODIGOPOSTAL ?? string.Empty,
            //        CLI_PUNTOSDISPONIBLES = c.CLI_PUNTOSDISPONIBLES,
            //        CREATEUSERID = c.CREATEUSERID ?? string.Empty
            //    })
            //    .Take(100)
            //    .ToList();
        }

        public string GetUltimoCliente()
        {
            var maxCia = _db.Clientes
        .AsEnumerable()
        .Select(c =>
        {
            bool success = int.TryParse(c.Cia, out int number);
            return new { Number = number, IsValid = success };
        })
        .Where(c => c.IsValid)
        .Max(c => c.Number);

            return maxCia.ToString();
        }

        public bool InsertCisCliente(clientes cliente)
        {
            _db.Clientes.Add(cliente);
            return _db.SaveChanges() > 0 ? true : false;
        }

        public void UpdateCliProcesado(decimal identificacion)
        {
            try
            {
                var existingCliente = _db.cli_clientes.Find(identificacion);
                if (existingCliente != null)
                {
                    existingCliente.CLI_PROCESADO = "N";
                    _db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public clientes GetCisClienteByCedula(string Cedula)
        {
            return _db.Clientes.Where(x => x.Codigo == Cedula).FirstOrDefault();
        }

        public void UpdateSaldoCisCliente(string cedula, decimal PuntosAsignar)
        {
            try
            {
                var parametros = new SqlParameter[]
                {
                    new SqlParameter("@Cedula", cedula),
                    new SqlParameter("@PuntosAsignar", (float)PuntosAsignar)
                };

                _db.Database.ExecuteSqlRaw("EXEC sp_actualizarPuntosAsignar @Cedula, @PuntosAsignar", parametros);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ICollection<cli_clientes> GetClientesAll()
        {
            var clientes = _db.Set<cli_clientes>().FromSqlRaw("EXEC sp_GetClientesUltimaHora").ToList();

            return clientes;
        }
    }
}
