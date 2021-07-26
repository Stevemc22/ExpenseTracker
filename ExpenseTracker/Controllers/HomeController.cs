using ExpenseTracker.Data;
using ExpenseTracker.Models;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        FirebaseDB firebase = new FirebaseDB();
        public ActionResult Index()
        {
            ParentModelView data = new ParentModelView();
            data.UserDataModel = getUserData();
            CalculateAmounts(data.UserDataModel);
            ViewBag.Username = User.Identity.Name.Split('|')[0];
            return View(data);
        }

        public UserDataModel getUserData()
        {
            FirebaseResponse response = firebase.Client.Get("UserData");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<UserDataModel>();
            var userEmail = User.Identity.Name.Split('|')[1];

            if (data != null) { 
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<UserDataModel>(((JProperty)item).Value.ToString()));
                }
            }

            if (list.Any(x => x.UserEmail == userEmail))
            {
                return list.Where(x => x.UserEmail == User.Identity.Name.Split('|')[1]).FirstOrDefault();
            } else
            {
                return null;
            }
            
        }

        public void CalculateAmounts(UserDataModel model)
        {
            var numberFormatInfo = new System.Globalization.NumberFormatInfo { NumberDecimalSeparator = "," };
            var totalIngreso = model.Ingresos.Sum(x => Decimal.Parse(x.Monto, numberFormatInfo));
            var totalGatos = model.Gastos.Sum(x => Decimal.Parse(x.Monto, numberFormatInfo));
            model.Disponible = (totalIngreso - totalGatos).ToString();
            model.GastoMes = (Decimal.Parse(model.Disponible) - Decimal.Parse(model.Objetivo)).ToString();
            model.GastoSemana = (Decimal.Parse(model.GastoMes) / 4).ToString();
        }

        [HttpPost]
        public ActionResult AddUpdateUserData(ParentModelView model)
        {
            try
            {
                UserDataModel data = getUserData();
                if(data != null)
                {
                    data.Objetivo = model.UserDataModel.Objetivo;
                    updateUserData(data);
                } else { 
                    model.UserDataModel.UserEmail = User.Identity.Name.Split('|')[1];
                    addUserData(model.UserDataModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Index");
        }

        private void addUserData(UserDataModel model)
        {
            PushResponse respone = firebase.Client.Push("UserData/", model);
            model.UserDataId = respone.Result.name;
            SetResponse setResponse = firebase.Client.Set("UserData/" + model.UserDataId, model);
            Console.WriteLine(setResponse);
        }

        private void updateUserData(UserDataModel model)
        {
            SetResponse setResponse = firebase.Client.Set("UserData/" + model.UserDataId, model);
            Console.WriteLine(setResponse);
        }

        [HttpPost]
        public ActionResult AddIncomes(ParentModelView model)
        {
            UserDataModel userData = getUserData();
            if (userData.Ingresos == null)
                userData.Ingresos = new List<IngresosModel>();

            userData.Ingresos.Add(model.IngresosModel);
            SetResponse setResponse = firebase.Client.Set("UserData/" + userData.UserDataId, userData);
            Console.WriteLine(setResponse);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddExpense(ParentModelView model)
        {
            UserDataModel userData = getUserData();
            if (userData.Gastos == null)
                userData.Gastos = new List<GastosModel>();

            userData.Gastos.Add(model.GastosModel);
            SetResponse setResponse = firebase.Client.Set("UserData/" + userData.UserDataId, userData);
            Console.WriteLine(setResponse);

            return RedirectToAction("Index");
        }
    }
}