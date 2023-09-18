using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteEnCarrito(int idCliente, int idProducto)
        {
            bool resultado = true;
          

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", con);
            cmd.Parameters.AddWithValue("ClienteId", idCliente);
            cmd.Parameters.AddWithValue("ProductoId", idProducto);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

            return resultado;

        }

        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string mensaje)
        {
            bool resultado = true;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", con);
            cmd.Parameters.AddWithValue("ClienteId", idCliente);
            cmd.Parameters.AddWithValue("ProductoId", idProducto);
            cmd.Parameters.AddWithValue("Sumar", sumar);
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            return resultado;
        }

        public int CantidadEnCarrito(int idCliente)
        {
            int resultado = 0;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("select count(*) from Carrito where ClienteId=@ClienteId", con);
            cmd.Parameters.AddWithValue("@ClienteId", idCliente);
            cmd.CommandType = CommandType.Text;
            con.Open();

            resultado =Convert.ToInt32(cmd.ExecuteScalar());

            return resultado;
        }

        public List<Carrito> ListaProductoEnCarrito(int idCliente)
        {

            List<Carrito> lista = new List<Carrito>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            string consulta = "select * from fn_ObtenerCarritoCliente(@ClienteId)";
             
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@ClienteId",idCliente);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Carrito()
                    {
                      Producto=new Producto()

                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Nombre = dr["Nombre"].ToString(),
                            Precio = Convert.ToDecimal( dr["Precio"],new CultureInfo("es-NI")),
                            RutaImagen = dr["RutaImagen"].ToString(),
                            NombreImagen = dr["NombreImagen"].ToString(),
                            oMarca=new Marca() { Descripcion=dr["DesMarca"].ToString()}
                            
                        },
                        Cantidad=Convert.ToInt32(dr["Cantidad"])
                    });
            }


            return lista;
        }

        public bool EliminarCarrito(int idCliente, int idProducto)
        {
            bool resultado = true;


            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", con);
            cmd.Parameters.AddWithValue("ClienteId", idCliente);
            cmd.Parameters.AddWithValue("ProductoId", idProducto);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

            return resultado;

        }
    }
}
