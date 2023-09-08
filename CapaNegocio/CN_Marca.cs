using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objCapaDato = new CD_Marca();
        public List<Marca> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Marca obj, out string mensaje)
        {

            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
                mensaje = "La descripcion de la Marca no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {
                return objCapaDato.Registrar(obj, out mensaje);
            }
            else
                return 0;
        }
        public bool Editar(Marca obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
                mensaje = "La descripcion de la Marca no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {
                return objCapaDato.Editar(obj, out mensaje);
            }
            else
                return false;

        }
        public bool Eliminar(int id, out string mernsaje)
        {
            return objCapaDato.Eliminar(id, out mernsaje);
        }

        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {
            return objCapaDato.ListarMarcaPorCategoria(idCategoria);
        }
    }
}
