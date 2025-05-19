using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Dto
{
    public class LoteDto
    {
        public int? LoteID { get; set; }

        [Required(ErrorMessage = "ProductoID es requerido")]
        public int ProductoID { get; set; }

        public string Producto { get; set; }


        public string CodigoLote { get; set; }

        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Número de lote es requerido")]
        public string NumeroLote { get; set; }

        public DateTime? FechaFabricacion { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [Required(ErrorMessage = "Cantidad inicial es requerida")]
        public int CantidadInicial { get; set; }

        public string Usuario { get; set; }

        public int Estado { get; set; }

        [Required(ErrorMessage = "Acción es requerida")]
        public string Accion { get; set; }
    }

}
