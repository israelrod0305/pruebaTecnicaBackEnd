using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Entities
{
    public class ResponseModel<T>
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }
        public Object tabla { get; set; }

    }

}
