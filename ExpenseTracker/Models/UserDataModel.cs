using System.Collections.Generic;

namespace ExpenseTracker.Models
{
    public class UserDataModel : ParentModelView
    {
        public string UserDataId { get; set; }
        public string UserEmail { get; set; }
        public string Objetivo { get; set; }
        public string Disponible { get; set; }
        public string GastoMes { get; set; }
        public string GastoSemana { get; set; }
        public List<IngresosModel> Ingresos { get; set; }
        public List<GastosModel> Gastos { get; set; }
        public List<EventoModel> Eventos { get; set; }
    }
}