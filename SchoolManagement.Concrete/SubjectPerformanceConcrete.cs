//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SchoolManagement.Models;
//using System.Linq.Dynamic;
//using SchoolManagement.Interface;
//using System.Data.SqlClient;
//using System.Configuration;
//using Dapper;

using Dapper;
using SchoolManagement.Interface;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;

namespace SchoolManagement.Concrete
{
    public class SubjectPerformanceConcrete : ISubjectPerformance
    {
        public SubjectPerformanceViewModel GetMenuList()
        {
            try
            {
                SubjectPerformanceViewModel subjectPerformance = new SubjectPerformanceViewModel();
                using (var _context = new DatabaseContext())
                {
                    var DropdownSet = _context.DropDownSet;
                    subjectPerformance.ListofExamType = DropdownSet.Where(i => i.Category == "ExamType").ToList();
                    subjectPerformance.ListofClass = DropdownSet.Where(i => i.Category == "Class").ToList();
                    subjectPerformance.ListofSubjects = DropdownSet.Where(i => i.Category == "Subject").ToList();
                }
                return subjectPerformance;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SubjectPerformanceViewModel ShowStudentsClassWiseSubjectWise(string sortColumn, string sortColumnDir, string Search,string examTypeID, string studentClassID, string subjectID, int currentSession)
        {
            var _context = new DatabaseContext();
            SubjectPerformanceViewModel subjectPerformanceViewModel = new SubjectPerformanceViewModel();
            int ExamTypeID = Convert.ToInt32(examTypeID);
            int StudentClassID = Convert.ToInt32(studentClassID);
            int SubjectID = Convert.ToInt32(subjectID);
            subjectPerformanceViewModel.ClassID = StudentClassID;
            subjectPerformanceViewModel.ExamTypeID = ExamTypeID;
            subjectPerformanceViewModel.SubjectID = SubjectID;
            var SessionExam = _context.SessionExam.Where(i => i.ExamTypeID == ExamTypeID && i.SessionID == currentSession && i.SubjectID == SubjectID && i.ClassID == StudentClassID).FirstOrDefault();
            if (SessionExam == null)
            {
                subjectPerformanceViewModel.TotalMarks = 0;
            }
            else
            {
                subjectPerformanceViewModel.TotalMarks = SessionExam.TotalMarks;
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                var param = new DynamicParameters();
                param.Add("@examTypeID", ExamTypeID);
                param.Add("@StudentClassID", StudentClassID);
                param.Add("@SubjectID", SubjectID);
                param.Add("@currentSession", currentSession);
                param.Add("@Search", Search);
                var result = con.Query<StudentPerformanceViewModel>("Usp_GetExamPerformance", param, null, true, 0, System.Data.CommandType.StoredProcedure);
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    if (sortColumn == "Name")
                        sortColumn = "StudentName";
                    result = result.OrderBy(sortColumn + " " + sortColumnDir);
                }
                subjectPerformanceViewModel.ListofStudent = result.ToList();

            }
            return subjectPerformanceViewModel;
        }

        public int InsertUpdateStudentMarks(StudentExamPerformance studentExamPerformance)
        {
            var _context = new DatabaseContext();
            if (studentExamPerformance.ID == 0)
            {
                _context.StudentExamPerformance.Add(studentExamPerformance);
                if (_context.SaveChanges() > 0)
                { return studentExamPerformance.ID; }
            }
            else
            {
                StudentExamPerformance studentPerformance = new StudentExamPerformance();
                studentPerformance = _context.StudentExamPerformance.Where(i => i.ID == studentExamPerformance.ID).FirstOrDefault();
                studentPerformance.UpdatedBy = studentExamPerformance.UpdatedBy;
                studentPerformance.UpdatedOn = studentExamPerformance.UpdatedOn;
                studentPerformance.MarksAcquired = studentExamPerformance.MarksAcquired;
                _context.Entry(studentPerformance).State = System.Data.Entity.EntityState.Modified;
                if (_context.SaveChanges() > 0)
                { return studentExamPerformance.ID; }
            }
            return 0;
        }

        public SubjectPerformanceViewModel GetSessionExamDetail(int subjectID, int classID, int examTypeID, int CurrentSessionID)
        {
            var _context = new DatabaseContext();
            SubjectPerformanceViewModel subjectPerformanceViewModel = new SubjectPerformanceViewModel();
            //int ExamTypeID = Convert.ToInt32(examTypeID);
            //int StudentClassID = Convert.ToInt32(studentClassID);
            //int SubjectID = Convert.ToInt32(subjectID);
            var sessionExam = _context.SessionExam.Where(i => i.ExamTypeID == examTypeID && i.SessionID == CurrentSessionID && i.SubjectID == subjectID && i.ClassID == classID).FirstOrDefault();
            if (sessionExam != null)
            {
                subjectPerformanceViewModel.TotalMarks = sessionExam.TotalMarks;
                subjectPerformanceViewModel.SessionExamID = sessionExam.ID;
            }
            else
            {
                var drpdwn = _context.DropDownSet;
                subjectPerformanceViewModel.ExamTypeID = examTypeID;
                subjectPerformanceViewModel.SessionID = CurrentSessionID;
                subjectPerformanceViewModel.ClassID = classID;
                subjectPerformanceViewModel.SubjectID = subjectID;
                subjectPerformanceViewModel.ExamTypeName = drpdwn.Where(i => i.Category == "ExamType" && i.Value == examTypeID).FirstOrDefault().Text;
                subjectPerformanceViewModel.ClassName = drpdwn.Where(i => i.Category == "class" && i.Value == classID).FirstOrDefault().Text;
                subjectPerformanceViewModel.SubjectName = drpdwn.Where(i => i.Category == "Subject" && i.Value == subjectID).FirstOrDefault().Text;
                subjectPerformanceViewModel.SessionName = drpdwn.Where(i => i.Category == "Session" && i.Value == CurrentSessionID).FirstOrDefault().Text;

            }
            return subjectPerformanceViewModel;
        }

        public bool InsertUpdateSessionExam(SubjectPerformanceViewModel subjectPerformance)
        {
            var _context = new DatabaseContext();
            if (subjectPerformance.ID == 0)
            {
                SessionExam sessionExam = new SessionExam();
                sessionExam.ClassID = subjectPerformance.ClassID;
                sessionExam.CreatedBy = subjectPerformance.CreatedBy;
                sessionExam.CreatedOn = DateTime.Now;
                sessionExam.ExamTypeID = subjectPerformance.ExamTypeID;
                sessionExam.SessionID = subjectPerformance.SessionID;
                sessionExam.SubjectID = subjectPerformance.SubjectID;
                sessionExam.TotalMarks = subjectPerformance.TotalMarks;
                sessionExam.SubjectName = subjectPerformance.SubjectName;
                sessionExam.ClassName = subjectPerformance.ClassName;
                sessionExam.SessionName = subjectPerformance.SessionName;
                _context.SessionExam.Add(sessionExam);
                if (_context.SaveChanges() > 0)
                { return true; }
            }
            else
            {
                SessionExam sessionExam = new SessionExam();
                sessionExam.TotalMarks = subjectPerformance.TotalMarks;
                sessionExam.UpdatedOn = DateTime.Now;
                sessionExam.UpdatedBy = subjectPerformance.CreatedBy;
                _context.Entry(sessionExam).State = System.Data.Entity.EntityState.Modified;
                if (_context.SaveChanges() > 0)
                { return true; }
            }
            return false;
        }

        public List<StudentPerformanceViewModel> ShowAllStudentPerformance(string ExamType, string studentClass, string subject, string sortColumn, string sortColumnDir, string Search, int currentSessionID)
        {
            List<StudentPerformanceViewModel> StudentPerformanceViewModelList = new List<StudentPerformanceViewModel>();
            var _context = new DatabaseContext();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                var param = new DynamicParameters();
                param.Add("@SessionID", currentSessionID);
                param.Add("@search", Search);
                param.Add("@ExamType", ExamType);
                param.Add("@studentClass", studentClass);
                param.Add("@subject", subject);
                var result = con.Query<StudentPerformanceViewModel>("Usp_GetCurrentStudentExamPerformance", param, null, true, 0, System.Data.CommandType.StoredProcedure);
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    result = result.OrderBy(sortColumn + " " + sortColumnDir);
                }
                StudentPerformanceViewModelList = result.ToList();
            }

            return StudentPerformanceViewModelList;

        }

        public bool SetDefaultMarksZeroBySesseionExamID(int ExamSessionID,int createdBy)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                con.Open();
                SqlTransaction sql = con.BeginTransaction();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@SessionExamID", ExamSessionID);
                    param.Add("@createdBy", createdBy);
                    var result = con.Execute("Usp_SetDefaultMarksZero", param, sql, 0, System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        sql.Commit();
                        return true;
                    }
                    else
                    {
                        sql.Rollback();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    sql.Rollback();
                    throw;
                }
            }

        }
    }
}
