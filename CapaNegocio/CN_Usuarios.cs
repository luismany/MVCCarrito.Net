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
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Creacion de Cuenta";
                string mensajeCorreo = "<h3>Su cuenta fue creada correctaente</h3></br>" +
                    "<p>Su contraseña para acceder es: </p>"+ clave;
                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo,asunto,mensajeCorreo);

                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDato.Registrar(obj, out mensaje);
                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return 0;
                }

               
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

        public bool CambiarClave(int idUsuario, string nuevaClave,out string mensaje)
        {
            return objCapaDato.CambiarClave(idUsuario,nuevaClave,out mensaje);
        }

        public bool RestabkecerClave(int idUsuario,string correo, out string mensaje)
        {

            mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.RestablecerClave(idUsuario,CN_Recursos.ConvertirSha256(nuevaClave),out mensaje);

            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensajeCorreo = "<h3>Su cuenta fue restablecida correctaente</h3></br>" +
                    "<p>Su contraseña para acceder ahora es: </p>" + nuevaClave;
                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                if (resultado) return true;
                else
                {
                    mensaje = "No se pudo eviar el correo";
                    return false;
                }

            }
            else return false;
            

        }


    }
}
