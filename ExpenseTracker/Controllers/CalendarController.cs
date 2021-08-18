using ExpenseTracker.Data;
using ExpenseTracker.Models;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        FirebaseDB firebase = new FirebaseDB();

        // GET: Calendar
        public ActionResult Index()
        {
            ParentEventModel model = new ParentEventModel();
            model.evento = new EventoModel();
            UserDataModel userDataModel = getUserData();
            decimal totalGastosEventos = 0;
            ViewBag.GastoMes = System.Web.HttpContext.Current.Session["gastoMes"];

            model.eventos = userDataModel.Eventos == null ? new List<EventoModel>() : userDataModel.Eventos.Where(x => Convert.ToDateTime(x.start).Month == DateTime.Now.Month).ToList();

            if (model.eventos != null) {
                totalGastosEventos = model.eventos.Sum(x => Decimal.Parse(x.Monto));
                ViewBag.Color = totalGastosEventos > Decimal.Parse(ViewBag.GastoMes) ? "red" : "mediumseagreen";
                ViewBag.TotalGastos = totalGastosEventos;
            }

            return View(model);
        }

        public UserDataModel getUserData()
        {
            FirebaseResponse response = firebase.Client.Get("UserData");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<UserDataModel>();
            var userEmail = User.Identity.Name.Split('|')[1];

            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<UserDataModel>(((JProperty)item).Value.ToString()));
                }
            }

            if (list.Any(x => x.UserEmail == userEmail))
            {
                return list.Where(x => x.UserEmail == User.Identity.Name.Split('|')[1]).FirstOrDefault();
            }
            else
            {
                return null;
            }

        }

        [HttpPost]
        public ActionResult AddEvent(EventoModel model)
        {
            UserDataModel userData = getUserData();
            if (userData.Eventos == null)
                userData.Eventos = new List<EventoModel>();

            model.title = model.title + " - $" + model.Monto;

            userData.Eventos.Add(model);

            SetResponse setResponse = firebase.Client.Set("UserData/" + userData.UserDataId, userData);
            Console.WriteLine(setResponse);

            return RedirectToAction("Index");
        }
    }
}