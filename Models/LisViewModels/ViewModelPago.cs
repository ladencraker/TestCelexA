using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCelexA.Models.LisViewModels
{
    public class ViewModelPago
    {
        public string Pago_referencia { get; set; }
        public int? Pago_monto { get; set; }
        public string Pago_ruta { get; set; }
        public int Alumno { get; set; }
    }
}