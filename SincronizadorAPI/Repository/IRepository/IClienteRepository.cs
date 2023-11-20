using SincronizadorAPI.Models;

namespace SincronizadorAPI.Repository.IRepository
{
    public interface IClienteRepository
    {
        public ICollection<cli_clientes> GetClientes();
        string GetUltimoCliente();
        bool InsertCisCliente(clientes cliente);
        void UpdateCliProcesado(cli_clientes cliente);
        clientes GetCisClienteByCedula(string cLI_IDENTIFICACION);
        void UpdateSaldoCisCliente(string cedula, decimal cLI_PUNTOSDISPONIBLES);
        public ICollection<cli_clientes> GetClientesAll();
    }
}
