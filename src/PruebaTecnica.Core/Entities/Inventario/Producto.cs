using Abp.Domain.Entities;
using PruebaTecnica.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Core
{ 
    public class Producto : Entity<int>
    {
        

        [Required, MaxLength(50)]
        public string CodigoProducto { get; set; }

        [Required, MaxLength(100)]
        public string NombreProducto { get; set; }

        [MaxLength(255)]
        public string Descripcion { get; set; }

        public int? CategoriaID { get; set; }

        public int? ProveedorID { get; set; }

        [Required]
        public int UnidadMedidaID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrecioCompra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrecioVenta { get; set; }

        public int StockMinimo { get; set; } = 0;

        public int? StockMaximo { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string UsuarioCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        [MaxLength(50)]
        public string UsuarioModificacion { get; set; }

        // Relaciones
        [ForeignKey("UnidadMedidaID")]
        public virtual UnidadMedida UnidadMedida { get; set; }

        public virtual ICollection<Lote> Lotes { get; set; }

        public virtual ICollection<MovimientoInventario> MovimientosInventario { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
