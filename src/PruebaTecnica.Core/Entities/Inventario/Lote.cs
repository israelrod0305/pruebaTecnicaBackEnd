using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Core
{
    public class Lote : Entity<int>
    {

        [Required]
        public int ProductoID { get; set; }

        [Required, MaxLength(50)]
        public string NumeroLote { get; set; }

        public DateTime? FechaFabricacion { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [Required]
        public int CantidadInicial { get; set; }

        [Required]
        public int CantidadDisponible { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; } = "Activo";

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [ForeignKey("ProductoID")]
        public virtual Producto Producto { get; set; }

        [MaxLength(50)]
        public string UsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<MovimientoInventario> MovimientosInventario { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
