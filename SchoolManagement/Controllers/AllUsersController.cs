using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Concrete;
using SchoolManagement.Filters;
using SchoolManagement.Interface;

namespace SchoolManagement.Controllers
{
    [ValidateSuperAdminSession]

    public class AllUsersController : Controller
    {
        private IUsers _IUsers;
        public AllUsersController()
        {
            _IUsers = new UsersConcrete();
        }

        // GET: AllUsers
        public ActionResult Users()
        {
            return View();
        }

        public ActionResult LoadUsersData()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                int recordsTotal = 0;

                var rolesData = _IUsers.ShowallUsers(sortColumn, sortColumnDir, searchValue,CurrentSessionID);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult LoadAlumniData()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                int recordsTotal = 0;

                var rolesData = _IUsers.ShowallAlumni(sortColumn, sortColumnDir, searchValue, CurrentSessionID);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult UserDetails(int? RegistrationID)
        {
            try
            {
                if (RegistrationID == null)
                {
                    return RedirectToAction("Users");
                }
                var userDetailsResponse = _IUsers.GetStudentDetailsByRegistrationID(RegistrationID);
                return PartialView("_StudentDetails", userDetailsResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Teachers()
        {
            return View();
        }

        public ActionResult LoadTeacherssData()
        {

            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                var rolesData = _IUsers.ShowAllTeachers(sortColumn, sortColumnDir, searchValue);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult AdminDetails(int? RegistrationID)
        {
            try
            {
                if (RegistrationID == null)
                {

                }
                var userDetailsResponse = _IUsers.GetUserDetailsByRegistrationID(RegistrationID);
                return PartialView("_UserDetails", userDetailsResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult OtherStaff()
        {
            return View();
        }

        public ActionResult CheckAdmissionSessionInfo(string RegistrationID,string AdmDate, string Class)
        {
            try
            {
                //string UserAdmDate = Convert.ToString(Request["AdmDate"]);
                int UserID = Convert.ToInt32(RegistrationID);
                if (AdmDate != "undefined")
                {
                    var userDetailsResponse = _IUsers.GetStudentAvailableAcademicSession(UserID, AdmDate);

                    
                    if (userDetailsResponse.errorStatus == false && userDetailsResponse.ListofSubDropDown != null)
                    {
                        userDetailsResponse.StudentClassID = Convert.ToInt32(Class);
                        return PartialView("_SessionDetUpdate", userDetailsResponse);
                    }
                    else
                    {
                        var result1 = new { Message = userDetailsResponse.errorMessage, Status = Convert.ToString(userDetailsResponse.errorStatus) };
                        return Json(result1, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                var result = new { Message = "", Status = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult CheckAdmissionSessionforRegistration(string AdmDate)
        {
            try
            {
                
                if (AdmDate != "undefined")
                {
                    var userDetailsResponse = _IUsers.GetStudentAvailableAcademicSession(0, AdmDate);

                    var result1 = new { Message = userDetailsResponse.errorMessage, Status = Convert.ToString(userDetailsResponse.errorStatus) };
                    return Json(result1, JsonRequestBehavior.AllowGet);

                }
                var result = new { Message = "", Status = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}