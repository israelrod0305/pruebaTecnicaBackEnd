using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Core
{
    public class UnidadMedida : Entity<int>
    {
       
        [Required, MaxLength(10)]
        public string Codigo { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string Descripcion { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
