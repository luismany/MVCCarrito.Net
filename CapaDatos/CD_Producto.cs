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
    public class CD_Producto
    {
        public List<Producto> Listar()
        {

            List<Producto> lista = new List<Producto>();

            SqlConnection con = new SqlConnection(Conexion.cn);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select p.IdProducto,p.Nombre,p.Descripcion,");
            sb.AppendLine("c.IdCategoria,c.Descripcion[DesCategoria],");
            sb.AppendLine("m.IdMarca,m.Descripcion[DesMarca],"); 
            sb.AppendLine("p.Precio,p.Stock,p.RutaImagen,p.NombreImagen,p.Activo");
            sb.AppendLine("from Producto p");
            sb.AppendLine("inner join Categoria c on c.IdCategoria=p.CategoriaId");
            sb.AppendLine("inner join Marca m on m.IdMarca=p.MarcaId");
            

            SqlCommand cmd = new SqlCommand(sb.ToString(), con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            
                while (dr.Read())
                {
                    lista.Add(
                        new Producto()
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            oCategoria = new Categoria { IdCategoria = Convert.ToInt32(dr["IdCategoria"]), Descripcion = dr["DesCategoria"].ToString() },
                            oMarca = new Marca() { IdMarca = Convert.ToInt32(dr["IdMarca"]), Descripcion = dr["DesMarca"].ToString() },
                            Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-NI")),
                            Stock = Convert.ToInt32(dr["Stock"]),
                            RutaImagen=dr["RutaImagen"].ToString(),
                            NombreImagen=dr["NombreImagen"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        });
                }
            

            return lista;
        }

        public int Registrar(Producto obj, out string mensaje)
        {
            int IdAutogenerado = 0;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_AgregarProducto", con);
            cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
            cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
            cmd.Parameters.AddWithValue("CategoriaId", obj.oCategoria.IdCategoria);
            cmd.Parameters.AddWithValue("MarcaId", obj.oMarca.IdMarca);
            cmd.Parameters.AddWithValue("Precio", obj.Precio);
            cmd.Parameters.AddWithValue("Stock", obj.Stock);
            cmd.Parameters.AddWithValue("Activo", obj.Activo);
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            IdAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return IdAutogenerado;

        }
        public bool Editar(Producto obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_EditarProducto", con);
            cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
            cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
            cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
            cmd.Parameters.AddWithValue("CategoriaId", obj.oCategoria.IdCategoria);
            cmd.Parameters.AddWithValue("MarcaId", obj.oMarca.IdMarca);           
            cmd.Parameters.AddWithValue("Precio", obj.Precio);
            cmd.Parameters.AddWithValue("Stock", obj.Stock);
            cmd.Parameters.AddWithValue("Activo", obj.Activo);
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return resultado;
        }

        public bool GuardarDatosImagen(Producto obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);

            string consulta = "update Producto set RutaImagen= @RutaImagen, NombreImagen= @NombreImagen where IdProducto=@IdProducto";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@RutaImagen", obj.RutaImagen);
            cmd.Parameters.AddWithValue("@NombreImagen", obj.NombreImagen);
            cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
            cmd.CommandType = CommandType.Text;
            con.Open();

            if (cmd.ExecuteNonQuery() > 0)
                resultado = true;
            else
                mensaje = "No se pudo actualizar la Imagen";


            return resultado;


        }

        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_EliminarProducto", con);
            cmd.Parameters.AddWithValue("IdProducto", id);
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return resultado;
        }


    }
}
