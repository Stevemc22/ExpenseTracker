using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTracker.Models
{
    public class ParentModelView
    {
        public UserDataModel UserDataModel { get; set; }
        public IngresosModel IngresosModel { get; set; }
        public GastosModel GastosModel { get; set; }
    }
}