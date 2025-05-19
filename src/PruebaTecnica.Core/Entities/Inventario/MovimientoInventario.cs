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
    public class MovimientoInventario : Entity<int>
    {

        [Required]
        public int ProductoID { get; set; }

        public int? LoteID { get; set; }

        [Required, MaxLength(20)]
        public string TipoMovimiento { get; set; } // Entrada, Salida, Ajuste

        [Required]
        public int Cantidad { get; set; }

        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [Required, MaxLength(50)]
        public string UsuarioResponsable { get; set; }

        [MaxLength(100)]
        public string Referencia { get; set; }

        [MaxLength(255)]
        public string Comentarios { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto Producto { get; set; }

        [ForeignKey("LoteID")]
        public virtual Lote Lote { get; set; }
    }
}
