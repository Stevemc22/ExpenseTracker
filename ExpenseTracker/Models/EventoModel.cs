using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseTracker.Models
{
    public class EventoModel : ParentEventModel
    {
        [Required]
        [DisplayName("Detalle")]
        public string title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public string start { get; set; }

        [Required]
        public string Monto { get; set; }
    }
}