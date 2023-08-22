using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
//using de autenticacion
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult RestablecerClave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string correo,string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();
            if (oUsuario == null)
            {
                ViewBag.Error = "El Correo o la contraseña son invalidos";
                return View();
            }
            else
            {
                if (oUsuario.Restablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                //autenticacion de usuario
                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index","Home");
            }

           
        }
        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string clave, string nuevaClave, string confirmarClave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            if(oUsuario.Clave != CN_Recursos.ConvertirSha256(clave))
            {
                TempData["IdUsuario"] = oUsuario.IdUsuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La Contraseña Actual es Incorrecta";
                return View();
            }
            else if(nuevaClave != confirmarClave)
            {
                TempData["IdUsuario"] = oUsuario.IdUsuario;
                ViewData["vclave"] = clave;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaClave = CN_Recursos.ConvertirSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave( int.Parse(idUsuario), nuevaClave, out mensaje);

            if (respuesta) return RedirectToAction("Index");
            else
            {
                TempData["IdUsuario"] = oUsuario.IdUsuario;
                ViewBag.Error = mensaje;
                return View();
            }
 
        }
        [HttpPost]
        public ActionResult RestablecerClave(string correo)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo).FirstOrDefault();

            if(oUsuario == null)
            {
                ViewBag.Error = "No se encontro ningun usuario relacionado a este correo electronico";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().RestabkecerClave(oUsuario.IdUsuario, correo, out mensaje);

            if (respuesta) return RedirectToAction("Index","Acceso");
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Acceso");
        }
    }
}