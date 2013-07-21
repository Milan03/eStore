using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eStoreWebsite.Filters;
using WebMatrix.WebData;
using System.Web.Security;
using eStoreViewModels;

namespace eStoreWebsite.Controllers
{
    public class HomeController : Controller
    {
        [InitializeSimpleMembership]
        public ActionResult Index()
        {
            if (Session["LoginStatus"] == null)
            {
                Session["LoginStatus"] = "Not Loggin In";
            }
            return View();
        }

        public ActionResult Register(eStoreViewModels.CustomerViewModel cust)
        {
            try
            {
                ViewBag.Message = "";
                WebSecurity.CreateUserAndAccount(cust.Username, cust.Password);
                cust.Register();
                if (cust.CustomerID > 0)
                {
                    ViewBag.Message = cust.Message;
                }
                else
                {
                    ViewBag.Message = "Problem Registering, try again later";
                    ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(cust.Username);
                    ((SimpleMembershipProvider)Membership.Provider).DeleteUser(cust.Username, true);
                }
                ViewBag.Message = cust.Message;
                return PartialView("PopupMessage");
            }
            catch (MembershipCreateUserException e)
            {
                ViewBag.Message = ErrorCodeToString(e.StatusCode);
                return PartialView("PopupMessage");
            }
        }

        public ActionResult Login(eStoreViewModels.CustomerViewModel cust)
        {
            try
            {
                if (WebSecurity.Login(cust.Username, cust.Password))
                {
                    cust.GetCurrentProfile();
                    Session["CustomerID"] = cust.CustomerID;
                    Session["Message"] = "Welcome " + cust.Username;
                    Session["LoginStatus"] = "logged in as " + cust.Username;
                    return Json(new { url = Url.Action("") }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "login failed - try again";
                    return PartialView("PopupMessage");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Server login error";
                String exMsg = ex.Message;
                return PartialView("PopupMessage");
            }
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}
