using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Marca
    {
        public List<Marca> Listar()
        {

            List<Marca> lista = new List<Marca>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            string consulta = "select IdMarca, Descripcion, Activo from Marca ";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new Marca()
                    {
                        IdMarca = Convert.ToInt32(dr["IdMarca"]),
                        Descripcion = dr["Descripcion"].ToString(),
                        Activo = Convert.ToBoolean(dr["Activo"])
                    }

                    );
            }

            return lista;
        }

        public int Registrar(Marca obj, out string mensaje)
        {
            int IdAutogenerado = 0;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_AgregarMarca", con);
            cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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

        public bool Editar(Marca obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_EditarMarca", con);
            cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
            cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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

        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_EliminarMarca", con);
            cmd.Parameters.AddWithValue("IdMarca", id);
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return resultado;
        }

        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {

            List<Marca> lista = new List<Marca>();

            SqlConnection con = new SqlConnection(Conexion.cn);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select distinct m.IdMarca,m.Descripcion from producto p");
            sb.AppendLine("join Categoria c on c.IdCategoria = p.CategoriaId");
            sb.AppendLine("join Marca m on m.IdMarca = p.MarcaId and m.Activo = 1");
            sb.AppendLine("where c.IdCategoria = iif(@IdCategoria = 0, c.IdCategoria, @IdCategoria)");

            SqlCommand cmd = new SqlCommand(sb.ToString(), con);
            cmd.Parameters.AddWithValue("@IdCategoria",idCategoria);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new Marca()
                    {
                        IdMarca = Convert.ToInt32(dr["IdMarca"]),
                        Descripcion = dr["Descripcion"].ToString(),
                        
                    }

                    );
            }

            return lista;
        }
    }
}
