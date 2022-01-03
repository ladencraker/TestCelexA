using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCelexA.Models.LisViewModels
{
    public class ViewModelAlumno
    {
        
        public int Alumno_Id { get; set; }
        
        public string Alumno_Email { get; set; }
        public string Alumno_Pass { get; set; }
        public string Alumno_Nombre { get; set; }
        public string Alumno_AP { get; set; }
        public string Alumno_AM { get; set; }
        public DateTime Alumno_FehcaIngreso { get; set; }
        public Boolean Alumno_Activo { get; set; }

    }
}