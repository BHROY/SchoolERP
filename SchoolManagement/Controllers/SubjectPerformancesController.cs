using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Concrete;
using SchoolManagement.Filters;
using SchoolManagement.Interface;
using SchoolManagement.Models;
using System.Dynamic;

namespace SchoolManagement.Controllers
{
    [ValidateSuperAdminSession]
    public class SubjectPerformancesController : Controller
    {
        private ISubjectPerformance _ISubjectPerformance;
        private IUsers _IUsers;
        public SubjectPerformancesController()
        {
            _ISubjectPerformance = new SubjectPerformanceConcrete();
            _IUsers = new UsersConcrete();
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
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["AdminUser"])))
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

        public ActionResult ShowStudentSubjectMarksModal()//(int? RegistrationID)
        {
            try
            {
                var subjectPerformace = new SubjectPerformanceViewModel();
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                subjectPerformace = _ISubjectPerformance.GetSessionExamDetail(1, 3, 1, CurrentSessionID);
                return PartialView("_StudentSubjectMarks", subjectPerformace);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetStudentMarksDataForGraph(int RegistrationID)
        {
            try
            {
                if (RegistrationID != 0)
                {
                    int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                    List<Subject_X_AllExam> subject_X_AllExam = new List<Subject_X_AllExam>();
                    List<Exam_X_AllSubject> exam_X_AllSubject = new List<Exam_X_AllSubject>();
                    var ds = _ISubjectPerformance.GetStudentMarksDataForGraph(RegistrationID, CurrentSessionID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var table = ds.Tables[0];
                        var Posts = (from DataRow row in table.Rows  //0 means 1st select
                                     select new Subject_X_AllExam  //Posts model to map
                                     {
                                         SubjectName = row["SubjectName"].ToString(), 
                                         Percentage = Convert.ToDecimal(row["Percentage"]),
                                     }).ToList();
                        subject_X_AllExam = Posts;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        var table = ds.Tables[1];
                        var Posts = (from DataRow row in table.Rows  //0 means 1st select
                                     select new Exam_X_AllSubject  //Posts model to map
                                     {
                                         ExamType = row["examType"].ToString(),
                                         Percentage = Convert.ToDecimal(row["Percentage"]),
                                     }).ToList();
                        exam_X_AllSubject = Posts;
                    }
                    var result = new { DataArray = subject_X_AllExam, DataArrayExamWise = exam_X_AllSubject, responseText = "Success" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var result1 = new { responseText = "Inalid!" };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        public ActionResult studentMarksProfile(int RegistrationID)
        {
            var resultd = new List<dynamic>();
            if (RegistrationID != 0)
            {
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                var ds = _ISubjectPerformance.GetStudentMarksDataForGraph(RegistrationID, CurrentSessionID);
                if (ds.Tables[2].Rows.Count > 0)
                {
                    var table = ds.Tables[2];
                    //var colN = table.Columns.Count;
                    var columns = table.Columns.Cast<DataColumn>();

                    var dictionaryList = table.AsEnumerable()
                        .Select(dataRow => columns
                            .Select(column =>
                                new { Column = column.ColumnName, Value = dataRow[column] })
                                     .ToDictionary(data => data.Column, data => data.Value)).ToList();

                    foreach (var emprow in dictionaryList)
                    {
                        var row = (IDictionary<string, object>)new ExpandoObject();
                        Dictionary<string, object> eachRow = (Dictionary<string, object>)emprow;

                        foreach (KeyValuePair<string, object> keyValuePair in eachRow)
                        {
                            if (keyValuePair.Value is System.DBNull)
                            {
                                row.Add(keyValuePair.Key, "Not Declared");
                            }
                            else
                                row.Add(keyValuePair);
                        }
                        resultd.Add(row);
                    }
                }

            }
            StudentViewModel studentViewModel = _IUsers.GetStudentDetailsByRegistrationID(RegistrationID);
            studentViewModel.ListAllSubjectMarks = resultd;
            return View(studentViewModel);
        }
        public ActionResult AssignRollNumbers()
        {
            SubjectPerformanceViewModel subjectPerformance = _ISubjectPerformance.GetMenuList();
            return View(subjectPerformance);
        }

        public ActionResult GetStudentRollNumber(string studentClass)
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
                var rolesData = _ISubjectPerformance.ShowStudentsClassWiseRollnumber(sortColumn, sortColumnDir, searchValue, studentClass, CurrentSessionID);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult EditRollNumber(int RegistrationID, int ClassID, int RollNumber,  int ID)
        {
            try
            {
                var result = new { Status = "false", Result = "An Error Occured" };
                if (RegistrationID != 0 && RollNumber != 0 && ID != 0)
                {
                    var studentSessionInfo = new StudentSessionInfo();
                    studentSessionInfo.ID = ID;
                    studentSessionInfo.RegistrationID = RegistrationID;
                    studentSessionInfo.RollNo = RollNumber;
                    studentSessionInfo.ClassID = ClassID;
                    studentSessionInfo.SessionID = Convert.ToInt32(Session["CurrentSessionID"]);

                    int ResultID = _ISubjectPerformance.InsertUpdateStudentRollNumber(studentSessionInfo);
                    result = new { Status = "true", Result = "Data saved successfully" };
                    if (ResultID == 0)
                        return Json(result, JsonRequestBehavior.AllowGet);
                    else if (ResultID == 1)
                        result = new { Status = "false", Result = "Roll Number for this Class already Exists!" };
                    else
                        result = new { Status = "true", Result = "Roll Number added successfully." };
                }
                else
                    result = new { Status = "false", Result = "Refresh the page!" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Status = "true", Result = "Error occurred. Error details: " + ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
                //return Json("Error occurred. Error details: " + ex.Message);
            }
        }
        public ActionResult SetDefaultAlphabeticallyRollNumber(int ClassID)
        {
            try
            {
                if (ClassID != 0)
                {
                    if (_ISubjectPerformance.SetDefaultAlphabeticallyRollNumber(ClassID, Convert.ToInt32(Session["CurrentSessionID"])))
                    {
                        var result = new { responseText = "Roll Numbers Set to Names Alphabetical Wise" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                var result1 = new { responseText = "Inalid!" };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }
        public ActionResult ExamAnalysisDashboard()
        {
            SubjectPerformanceViewModel subjectPerformance = _ISubjectPerformance.GetMenuList();
            return View(subjectPerformance);
        }
        public ActionResult AssignSubjectToClass()
        {
            SubjectPerformanceViewModel subjectPerformance = _ISubjectPerformance.GetMenuList();
            return View(subjectPerformance);
        }
        public ActionResult ShowClassWiseAddedSubjects(string ClassID)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                int recordsTotal = 0;
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                var rolesData = _ISubjectPerformance.ShowClassWiseAddedSubjects(ClassID, CurrentSessionID);
                recordsTotal = rolesData.Count();
                var data = rolesData.ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult CheckIfExamHeldForClass(string ClassID)
        {
            try
            {
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                var Data = _ISubjectPerformance.CheckIfExamHeldForClass(ClassID, CurrentSessionID);

                return Json(new { checkIfExamHeldForClass = Data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult InsertUpdateClassToSubject(int ClassID, int[] SubjectIDList)
        {
            try
            {
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                int createdBy = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(Session["AdminUser"])))
                    createdBy = Convert.ToInt32(Session["AdminUser"]);
                else if (!string.IsNullOrEmpty(Convert.ToString(Session["SuperAdmin"])))
                    createdBy = Convert.ToInt32(Session["SuperAdmin"]);
                List<ClassToSubject> classToSubjectsList = new List<ClassToSubject>();
                //int ClassID = Convert.ToInt32(ClassID);
                for (var i = 0; i < SubjectIDList.Length; i++)
                {
                    ClassToSubject classToSubject = new ClassToSubject();
                    classToSubject.ClassID = ClassID;
                    classToSubject.CreatedBy = createdBy;
                    classToSubject.SessionID = CurrentSessionID;
                    classToSubject.CreatedOn = DateTime.Now;
                    classToSubject.SubjectID = SubjectIDList[i];
                    classToSubject.IsDeleted = false;
                    classToSubjectsList.Add(classToSubject);
                }
                var Data = _ISubjectPerformance.InsertUpdateClassToSubject(classToSubjectsList);

                return Json(new { checkIfExamHeldForClass = Data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetClassBasedSubjects(int ClassID)
        {
            try
            {
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                var Data = _ISubjectPerformance.GetClassBasedSubjects(ClassID, CurrentSessionID);
                return Json(new { SubjectList = Data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetMarksAnalysisForAdminForGraph(int ExamTypeID, int ClassID, int SubjectID)
        {
            try
            {
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                List<ClassOverAllGrade> classOverAllGrade = new List<ClassOverAllGrade>();
                List<ClassWinners> classWinners = new List<ClassWinners>();
                List<ClassClassWiseData> classClassWiseData = new List<ClassClassWiseData>();
                List<Subject_X_AllExam> subject_X_AllExam = new List<Subject_X_AllExam>();
                List<ClassUndeclaredSubjectData> classUndeclaredSubjectData = new List<ClassUndeclaredSubjectData>();
                var ds = _ISubjectPerformance.GetMarksAnalysisForAdminForGraph(ExamTypeID, ClassID, SubjectID, CurrentSessionID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var table = ds.Tables[0];
                    var Posts = (from DataRow row in table.Rows  //0 means 1st select
                                 select new ClassOverAllGrade  //Posts model to map
                                 {
                                     Grade = row["Grade"].ToString(),
                                     NumberOfStudent = Convert.ToInt32(row["NumberOfStudent"]),
                                     Tooltip = row["Tooltip"].ToString(),
                                 }).ToList();
                    classOverAllGrade = Posts;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    var table = ds.Tables[1];
                    var Posts = (from DataRow row in table.Rows  //0 means 1st select
                                 select new ClassClassWiseData  //Posts model to map
                                 {
                                     ClassName = row["ClassName"].ToString(),
                                     Percentage = Convert.ToDecimal(row["Percentage"]),
                                 }).ToList();
                    classClassWiseData = Posts;
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    var table = ds.Tables[2];
                    var Posts = (from DataRow row in table.Rows  //0 means 1st select
                                 select new Subject_X_AllExam  //Posts model to map
                                 {
                                     SubjectName = row["SubjectName"].ToString(),
                                     Percentage = Convert.ToDecimal(row["Percentage"]),
                                 }).ToList();
                    subject_X_AllExam = Posts;
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    var table = ds.Tables[3];
                    var Posts = (from DataRow row in table.Rows  //0 means 1st select
                                 select new ClassUndeclaredSubjectData  //Posts model to map
                                 {
                                     SubjectName = row["SubjectName"].ToString(),
                                     ClassName = row["ClassName"].ToString(),
                                 }).ToList();
                    classUndeclaredSubjectData = Posts;
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    var table = ds.Tables[4];
                    var Posts = (from DataRow row in table.Rows  //0 means 1st select
                                 select new ClassWinners  //Posts model to map
                                 {
                                     ClassName = row["ClassName"].ToString(),
                                     StudentName = row["StudentName"].ToString(),
                                     RollNo = Convert.ToInt32(row["RollNo"]),
                                     StudentRank = Convert.ToInt32(row["StudentRank"]),
                                 }).ToList();
                    classWinners = Posts;
                }
                var result = new { DataArrayOverAllGrade = classOverAllGrade, DataArrayWinners = classWinners, DataArrayClassWise= classClassWiseData, DataArraySubjectWise= subject_X_AllExam, DataArrayUndeclaredSubjects= classUndeclaredSubjectData, responseText = "Success" };
                return Json(result, JsonRequestBehavior.AllowGet);
                var result1 = new { responseText = "Inalid!" };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }
        public ActionResult GetGradeWiseStudentList(int ExamTypeID, int ClassID, int SubjectID, string Grade)
        {
            try
            {
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                List<ClassOverAllGrade> classOverAllGrade = new List<ClassOverAllGrade>();
                var GradeWiseStudentList = _ISubjectPerformance.GetGradeWiseStudentList(ExamTypeID, ClassID, SubjectID, Grade, CurrentSessionID);
                
                var result = new { DataArrayGradeWiseStudentList = GradeWiseStudentList, responseText = "Success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }
        public ActionResult checkRpt()
        {
            SubjectPerformanceViewModel subjectPerformance = _ISubjectPerformance.GetMenuList();
            return View(subjectPerformance);
        }
        public void GetStudentReport(int RegistrationID)
        {
            ReportParams reportParams = new ReportParams();
            var data = GetStudentInfo(RegistrationID);
            reportParams.DataSource = data.Tables[2];
            reportParams.ReportTitle = "Student Marks Report";
            reportParams.RptFileName = "StudentInfoReport";
            reportParams.ReportType = "StudentsMarksReport";
            reportParams.DataSetName = "dsStudentReport";
            this.HttpContext.Session["ReportParam"] = reportParams;
        }

        public DataSet GetStudentInfo(int RegistrationID)
        {
            try
            {
                int CurrentSessionID = Convert.ToInt32(Session["CurrentSessionID"]);
                var ds = _ISubjectPerformance.GetStudentMarksDataForGraph(RegistrationID, CurrentSessionID);
                return ds;
            }
            catch (Exception ex)
            {
                throw;
                //return Json("Error occurred. Error details: " + ex.Message);
            }
        }
        private class Subject_X_AllExam
        {
            public string SubjectName { get; set; }
            public decimal Percentage;
        }
        private class Exam_X_AllSubject
        {
            public string ExamType { get; set; }
            public decimal Percentage;
        }
        private class ClassClassWiseData
        {
            public string ClassName { get; set; }
            public decimal Percentage;
        }
        private class ClassOverAllGrade
        {
            public string Grade { get; set; }
            public int NumberOfStudent;
            public string Tooltip;
        }
        private class ClassUndeclaredSubjectData
        {
            public string ClassName { get; set; }
            public string SubjectName;
        }
        private class ClassWinners
        {
            public string ClassName { get; set; }
            public string StudentName;
            public int RollNo;
            public int StudentRank;
        }
    }
}
