using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Dto
{
    public class LoteRequest
    {
        public int? LoteID { get; set; }
        public int? ProductoID { get; set; }
        public string NumeroLote { get; set; }
        public DateTime? FechaFabricacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? CantidadInicial { get; set; }
        public string Usuario { get; set; }
        public string Estado { get; set; }
        public string Accion { get; set; }
    }

}
