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
    public class Stock : Entity<int>
    {
        [Required]
        public int ProductoID { get; set; }

        public int? LoteID { get; set; }


        [Required]
        public int CantidadDisponible { get; set; }

        [MaxLength(50)]
        public string Ubicacion { get; set; }

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        [MaxLength(50)]
        public string UsuarioCreacion { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto Producto { get; set; }

        [ForeignKey("LoteID")]
        public virtual Lote Lote { get; set; }
    }
}
