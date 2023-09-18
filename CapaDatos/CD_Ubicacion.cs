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
    public class CD_Ubicacion
    {
        public List<Departamento> ListaDepartamento()
        {

            List<Departamento> lista = new List<Departamento>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            string consulta = "select * from Departamento";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new Departamento()
                    {
                        IdDepartamento = dr["IdDepartamento"].ToString(),
                        Descripcion = dr["Descripcion"].ToString()

                    }

                    );
            }
            return lista;
        }

        public List<Provincia> ListaProvincia(string idDepartamento)
        {

            List<Provincia> lista = new List<Provincia>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            string consulta = "select * from Provincia where DepartamentoId=@DepartamentoId";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@DepartamentoId",idDepartamento);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new Provincia()
                    {
                        IdProvincia = dr["IdProvincia"].ToString(),
                        Descripcion = dr["Descripcion"].ToString()

                    }

                    );
            }
            return lista;
        }

        public List<Distrito> ListaDistrito(string idDepartamento,string idProvinvia)
        {

            List<Distrito> lista = new List<Distrito>();

            SqlConnection con = new SqlConnection(Conexion.cn);
            string consulta = "select * from Distrito where IdDepartamento=@IdDepartamento and IdProvincia=@IdProvincia";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@IdDepartamento", idDepartamento);
            cmd.Parameters.AddWithValue("@IdProvincia",idProvinvia);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new Distrito()
                    {
                        IdDistrito = dr["IdDistrito"].ToString(),
                        Descripcion = dr["Descripcion"].ToString()

                    }

                    );
            }
            return lista;
        }
    }
}
