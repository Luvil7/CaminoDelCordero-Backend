using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploClase 
{ 
    public class DbHandler // para ahcer que las otras clases hereden de esta y tengan el coneection string
    {
        public const string ConnectionString =
        "Server=DESKTOP-LUCAS1\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";
    }
}
