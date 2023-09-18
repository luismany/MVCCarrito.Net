using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DetalleProducto(int idProducto=0)
        {
            bool conversion;
            Producto oProducto = new Producto();

            oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == idProducto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);     
            }
            return View(oProducto);

        }
        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar();

            return Json(new{ data =lista},JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult ListarMarcaporCategoria(int idCategoria)
        {

            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaPorCategoria(idCategoria);

            return Json(new{data=lista },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductos(int idCategoria, int idMarca)
        {
            bool conversion;
            List<Producto> lista = new List<Producto>();

            lista = new CN_Producto().Listar().Select(p => new Producto
            {

                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
                        p.oCategoria.IdCategoria == (idCategoria == 0 ? p.oCategoria.IdCategoria : idCategoria) &&
                        p.oMarca.IdMarca == (idMarca == 0 ? p.oMarca.IdMarca : idMarca) &&
                        p.Stock > 0 && p.Activo == true
                        ).ToList();


            var jsonresult = Json(new{data=lista },JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idProducto)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool existe = new CN_Carrito().ExisteEnCarrito(idCliente,idProducto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe) mensaje = "El Producto ya existe en el Carrito.";
            else respuesta = new CN_Carrito().OperacionCarrito(idCliente,idProducto,true,out mensaje);

            return Json(new{respuesta=respuesta, mensaje=mensaje },JsonRequestBehavior.AllowGet);   
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idCliente);

            return Json(new { cantidad=cantidad},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarProductoCarrito()
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool conversion;

            List<Carrito> oLista = new List<Carrito>();

            oLista = new CN_Carrito().ListaProductoEnCarrito(idCliente).Select(c => new Carrito()
            {

                Producto = new Producto()
                {
                    IdProducto = c.Producto.IdProducto,
                    Nombre = c.Producto.Nombre,
                    oMarca=c.Producto.oMarca,
                    Precio = c.Producto.Precio,
                    RutaImagen = c.Producto.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(c.Producto.RutaImagen, c.Producto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(c.Producto.NombreImagen)
                },
                Cantidad = c.Cantidad


            }).ToList();

            return Json(new { data=oLista},JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idProducto,bool sumar)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idCliente,idProducto,true,out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCarrito(int idProducto)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta;
            string mensaje = string.Empty;


            respuesta = new CN_Carrito().EliminarCarrito(idCliente,idProducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();

            lista = new CN_Ubicacion().ListaDepartamento();

            return Json(new { lista=lista},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerProvincia(string idDepartamento)
        {
            List<Provincia> lista = new List<Provincia>();

            lista = new CN_Ubicacion().ListaProvincia(idDepartamento);

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerDistrito(string idDepartamento, string idProvincia)
        {
            List<Distrito> lista = new List<Distrito>();

            lista = new CN_Ubicacion().ListaDistrito(idDepartamento,idProvincia);

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Carrito()
        {
            return View();
        }
    }
}