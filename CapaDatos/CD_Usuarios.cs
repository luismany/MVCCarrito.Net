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
            string consulta = "select IdUsuario, Nombres, Apellidos, Correo, Clave, Restablecer, Activo from Usuario";
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
    }
}
