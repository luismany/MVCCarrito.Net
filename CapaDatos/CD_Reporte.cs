using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Reporte
    {

        public List<ReporteVentas> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<ReporteVentas> lista = new List<ReporteVentas>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_ReporteVentas", con);
            cmd.Parameters.AddWithValue("FechaInicio",fechaInicio);
            cmd.Parameters.AddWithValue("FechaFin",fechaFin);
            cmd.Parameters.AddWithValue("IdTransaccion",idTransaccion);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                lista.Add(
                    new ReporteVentas()
                    {
                        FechaVenta = dr["FechaVenta"].ToString(),
                        Cliente=dr["Cliente"].ToString(),
                        Producto=dr["Producto"].ToString(),
                        Precio=Convert.ToDecimal(dr["Precio"]),
                        Cantidad=Convert.ToInt32(dr["Cantidad"]),
                        Total=Convert.ToDecimal(dr["Total"]),
                        IdTransaccion=dr["IdTransaccion"].ToString()
                    }

                    ) ;
            }

            return lista;
        }


        public Dashboard VerDashboard()
        {
            Dashboard objeto = new Dashboard();

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                objeto = new Dashboard()
                {

                    TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                    TotalVenta=Convert.ToInt32(dr["TotalVenta"]),
                    TotalProducto=Convert.ToInt32(dr["TotalProducto"])
                };
  
            }
            return objeto;
        }
    }
}
