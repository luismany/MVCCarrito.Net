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
           // List<Producto> lista = new List<Producto>();

           var lista = new CN_Producto().Listar().Select(p => new Producto
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
    }
}