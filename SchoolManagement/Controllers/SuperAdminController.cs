using EventApplicationCore.Library;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SchoolManagement.Concrete;
using SchoolManagement.Filters;
using SchoolManagement.Helpers;
using SchoolManagement.Interface;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [ValidateSuperAdminSession]
    public class SuperAdminController : Controller
    {
        private ITbl_user _ITbl_user;
        private IRoles _IRoles;
        private ICacheManager _ICacheManager;
        private IUsers _IUsers;


        public SuperAdminController()
        {
            _ITbl_user = new Tbl_userConcrete();
            _IRoles = new RolesConcrete();
            _IUsers = new UsersConcrete();
            _ICacheManager = new CacheManager();
        }

        // GET: SuperAdmin
        public ActionResult Dashboard()
        {
            try
            {

                var teacherCount = _ICacheManager.Get<object>("TeacherCount");
                var studentCount = _ICacheManager.Get<object>("StudentCount");
                var supportingStaffCount = _ICacheManager.Get<object>("ProjectCount");

                if (teacherCount == null || studentCount == null || supportingStaffCount == null)
                {
                    var allcount = _IUsers.GetAllCount();
                    var TeacherCount = allcount.Where(f => f.RoleID == 1).Select(n => n.countCase).FirstOrDefault();
                    _ICacheManager.Add("TeacherCount", TeacherCount);
                    ViewBag.AdminCount = TeacherCount;

                    var StudentCount = allcount.Where(f => f.RoleID == 2).Select(n => n.countCase).FirstOrDefault();
                    _ICacheManager.Add("StudentCount", StudentCount);
                    ViewBag.UsersCount = StudentCount;

                    var SupportingStaffCount = allcount.Where(f => f.RoleID == 4).Select(n => n.countCase).FirstOrDefault();
                    _ICacheManager.Add("SupportingStaff", SupportingStaffCount);
                    ViewBag.ProjectCount = SupportingStaffCount;
                }
                else
                {
                    ViewBag.AdminCount = teacherCount;
                    ViewBag.UsersCount = studentCount;
                    ViewBag.ProjectCount = supportingStaffCount;
                }


                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {
            StaffViewModel tbl_ = new StaffViewModel();
            tbl_.ListofRoles = _IRoles.ListofRole();
            return View(tbl_);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin(StaffViewModel registration)
        {

            try
            {
                var isUsernameExists = _ITbl_user.CheckUserNameExists(registration.Username);

                if (isUsernameExists && registration.RegistrationID == 0)
                {
                    ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
                }
                else
                {
                    Tbl_user tbl_ = new Tbl_user();
                    tbl_ = Convert_tbl_user_StaffViewModel(tbl_, registration, false);
                    if (_ITbl_user.AddUser(tbl_) > 0)
                    {
                        TempData["MessageRegistration"] = "Data Saved Successfully!";
                        return RedirectToAction("CreateAdmin");
                    }
                    else
                    {
                        return View("CreateAdmin", registration);
                    }
                }

                return RedirectToAction("Dashboard");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Registration()
        {
            StudentViewModel tbl_ = new StudentViewModel();
            tbl_.ListofClass = _IUsers.GetDropDownList("Class");

            return View(tbl_);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(StudentViewModel registration)
        {

            try
            {
                registration.ListofClass = _IUsers.GetDropDownList("Class");
                Tbl_user tbl_ = new Tbl_user();
                tbl_ = Convert_tbl_user_StudentViewModel(tbl_, registration, false);
                int RegistrationID = _ITbl_user.AddStudent(tbl_);
                if (RegistrationID > 0)
                {
                    TempData["MessageRegistration"] = "Data Saved Successfully!";
                    return RedirectToAction("EditStudent", new { RegistrationID = RegistrationID });
                }

            }
            catch
            {
                throw;
            }
            registration.ListofClass = _IUsers.GetDropDownList("Class");
            TempData["MessageRegistration"] = "Fill the form correctly!";
            return View("Registration", registration);
        }

        public ActionResult DeleteUser(int ID)
        {
            // result = new { res = "An error occured", status=0 };
            var result = new { Result = "An error occured", Status = "0" };
            try
            {
                if (ID != 0)
                {
                    var userDetailsResponse = _IUsers.GetUserDetailsByRegistrationID(ID);
                    userDetailsResponse.IsDleted = true;
                    if (_ITbl_user.EditUser(userDetailsResponse) > 0)
                    {
                        result = new { Result = "user deleted successfully", Status = "1" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = new { Result = "Error occurred. Error details: " + ex.Message, Status = "0" };
                //throw;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult EditStaff(int RegistrationID)
        {
            try
            {
                if (RegistrationID != 0)
                {
                    var DocumentTypeList = _IUsers.GetDropDownList("UserDocument");
                    SelectList list = new SelectList(DocumentTypeList, "Value", "Text");
                    ViewData["DocumentType"] = list;
                    var userDetailsResponse = _IUsers.GetUserDetailsByRegistrationID(RegistrationID);
                    StaffViewModel staffViewModel = new StaffViewModel();
                    staffViewModel = Convert_StaffViewModel_tbl_user(userDetailsResponse, staffViewModel);

                    staffViewModel.ListofRoles = _IRoles.ListofRole();
                    return View("EditStaff", staffViewModel);
                }
                return RedirectToAction("Admin", "AllUsers");

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStaff(StaffViewModel registration)
        {
            try
            {
                if (registration.RegistrationID != 0)
                {
                    var DocumentTypeList = _IUsers.GetDropDownList("UserDocument");
                    SelectList list = new SelectList(DocumentTypeList, "Value", "Text");
                    ViewData["DocumentType"] = list;
                    var userDetailsResponse = _IUsers.GetUserDetailsByRegistrationID(registration.RegistrationID);
                    if (userDetailsResponse.RegistrationID != 0)
                    {
                        userDetailsResponse = Convert_tbl_user_StaffViewModel(userDetailsResponse, registration, true);
                        if (_ITbl_user.EditUser(userDetailsResponse) > 0)
                            TempData["MessageRegistration"] = "Data Updated Successfully!";
                        else
                            TempData["MessageRegistration"] = "Some error occured!";

                        return RedirectToAction("EditStaff", new { RegistrationID = userDetailsResponse.RegistrationID });
                    }
                }
                registration.ListofRoles = _IRoles.ListofRole();
                return View("EditStaff", registration);
            }
            catch
            {
                throw;
                //return View();
            }
        }

        [HttpGet]
        public ActionResult EditStudent(int RegistrationID)
        {
            try
            {
                if (RegistrationID != 0)
                {
                    var DocumentTypeList = _IUsers.GetDropDownList("UserDocument");
                    SelectList list = new SelectList(DocumentTypeList, "Value", "Text");
                    ViewData["DocumentType"] = list;
                    var userDetailsResponse = _IUsers.GetStudentDetailsByRegistrationID(RegistrationID);
                    userDetailsResponse.CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                    userDetailsResponse.ListofClass = _IUsers.GetDropDownList("Class");
                    return View("EditStudent", userDetailsResponse);
                }
                return RedirectToAction("Admin", "AllUsers");

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(StudentViewModel registration)
        {
            try
            {
                var DocumentTypeList = _IUsers.GetDropDownList("UserDocument");
                if (registration.RegistrationID != 0)
                {
                    SelectList list = new SelectList(DocumentTypeList, "Value", "Text");
                    ViewData["DocumentType"] = list;
                    var userDetailsResponse = _IUsers.GetUserDetailsByRegistrationID(registration.RegistrationID);
                    userDetailsResponse = Convert_tbl_user_StudentViewModel(userDetailsResponse, registration, true);
                    if (_ITbl_user.AddStudent(userDetailsResponse) > 0)
                        TempData["MessageRegistration"] = "Data Updated Successfully!";
                    else
                        TempData["MessageRegistration"] = "Some error occured!";

                    return RedirectToAction("EditStudent", new { RegistrationID = userDetailsResponse.RegistrationID });

                }
                registration.ListofClass = DocumentTypeList;
                return View("EditStaff", registration);
            }
            catch
            {
                throw;
                //return View();
            }
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            var result = new { Result = "File not uploaded!", Status = "0" };


            if (Request.Files.Count > 0 && Request["DoctypeName"] != string.Empty && Request["hdnRegistrationID"] != string.Empty)
            {
                try
                {
                    int Doctype = Convert.ToInt32(Request["DoctypeName"]);
                    int UserID = Convert.ToInt32(Request["hdnRegistrationID"]);
                    string UserName = Request["hdnUserName"];
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    if (UserID > 0 && UserName != string.Empty)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {

                            HttpPostedFileBase file = files[i];
                            string fname;

                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }
                            var supportedTypes = new[] { "jpg", "png", "pdf" };
                            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                            var filesize = 24;
                            if (!supportedTypes.Contains(fileExt.ToLower()))
                            {
                                result = new { Result = "Only Upload jpg/pdf File!", Status = "0" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            else if (file.ContentLength > (filesize * 1000000))
                            {
                                result = new { Result = "File size Should Be UpTo " + filesize + "MB", Status = "0" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            if (fname.Length > 50)
                            {
                                result = new { Result = "File Name must be less than 50 character!", Status = "0" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            if (UserID != 0 && file != null && file.ContentLength > 0)
                            {
                                var userDocuments = new Documents();
                                string ImgPath = Server.MapPath("~" + ConfigurationManager.AppSettings["UserIdentity"].ToString()) + UserName + "\\";
                                string docPath = ConfigurationManager.AppSettings["UserIdentity"].ToString() + UserName + "\\";
                                if (!Directory.Exists(ImgPath))
                                    Directory.CreateDirectory(ImgPath);
                                userDocuments.UserID = UserID;
                                userDocuments.DocumentName = fname;
                                userDocuments.DocumentPath = docPath + fname;
                                userDocuments.DocumentType = Doctype;
                                userDocuments.CreatedOn = DateTime.Now;
                                int DocumentID = _ITbl_user.CheckUserDocExists(Doctype, UserID);
                                if (DocumentID > 0)
                                {
                                    userDocuments.DocumentID = DocumentID;
                                    if (_ITbl_user.UpdateUserDocuments(userDocuments, false) == true)
                                        result = new { Result = "File Uploaded Successfully!", Status = "1" };
                                    else
                                        result = new { Result = "Some error occured!", Status = "0" };
                                }
                                else
                                {
                                    if (_ITbl_user.AddUserDocuments(userDocuments) > 0)
                                        result = new { Result = "File Uploaded Successfully!", Status = "1" };
                                    else
                                        result = new { Result = "Some error occured!", Status = "0" };
                                }
                                string imgPath = Path.Combine(ImgPath, fname);
                                file.SaveAs(imgPath);
                            }
                            else
                                result = new { Result = "Inalid File!", Status = "0" };
                        }
                    }
                    else
                        result = new { Result = "Inalid User!", Status = "0" };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = new { Result = "Error occurred. Error details: " + ex.Message, Status = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                result = new { Result = "No files selected.", Status = "0" };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteFiles(int DocumentID)
        {
            try
            {
                if (DocumentID != 0)
                {
                    var userDocuments = new Documents();
                    userDocuments.DocumentID = DocumentID;
                    if (_ITbl_user.UpdateUserDocuments(userDocuments, true) == true)
                        return Json("File Deleted Successfully!");
                }
                return Json("Document not valid!");
            }
            catch (Exception ex)
            {
                //throw;
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        [HttpGet]
        public ActionResult GetUserDocuments(int UserID)
        {
            try
            {
                var userDocs = _IUsers.GetUserDocument(UserID);
                var result = new { userDocs = userDocs };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Tbl_user Convert_tbl_user_StaffViewModel(Tbl_user tbl_User, StaffViewModel staffViewModel, bool isEdit)
        {
            if (isEdit == false)
            {
                tbl_User.Username = staffViewModel.Username;
                tbl_User.CreatedOn = DateTime.Now;
                tbl_User.Password = EncryptionLibrary.EncryptText(staffViewModel.Password);
                tbl_User.RoleID = staffViewModel.RoleID;
            }
            tbl_User.Gender = staffViewModel.Gender;
            tbl_User.Name = staffViewModel.Name;
            tbl_User.Mobileno = staffViewModel.Mobileno;
            tbl_User.EmailID = staffViewModel.EmailID;
            tbl_User.Birthdate = staffViewModel.Birthdate;
            tbl_User.DateofJoining = staffViewModel.DateofJoining;
            return tbl_User;
        }

        private StaffViewModel Convert_StaffViewModel_tbl_user(Tbl_user tbl_User, StaffViewModel staffViewModel)
        {
            staffViewModel.Username = tbl_User.Username;
            staffViewModel.Name = tbl_User.Name;
            staffViewModel.Mobileno = tbl_User.Mobileno;
            staffViewModel.EmailID = tbl_User.EmailID;
            staffViewModel.Birthdate = tbl_User.Birthdate;
            staffViewModel.DateofJoining = tbl_User.DateofJoining;
            //staffViewModel.CreatedOn = DateTime.Now;
            staffViewModel.RoleID = tbl_User.RoleID;
            staffViewModel.Gender = tbl_User.Gender;
            return staffViewModel;
        }

        private Tbl_user Convert_tbl_user_StudentViewModel(Tbl_user tbl_User, StudentViewModel studentViewModel, bool isEdit)
        {
            if (isEdit == false)
            {
                tbl_User.Username = studentViewModel.Username;
                tbl_User.CreatedOn = DateTime.Now;
                tbl_User.Password = EncryptionLibrary.EncryptText(ConstantVariable.DefaultPassword);
                tbl_User.RoleID = ConstantVariable.StudentRoleID;
            }
            tbl_User.Gender = studentViewModel.Gender;
            tbl_User.Name = studentViewModel.Name;
            tbl_User.Mobileno = studentViewModel.Mobileno;
            tbl_User.EmailID = studentViewModel.EmailID;
            tbl_User.Birthdate = studentViewModel.Birthdate;
            tbl_User.DateofJoining = studentViewModel.DateofJoining;
            tbl_User.FatherName = studentViewModel.FatherName;
            tbl_User.MotherName = studentViewModel.MotherName;
            tbl_User.Address = studentViewModel.Address;
            tbl_User.StudentClassID = studentViewModel.StudentClassID;
            return tbl_User;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStudentSession(StudentViewModel registration)
        {
            try
            {
                if (registration.RegistrationID != 0)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["SuperAdmin"])))
                        registration.UpdatedByUserID = Convert.ToInt32(Session["SuperAdmin"]);
                    var userDetailsResponse = _IUsers.UpdateStudentSession(registration);

                }
                return View("EditStaff", registration);
            }
            catch
            {
                throw;
                //return View();
            }
        }
    }
}