using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio;
using CapaEntidad;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult RestablcerClave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if(objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            else
            {
                resultado = new CN_Cliente().Registrar(objeto, out mensaje);
            }

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Login","Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
   
        }
        [HttpPost]
        public ActionResult Login(string correo, string clave)
        {
            Cliente oCliente = null;

            oCliente = new CN_Cliente().Listar().Where(c => c.Correo == correo && c.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oCliente == null) 
            {
                ViewBag.Error="Correo o Contraseña incorrectos"; 
                return View();
            }
            else
            {
                if (oCliente.Restablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }

        }
        [HttpPost]
        public ActionResult CambiarClave(string idCliente, string clave, string nuevaClave, string confirmarClave)
        {
            Cliente oCliente = new Cliente();
            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(idCliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.ConvertirSha256(clave))
            {
                TempData["IdUsuario"] = oCliente.IdCliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La Contraseña Actual es Incorrecta";
                return View();
            }
            else if (nuevaClave != confirmarClave)
            {
                TempData["IdUsuario"] = oCliente.IdCliente;
                ViewData["vclave"] = clave;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaClave = CN_Recursos.ConvertirSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idCliente), nuevaClave, out mensaje);

            if (respuesta) return RedirectToAction("Login","Acceso");
            else
            {
                TempData["IdCliente"] = oCliente.IdCliente;
                ViewBag.Error = mensaje;
                return View();
            }
        }
        [HttpPost]
        public ActionResult RestablcerClave(string correo)
        {
            Cliente oCliente = new Cliente();
            oCliente = new CN_Cliente().Listar().Where(c => c.Correo == correo).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "No se encontro ningun Cliente relacionado a este correo electronico";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().RestablecerClave(oCliente.IdCliente, correo, out mensaje);

            if (respuesta) return RedirectToAction("Login", "Acceso");
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
            
        }

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Tienda");
        }
    }

}