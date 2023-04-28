using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Usuario obj, out string mensaje)
        {

            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
                mensaje = "El nombre de usuario no puede estar vacio";
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
                mensaje = "El Apellido del usuario es obligatorio";
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
                mensaje = " El correo es obligatorio";

            if (string.IsNullOrEmpty(mensaje))
            {
                string clave = "test123";
                obj.Clave = CN_Recursos.ConvertirSha256(clave);
                return objCapaDato.Registrar(obj, out mensaje);
            }
            else
                return 0;

        }

        public bool Editar(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
                mensaje = "El nombre de usuario no puede estar vacio";
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
                mensaje = "El Apellido del usuario es obligatorio";
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
                mensaje = " El correo es obligatorio";

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

    }
}
