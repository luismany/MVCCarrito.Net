using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> olista = new List<Usuario>();
            olista = new CN_Usuarios().Listar();

            return Json(new {data= olista } , JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarUsuarios(Usuario oUsuario)
        {
            object resultado;
            string mensaje = string.Empty;

            if (oUsuario.IdUsuario == 0)
                resultado = new CN_Usuarios().Registrar(oUsuario, out mensaje);
            else
                resultado = new CN_Usuarios().Editar(oUsuario, out mensaje);

            return Json(new {resultado= resultado, mensaje=mensaje }, JsonRequestBehavior.AllowGet);

        }



    }
}