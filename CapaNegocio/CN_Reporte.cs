﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objCapaDato = new CD_Reporte();

        public Dashboard VerDashboard()
        {
            return objCapaDato.VerDashboard();
        }

        public List<ReporteVentas> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            return objCapaDato.Ventas(fechaInicio,fechaFin,idTransaccion);
        }
    }
}
