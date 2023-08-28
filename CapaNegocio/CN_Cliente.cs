using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDato = new CD_Cliente();

        public int Registrar(Cliente obj, out string mensaje)
        {

            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
                mensaje = "El nombre de cliente no puede estar vacio";
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
                mensaje = "El Apellido del cliente es obligatorio";
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
                mensaje = " El correo es obligatorio";

            if (string.IsNullOrEmpty(mensaje))
            {
                obj.Clave = CN_Recursos.ConvertirSha256(obj.Clave);
                return objCapaDato.Registrar(obj, out mensaje);
              
            }
            else
                return 0;

        }

        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }

        public bool CambiarClave(int idCliente, string nuevaClave, out string mensaje)
        {
            return objCapaDato.CambiarClave(idCliente, nuevaClave, out mensaje);
        }

        public bool RestablecerClave(int idCliente, string correo, out string mensaje)
        {

            mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.RestablecerClave(idCliente, CN_Recursos.ConvertirSha256(nuevaClave), out mensaje);

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
