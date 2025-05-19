using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Dto
{
    public class ResultadoDto
    {
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public int? Id { get; set; } // Solo para INS
    }
}
