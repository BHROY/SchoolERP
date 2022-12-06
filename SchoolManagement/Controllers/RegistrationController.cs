using EventApplicationCore.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Concrete;
using SchoolManagement.Filters;
using SchoolManagement.Interface;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [ValidateSuperAdminSession]
    public class RegistrationController : Controller
    {
        private ITbl_user _IRegistration;
        private IRoles _IRoles;
        public RegistrationController()
        {
            _IRegistration = new Tbl_userConcrete();
            _IRoles = new RolesConcrete();
        }

        // GET: Registration/Create
        public ActionResult Registration()
        {
            return View(new StudentViewModel());
        }

        // POST: Registration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(StudentViewModel registration1)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Tbl_user registration = new Tbl_user();
                    var isUsernameExists = _IRegistration.CheckUserNameExists(registration.Username);

                    if (isUsernameExists)
                    {
                        ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
                    }
                    else
                    {

                        registration.CreatedOn = DateTime.Now;
                        registration.RoleID = _IRoles.getRolesofUserbyRolename("Users");
                        registration.Password = EncryptionLibrary.EncryptText(registration.Password);
                        //registration.ConfirmPassword = EncryptionLibrary.EncryptText(registration.ConfirmPassword);
                        if (_IRegistration.AddUser(registration) > 0)
                        {
                            TempData["MessageRegistration"] = "Data Saved Successfully!";
                            return RedirectToAction("Registration");
                        }
                        else
                        {
                            return View(registration);
                        }
                    }
                    return RedirectToAction("Registration");
                }
                catch
                {
                    
                }
            }
            return View(registration1);
        }

        public JsonResult CheckUserNameExists(string Username)
        {
            try
            {
                var isUsernameExists = false;

                if (Username != null)
                {
                    isUsernameExists = _IRegistration.CheckUserNameExists(Username);
                }

                if (isUsernameExists)
                {
                    return Json(data: true);
                }
                else
                {
                    return Json(data: false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
