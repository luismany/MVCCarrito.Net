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
    public class CD_Cliente
    {

        public int Registrar(Cliente obj, out string mensaje)
        {
            int IdAutogenerado = 0;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", con);
            cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
            cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
            cmd.Parameters.AddWithValue("Correo", obj.Correo);
            cmd.Parameters.AddWithValue("Clave", obj.Clave);
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();

            IdAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return IdAutogenerado;

        }
        public List<Cliente> Listar()
        {

            List<Cliente> lista = new List<Cliente>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            string consulta = "select IdCliente, Nombres, Apellidos, " +
                "Correo, Clave, Restablecer from Cliente";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new Cliente()
                    {
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        Nombres = dr["Nombres"].ToString(),
                        Apellidos = dr["Apellidos"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        Clave = dr["Clave"].ToString(),
                        Restablecer = Convert.ToBoolean(dr["Restablecer"])
                    }

                    );
            }

            return lista;
        }

        public bool CambiarClave(int idCliente, string nuevaClave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("update Cliente set Clave=@nuevaClave, Restablecer=0 where IdCliente=@idCliente ", con);
            cmd.Parameters.AddWithValue("@idCliente", idCliente);
            cmd.Parameters.AddWithValue("@nuevaClave", nuevaClave);
            cmd.CommandType = CommandType.Text;
            con.Open();
            resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
            return resultado;
        }
        public bool RestablecerClave(int idCliente, string clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("update Cliente set Clave=@clave, Restablecer=1 where IdCliente=@idCliente ", con);
            cmd.Parameters.AddWithValue("@idCliente", idCliente);
            cmd.Parameters.AddWithValue("@clave", clave);
            cmd.CommandType = CommandType.Text;
            con.Open();
            resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
            return resultado;
        }
    }
}
