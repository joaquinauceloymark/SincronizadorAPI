namespace SincronizadorAPI
{
    public class Model
    {
        public string? Cia { get; set; }
        public string? Caja { get; set; }
        public string? N_factura { get; set; }
        public string? ORDEN { get; set; }
        public string? C_SALONERO { get; set; }
        public decimal? MONTO { get; set; }
        public decimal? PROPINA { get; set; }
        public decimal? DESCUENTO { get; set; }
        public string? F_FACTURA { get; set; }
        public decimal? IMPUESTO { get; set; }
        public string? TIPO_FACT { get; set; }
        public decimal? EXTRAS { get; set; }
        public string? N_HABITACI { get; set; }
        public string? H_PEDI { get; set; } 
        public string? H_FACT { get; set; } 
        public int? N_PERSONAS { get; set; }
        public string? Hora { get; set; }
        public string? CTF_1 { get; set; }
        public string? CTF_2 { get; set; }
        public string? CTF_3 { get; set; }
        public decimal? PAGO_1 { get; set; }
        public decimal? PAGO_2 { get; set; }
        public decimal? PAGO_3 { get; set; }
        public string? C_MESA { get; set; }
        public int? estado { get; set; }
        public string? Cajero { get; set; }
        public string? C_CLIENTE { get; set; }
        public bool? LeidoFreq { get; set; } 
        public string? leidofecha { get; set; } 
        public string? CUPONDIGITAL { get; set; }
        public int? Pedido { get; set; }
        public string? C_moto { get; set; }
        public string? Extension { get; set; }
        public string? FactCaja { get; set; }
        public decimal? Vuelto { get; set; }
        public string? TELEFONO { get; set; }
        public decimal? MONTOOSA { get; set; }
        public string? Numcierre { get; set; }
        public string? C_EMPRESA { get; set; }
        public string? C_EMPLEADO { get; set; }
        public int? ENNUBE { get; set; }
        public string? N_FISCAL { get; set; }
        public string? NombreArchivo { get; set; }
        public string? Serie { get; set; }
        public List<DetalleModel> Detalle { get; set; }
        public string Tienda { get; set; }
    }
}
