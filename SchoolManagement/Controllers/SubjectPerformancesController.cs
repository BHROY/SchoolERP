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
    public class SubjectPerformancesController : Controller
    {
        //private DatabaseContext db = new DatabaseContext();
        private ISubjectPerformance _ISubjectPerformance;

        public SubjectPerformancesController()
        {
            _ISubjectPerformance = new SubjectPerformanceConcrete();
        }

        // GET: SubjectPerformances
        public ActionResult Index()
        {
            SubjectPerformanceViewModel subjectPerformance = _ISubjectPerformance.GetMenuList();
            return View(subjectPerformance);
        }


        // GET: SubjectPerformances/Create
        public ActionResult Create()
        {
            SubjectPerformanceViewModel subjectPerformance = _ISubjectPerformance.GetMenuList();
            return View(subjectPerformance);
        }

        public ActionResult GetStudentMarksData( string ExamType, string studentClass, string subject )
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
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                var rolesData = _ISubjectPerformance.ShowStudentsClassWiseSubjectWise(sortColumn, sortColumnDir, searchValue, ExamType, studentClass, subject, CurrentSessionID);
                recordsTotal = rolesData.ListofStudent.Count();
                var data = rolesData.ListofStudent.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult EditMarks(int RegistrationID, string Name, int Marks, int ExamSessionID, int ID)
        {
            try
            {
                if (RegistrationID != 0 && ExamSessionID != 0)
                {
                    var studentExamPerformance = new StudentExamPerformance();
                    studentExamPerformance.ID = ID;
                    studentExamPerformance.RegistrationID = RegistrationID;
                    studentExamPerformance.SessionExamID = ExamSessionID;
                    studentExamPerformance.StudentName = Name;
                    studentExamPerformance.MarksAcquired = Marks;
                    studentExamPerformance.CreatedOn = DateTime.Now;
                    if(!string.IsNullOrEmpty(Convert.ToString(Session["AdminUser"])))
                        studentExamPerformance.CreatedBy = Convert.ToInt32(Session["AdminUser"]);
                    else if (!string.IsNullOrEmpty(Convert.ToString(Session["SuperAdmin"])))
                        studentExamPerformance.CreatedBy = Convert.ToInt32(Session["SuperAdmin"]);
                    studentExamPerformance.UpdatedOn = studentExamPerformance.CreatedOn;
                    studentExamPerformance.UpdatedBy = studentExamPerformance.CreatedBy;
                    int ResultID = _ISubjectPerformance.InsertUpdateStudentMarks(studentExamPerformance);
                    if (ResultID > 0)
                    {
                        var result = new { Status = "true", Result = "Data saved successfully", StudentID = ResultID };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                var result1 = new { Status = "true", Result = "Ivalid! Please add Total Marks of Exam" };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Status = "true", Result = "Error occurred. Error details: " + ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
                //return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        public ActionResult GetSessionExamDetail(int SubjectID, int ClassID, int ExamTypeID)
        {
            try
            {
                if (SubjectID != 0 && ExamTypeID != 0 && ClassID != 0)
                {
                    var subjectPerformace = new SubjectPerformanceViewModel();
                    int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                    subjectPerformace = _ISubjectPerformance.GetSessionExamDetail(SubjectID, ClassID, ExamTypeID, CurrentSessionID);
                    if (subjectPerformace.SessionExamID == 0)
                    {
                        //userDetailsResponse.StudentClassID = Convert.ToInt32(Class);
                        return PartialView("_SessionExamMarks", subjectPerformace);
                    }
                    else
                    {
                        var result = new { SessionExamID = Convert.ToString(subjectPerformace.SessionExamID), Status = "true", TotalMarks = Convert.ToString(subjectPerformace.TotalMarks) };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                var result1 = new {  Status = "false", Message = "Select Exam Type" };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult UpdateSessionExam(SubjectPerformanceViewModel subjectPerformance)
        {
            try
            {
                var result = new { Status = "false", Message = "Total marks must be greater than 5" };
                if (subjectPerformance.TotalMarks> 5)
                {
                    if (_ISubjectPerformance.InsertUpdateSessionExam(subjectPerformance) == true)
                    {
                        result = new { Status = "true", Message = "Success" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    result = new { Status = "false", Message = "An Error occured While saving" };
                }
                
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result1 = new { Status = "false", Message = "Error occurred. Error details: " + ex.Message };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowAllStudentPerformance(string ExamType, string studentClass, string subject)
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

                var rolesData = _ISubjectPerformance.ShowAllStudentPerformance(ExamType, studentClass, subject,sortColumn, sortColumnDir, searchValue, CurrentSessionID);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult SetDefaultMarksZeroBySesseionExamID(int ExamSessionID)
        {
            try
            {
                if (ExamSessionID != 0)
                {
                    int createdBy = 0;
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["AdminUser"])))
                        createdBy = Convert.ToInt32(Session["AdminUser"]);
                    else if (!string.IsNullOrEmpty(Convert.ToString(Session["SuperAdmin"])))
                        createdBy = Convert.ToInt32(Session["SuperAdmin"]);
                    if (_ISubjectPerformance.SetDefaultMarksZeroBySesseionExamID(ExamSessionID, createdBy))
                    {
                        var result = new { responseText = "Default Marks set to 0" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                var result1 = new {  responseText = "Inalid!" };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }


    }
}
