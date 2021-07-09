using ExpenseTracker.Models;
using Firebase.Auth;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        private static string ApiKey = "";

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            try
            {
                if (this.Request.IsAuthenticated)
                {
                    return this.RedirectToRoute(ReturnUrl);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return this.View(new LoginModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model, string ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var response = await auth.SignInWithEmailAndPasswordAsync(model.Email, model.Password);
                    string token = response.FirebaseToken;
                    var user = response.User;

                    if (token != "")
                    {
                        this.SignInUser(user.Email, user.DisplayName, token, false);
                        return this.RedirectToLocal(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return this.View(model);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(SignUpModel model)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

                var response = await auth.CreateUserWithEmailAndPasswordAsync(model.Email, model.Password, model.Name, true);
                ModelState.AddModelError(string.Empty, "Please verify your email then login ^^");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        private void SignInUser(string email, string displayName, string token, bool isPersistent)
        {
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimTypes.Name, displayName + "|" + email));
                claims.Add(new Claim(ClaimTypes.Email, email));
                claims.Add(new Claim(ClaimTypes.Authentication, token));
                var claimIdentities = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                authenticationManager.SignIn(
                    new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = isPersistent }, claimIdentities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClaimIdentities(string username, bool isPersistent)
        {
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdentities = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ActionResult RedirectToLocal(string ReturnUrl)
        {
            try
            {
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return this.Redirect(ReturnUrl);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return this.RedirectToAction("LogOff", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
    }
}