using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestCelexA.Models.EF;
using TestCelexA.Models.LisViewModels;

namespace TestCelexA.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";
          

            return View();
        }
        [HttpPost]
        public ActionResult Login(ViewModelAlumno oAlumno)
        {
           
            //List<ViewModelAlumno> list;
            ViewBag.Message = "Your contact page.";
            using (bd_CELEXAEntities db = new bd_CELEXAEntities())
            {
                /*
                list = (from d in db.TBL_OPE_ALUMNO
                      where d.Alumno_Email == oAlumno.Alumno_Email
                      && d.Alumno_Pass == oAlumno.Alumno_Pass
                      select  new ViewModelAlumno
                      {
                          Alumno_Id = d.Alumno_Id,
                          Alumno_Nombre = d.Alumno_Nombre,
                          Alumno_AP = d.Alumno_AP,
                          Alumno_AM = d.Alumno_AP
                      }).ToList();
                */

                var obj= db.TBL_OPE_ALUMNO.Where(a => a.Alumno_Email.Equals(oAlumno.Alumno_Email) && a.Alumno_Pass.Equals(oAlumno.Alumno_Pass)).FirstOrDefault();
                
                if (obj != null)
                {

                    Session["UserID"] = obj.Alumno_Id;
                    Session["UserName"] = obj.Alumno_Nombre+ " "+obj.Alumno_AP+" "+ obj.Alumno_AM;
                    Session["UserPago"] = "pagar";
                    return RedirectToAction("AlumnoInicio");
                }
                var obj2 = db.TBL_OPE_PROFESOR.Where(a=>a.Profesor_Email.Equals(oAlumno.Alumno_Email) && a.Profesor_Pass.Equals(oAlumno.Alumno_Pass)).FirstOrDefault();
                if (obj2 != null)
                {

                    Session["UserID"] = obj2.Profesor_Id;
                    Session["UserName"] = obj2.Profesor_Nombre + " " + obj2.Profesor_AP + " " + obj2.Profesor_AM;
                    Session["UserTipo"] = "p";
                    return RedirectToAction("ProfesorInicio");
                }

            }

                return View(oAlumno);
        }
        public ActionResult AlumnoInicio()
        {
            if (Session["UserID"] != null)
            {
                int UserId =  Int32.Parse(Session["UserID"].ToString());
                using (bd_CELEXAEntities db = new bd_CELEXAEntities())
                {
                    ViewBag.Idioma = (from d in db.TBL_REL_ALUMNO_PROFESOR_IDIOMA
                                     join e in db.TBL_CAT_IDIOMA on d.Idioma_Id equals e.Idioma_Id
                                     where d.Alumno_Id == UserId
                                     select e.Idioma_Nombre).FirstOrDefault().ToString();

                    ViewBag.Nivel = (from d in db.TBL_REL_ALUMNO_PROFESOR_IDIOMA
                                      join e in db.TBL_CAT_NIVEL on d.Nivel_Id equals e.Nivel_Id
                                      where d.Alumno_Id == UserId
                                      select e.Nivel_Grado).FirstOrDefault().ToString();
                    ViewBag.Grupo = (from d in db.TBL_REL_ALUMNO_PROFESOR_IDIOMA
                                     join e in db.TBL_CAT_GRUPO on d.Grupo_Id equals e.Grupo_Id
                                     where d.Alumno_Id == UserId
                                     select e.Grupo_Nombre).FirstOrDefault().ToString();

                    return View();
                }

                    
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
     
        public ActionResult AlumnoPago()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AlumnoPago(ViewModelPago opago)
        {
            if (Session["UserID"] != null)
            {
                string ruta = "";
                int UserId = Int32.Parse(Session["UserID"].ToString());
                using (bd_CELEXAEntities db = new bd_CELEXAEntities())
                {
                    
                    var obj = new TBL_OPE_PAGO();
                    obj.Pago_Referencia = opago.Pago_referencia;
                    obj.Pago_Monto = opago.Pago_monto;
                    obj.Pago_Vaucher_ruta = ruta;
                    obj.Alumno_Id = UserId;
                    db.TBL_OPE_PAGO.Add(obj);
                    db.SaveChanges();
                    Session["UserPago"] = true;
                    return RedirectToAction("AlumnoInicio");
                }
                    
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult AlumnoResultados()
        {
            if (Session["UserID"] != null)
            {
                int UserId = Int32.Parse(Session["UserID"].ToString());
                using (bd_CELEXAEntities db = new bd_CELEXAEntities())
                {
                    var list = (from d in db.TBL_OPE_ALUMNO
                               join e in db.TBL_OPE_RESULTADOS on d.Alumno_Id equals e.Alumno_Id
                               where d.Alumno_Id == UserId
                               select e).ToList();
                    foreach ( var i in list)
                    {
                        ViewBag.ME = i.Resultado_ME;
                        ViewBag.FE = i.Resultado_FE;
                        ViewBag.AVG = i.Resultado_AVG;

                    }
                }
                    return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult ProfesorInicio()
        {
            if (Session["UserID"] != null)
            {
                int UserId = Int32.Parse(Session["UserID"].ToString());
                using (bd_CELEXAEntities db = new bd_CELEXAEntities())
                {
                    

                    ViewBag.Grupo = (from d in db.TBL_REL_ALUMNO_PROFESOR_IDIOMA
                                    join e in db.TBL_CAT_GRUPO on d.Grupo_Id equals e.Grupo_Id
                                    where d.Profesor_Id ==UserId
                                    select e.Grupo_Nombre
                                     ).ToList().FirstOrDefault();
                    ViewBag.Cantidad = (from d in db.TBL_REL_ALUMNO_PROFESOR_IDIOMA
                                    where d.Profesor_Id==UserId
                                    select d.Alumno_Id
                                   ).Count();
                    
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
       
        public ActionResult ProfesorCalificaciones()
        {
            if (Session["UserID"] != null)
            {
                int UserId = Int32.Parse(Session["UserID"].ToString());
                using (bd_CELEXAEntities db = new bd_CELEXAEntities())
                {
                    var list = (from d in db.TBL_OPE_ALUMNO
                                join e in db.TBL_REL_ALUMNO_PROFESOR_IDIOMA on d.Alumno_Id equals e.Alumno_Id
                                where e.Profesor_Id == UserId
                                select new ListNombres_Alumnos
                                {
                                    Full_Name = d.Alumno_Nombre + "" + d.Alumno_AP + "" + d.Alumno_AM
                                }

                            ).ToList();
                    ViewBag.Nivel = (from d in db.TBL_REL_ALUMNO_PROFESOR_IDIOMA
                                     join e in db.TBL_CAT_NIVEL on d.Nivel_Id equals e.Nivel_Id
                                     where d.Alumno_Id == UserId
                                     select e.Nivel_Grado).FirstOrDefault().ToString();
                    ViewBag.Grupo = (from d in db.TBL_REL_ALUMNO_PROFESOR_IDIOMA
                                     join e in db.TBL_CAT_GRUPO on d.Grupo_Id equals e.Grupo_Id
                                     where d.Profesor_Id == UserId
                                     select e.Grupo_Nombre
                                     ).ToList().FirstOrDefault();
                    return View(list);
                }


                   
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

    }
}