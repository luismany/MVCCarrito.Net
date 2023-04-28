using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {

            List<Usuario> lista = new List<Usuario>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            string consulta = "select IdUsuario, Nombres, Apellidos, " +
                "Correo, Clave, Restablecer, Activo from Usuario";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new Usuario()
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Nombres = dr["Nombres"].ToString(),
                        Apellidos= dr["Apellidos"].ToString(),
                        Correo= dr["Correo"].ToString(),
                        Clave= dr["Clave"].ToString(),
                        Restablecer= Convert.ToBoolean(dr["Restablecer"]),
                        Activo= Convert.ToBoolean(dr["Activo"])
                    }

                    );
            }


            return lista;
        }

        public int Registrar(Usuario obj, out string mensaje)
        {
            int IdAutogenerado = 0;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_AgregarUsuario",con);
            cmd.Parameters.AddWithValue("Nombres",obj.Nombres);
            cmd.Parameters.AddWithValue("Apellidos",obj.Apellidos);
            cmd.Parameters.AddWithValue("Correo",obj.Correo);
            cmd.Parameters.AddWithValue("Clave",obj.Clave);
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

        public bool Editar(Usuario obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_EditarUsuario", con);
            cmd.Parameters.AddWithValue("IdUsuario",obj.IdUsuario);
            cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
            cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
            cmd.Parameters.AddWithValue("Correo",obj.Correo);
            cmd.Parameters.AddWithValue("Activo",obj.Activo);
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return resultado;
        }

        public bool Eliminar(int id, out string mernsaje)
        {
            bool resultado = false;
            mernsaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("delete top(1) from Usuario where IdUsuario=@IdUsuario",con);
            cmd.Parameters.AddWithValue("@IdUsuario",id);
            cmd.CommandType = CommandType.Text;
            con.Open();
            // ExecuteNonQuery() devuelve el total de filas afectadas
            
            resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

            return resultado;
        }
    }
}
