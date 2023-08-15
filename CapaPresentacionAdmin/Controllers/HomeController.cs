using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

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
        [HttpPost]
        public JsonResult EliminarUsuarios(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard objeto = new CN_Reporte().VerDashboard();

            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult ListaReporte(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<ReporteVentas> oLista = new List<ReporteVentas>();
            
            oLista = new CN_Reporte().Ventas(fechaInicio,fechaFin,idTransaccion);

            return Json(new {data=oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<ReporteVentas> olista = new List<ReporteVentas>();
            olista = new CN_Reporte().Ventas(fechaInicio,fechaFin,idTransaccion);

            DataTable dt = new DataTable();
            dt.Locale = new CultureInfo("es-NI");
            dt.Columns.Add("Fecha Venta",typeof(string));
            dt.Columns.Add("Cliente",typeof(string));
            dt.Columns.Add("Producto",typeof(string));
            dt.Columns.Add("Precio",typeof(decimal));
            dt.Columns.Add("Cantidad",typeof(int));
            dt.Columns.Add("Total",typeof(decimal));
            dt.Columns.Add("IdTransaccion",typeof(string));

            foreach (ReporteVentas rv in olista)
            {
                dt.Rows.Add(new object[] {
                    rv.FechaVenta,
                    rv.Cliente,
                    rv.Producto,
                    rv.Precio,
                    rv.Cantidad,
                    rv.Total,
                    rv.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt);

            MemoryStream stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVentas" + DateTime.Now.ToString()+".xlsx");
        }


    }
}