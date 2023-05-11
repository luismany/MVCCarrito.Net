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
    public class CD_Categoria
    {
        public List<Categoria> Listar()
        {

            List<Categoria> lista = new List<Categoria>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            string consulta = "select IdCategoria, Descripcion, Activo from Categoria ";   
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new Categoria()
                    {
                        IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                        Descripcion = dr["Descripcion"].ToString(),
                        Activo = Convert.ToBoolean(dr["Activo"])
                    }

                    );
            }


            return lista;
        }

        public int Registrar(Categoria obj, out string mensaje)
        {
            int IdAutogenerado = 0;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_AgregarCategoria", con);
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

        public bool Editar(Categoria obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_EditarCategoria", con);
            cmd.Parameters.AddWithValue("IdCategoria", obj.IdCategoria);
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

        public bool Eliminar( int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", con);
            cmd.Parameters.AddWithValue("IdCategoria", id);
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
