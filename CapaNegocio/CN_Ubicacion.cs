using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDato = new CD_Ubicacion();

        public List<Departamento> ListaDepartamento()
        {
            return objCapaDato.ListaDepartamento();
        }

        public List<Provincia> ListaProvincia(string idDepartamento)
        {
            return objCapaDato.ListaProvincia(idDepartamento);
        }

        public List<Distrito> ListaDistrito(string idDepartamento, string idProvinvia)
        {
            return objCapaDato.ListaDistrito(idDepartamento,idProvinvia);
        }
    }
}
