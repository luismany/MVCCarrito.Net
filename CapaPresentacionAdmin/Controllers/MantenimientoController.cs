using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }

        public ActionResult Producto()
        {
            return View();
        }
        #region Categoria
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> olista = new List<Categoria>();
            olista = new CN_Categoria().Listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria oCategoria)
        {
            object resultado;
            string mensaje = string.Empty;

            if (oCategoria.IdCategoria == 0)
                resultado = new CN_Categoria().Registrar(oCategoria, out mensaje);
            else
                resultado = new CN_Categoria().Editar(oCategoria, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region Marca
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> olista = new List<Marca>();
            olista = new CN_Marca().Listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca oMarca)
        {
            object resultado;
            string mensaje = string.Empty;

            if (oMarca.IdMarca == 0)
                resultado = new CN_Marca().Registrar(oMarca, out mensaje);
            else
                resultado = new CN_Marca().Editar(oMarca, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region Producto
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> olista = new List<Producto>();
            olista = new CN_Producto().Listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;

            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-NI"), out precio))

                oProducto.Precio = precio;
            else
                return Json(new { operacion_exitosa = false, mensaje = "El formato del plrecio debe ser ##.##" }, JsonRequestBehavior.AllowGet);


            if (oProducto.IdProducto == 0)
            { 
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

            if (idProductoGenerado != 0)
                oProducto.IdProducto = idProductoGenerado;
            else
                operacion_exitosa = false;
            }
            else
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);

            if(operacion_exitosa)
            {
                if(archivoImagen != null)
                {
                    string rutaGuardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extencionImagen = Path.GetExtension(archivoImagen.FileName);
                    string nombreImagen = string.Concat(oProducto.IdProducto.ToString(), extencionImagen);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(rutaGuardar,nombreImagen));
                    }
                    catch(Exception ex)
                    {
                      string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {
                        oProducto.RutaImagen = rutaGuardar;
                        oProducto.NombreImagen = nombreImagen;
                        bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                        mensaje = "se guardo el producto pero hubo un problema con la imagen";
                }
 
            }
            return Json(new { operacion_exitosa = operacion_exitosa, idGenerado=oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}