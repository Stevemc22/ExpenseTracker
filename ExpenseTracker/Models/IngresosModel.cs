using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class IngresosModel : ParentModelView
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required]
        public string Concepto { get; set; }

        [Required]
        public string Monto { get; set; }
    }
}