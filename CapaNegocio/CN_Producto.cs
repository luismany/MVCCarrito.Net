using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {

        private CD_Producto objCapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Producto obj, out string mensaje)
        {

            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
                mensaje = "El nombre del producto no puede estar vacio";
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
                mensaje = "La descripcion de la categoria no puede estar vacio";
            else if (obj.oMarca.IdMarca == 0) 
                mensaje = "Debe seleccionar una Marca";
            else if (obj.oCategoria.IdCategoria == 0)
                mensaje = "Debe seleccionar una Categoria";
            else if (obj.oCategoria.IdCategoria == 0)
                mensaje = "Debe seleccionar una Categoria";
            else if (obj.Precio == 0)
                mensaje = "Debe ingresar un precio";
            else if (obj.Stock == 0)
                mensaje = "Debe ingresar un stock";

            if (string.IsNullOrEmpty(mensaje))
            {
                return objCapaDato.Registrar(obj, out mensaje);
            }
            else
                return 0;
            
        }

        public bool Editar(Producto obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
                mensaje = "El nombre del producto no puede estar vacio";
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
                mensaje = "La descripcion de la categoria no puede estar vacio";
            else if (obj.oCategoria.IdCategoria == 0)
                mensaje = "Debe seleccionar una Categoria";
            else if (obj.oMarca.IdMarca == 0)
                mensaje = "Debe seleccionar una Marca";  
            else if (obj.Precio == 0)
                mensaje = "Debe ingresar un precio";
            else if (obj.Stock == 0)
                mensaje = "Debe ingresar un stock";

            if (string.IsNullOrEmpty(mensaje))
            {
                return objCapaDato.Editar(obj, out mensaje);
            }
            else
                return false;

        }
        public bool GuardarDatosImagen(Producto obj, out string mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj , out mensaje);
        }

        public bool Eliminar(int id, out string mernsaje)
        {
            return objCapaDato.Eliminar(id, out mernsaje);
        }
    }
}
