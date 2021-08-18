using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTracker.Models
{
    public class ParentEventModel
    {
        public ParentEventModel()
        {
            eventos = new List<EventoModel>();
        }
        public List<EventoModel> eventos { get; set; }
        public EventoModel evento { get; set; }
    }
}