
using Dapper;
using SchoolManagement.Interface;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

        public SubjectPerformanceViewModel ShowStudentsClassWiseSubjectWise(string sortColumn, string sortColumnDir, string Search, string examTypeID, string studentClassID, string subjectID, int currentSession)
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

        public bool SetDefaultMarksZeroBySesseionExamID(int ExamSessionID, int createdBy)
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

        public DataSet GetStudentMarksDataForGraph(int registrationID, int CurrentSessionID)
        {
            var _context = new DatabaseContext();
            DataSet ds = new DataSet(); //dataset
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                var cmd = new SqlCommand("[dbo].[Usp_GetStudent_Marks_Analysis]", con);
                cmd.CommandText = "[dbo].[Usp_GetStudent_Marks_Analysis]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RegistrationID", registrationID));
                cmd.Parameters.Add(new SqlParameter("@currentSessionID", CurrentSessionID));

                SqlDataAdapter da = new SqlDataAdapter(); //adapter

                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }

            return ds;
        }
        public List<StudentViewModel> ShowStudentsClassWiseRollnumber(string sortColumn, string sortColumnDir, string Search, string studentClassID, int currentSession)
        {
            var _context = new DatabaseContext();
            int StudentClassID = Convert.ToInt32(studentClassID);
            var studentSessionInfo = (from student in _context.StudentSessionInfo.Where(i => i.SessionID == currentSession && i.ClassID == StudentClassID)
                                      join registration in _context.Tbl_user on
                                          new { key1 = student.RegistrationID } equals
                                         new { key1 = registration.RegistrationID }
                                      select new StudentViewModel
                                      {
                                          ID = student.ID,
                                          Name = student.Name,
                                          RegistrationID = student.RegistrationID,
                                          FatherName = registration.FatherName,
                                          RollNo = student.RollNo
                                      }).ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                studentSessionInfo = studentSessionInfo.Where(m => m.Name == Search || m.RollNo.ToString() == Search).ToList();
            }
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                studentSessionInfo = studentSessionInfo.OrderBy(sortColumn + " " + sortColumnDir).ToList();
            }
            return studentSessionInfo;
        }

        public int InsertUpdateStudentRollNumber(StudentSessionInfo studentSessionInfo)
        {
            var _context = new DatabaseContext();
            if (studentSessionInfo.ID != 0)
            {
                var studentInfoList = _context.StudentSessionInfo.Where(i => i.ClassID == studentSessionInfo.ClassID && i.SessionID == studentSessionInfo.SessionID).ToList();
                var studentCheckRollExists = studentInfoList.Where(i => i.RollNo == studentSessionInfo.RollNo).FirstOrDefault();
                if (studentCheckRollExists != null)
                    return 1; //Roll Number Already Exists
                else
                {
                    var studentRoll = studentInfoList.Where(i => i.ID == studentSessionInfo.ID).FirstOrDefault();
                    studentRoll.RollNo = studentSessionInfo.RollNo;
                    _context.Entry(studentRoll).State = System.Data.Entity.EntityState.Modified;
                    if (_context.SaveChanges() > 0)
                        return 2; //Success
                }
            }
            return 0; //error ID doesnot exist
        }
        public bool SetDefaultAlphabeticallyRollNumber(int ClassID, int CurrentSession)
        {
            try
            {
                var _context = new DatabaseContext();

                if (ClassID != 0)
                {
                    var studentInfoList = _context.StudentSessionInfo.Where(i => i.ClassID == ClassID && i.SessionID == CurrentSession).OrderBy(i => i.Name).ToList();
                    int a = 1;
                    using (System.Data.Entity.DbContextTransaction dbTran = _context.Database.BeginTransaction())
                    {
                        foreach (var student in studentInfoList)
                        {
                            student.RollNo = a;
                            _context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                            if (_context.SaveChanges() == 0)
                            {
                                dbTran.Rollback();
                                return false;
                            }
                            a++;
                        }
                        dbTran.Commit();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public List<DropDown> ShowClassWiseAddedSubjects(string studentClass, int CurrentSessionID)
        {
            try
            {
                var _context = new DatabaseContext();
                var ClassID = Convert.ToInt32(studentClass);
                var SubjectList = _context.DropDownSet.Where(i => i.Category == "Subject").ToList();
                var ClaasWiseSubjects = _context.ClassToSubject.Where(i => i.ClassID == ClassID && i.SessionID == CurrentSessionID && i.IsDeleted == false).ToList();
                SubjectList = SubjectList.Where(i => ClaasWiseSubjects.Any(p => p.SubjectID == i.Value)).ToList();
                return SubjectList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckIfExamHeldForClass(string studentClass, int CurrentSessionID)
        {
            try
            {
                var _context = new DatabaseContext();
                var ClassID = Convert.ToInt32(studentClass);
                var SessionExamtList = _context.SessionExam.Where(i => i.ClassID == ClassID && i.SessionID == CurrentSessionID).Select(r => r.ID).ToList();
                //var ExamSubjects = _context.StudentExamPerformance.ToList();
                //ExamSubjects = ExamSubjects.Where(i => SessionExamtList.Any(p => p.ID == i.SessionExamID)).ToList();
                var ExamSubjects = _context.StudentExamPerformance.Where(i => SessionExamtList.Contains(i.SessionExamID)).ToList();
                if (ExamSubjects.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {

                throw;
            }
        }


        public bool InsertUpdateClassToSubject(List<ClassToSubject> ClassToSubjectList)
        {
            var _context = new DatabaseContext();
            var ClassAndSession = ClassToSubjectList.FirstOrDefault();
            var ClassToSubjectExistingList = _context.ClassToSubject.Where(i => i.ClassID == ClassAndSession.ClassID && i.SessionID == ClassAndSession.SessionID).ToList();
            var ClassToSubjectToBeAddedList = ClassToSubjectList.Where(i => !ClassToSubjectExistingList.Any(p => p.SubjectID == i.SubjectID)).ToList();
            var ClassToSubjectToBeDeletedList = ClassToSubjectExistingList.Where(i => !ClassToSubjectList.Any(p => p.SubjectID == i.SubjectID) && i.IsDeleted == false).ToList();
            var ClassToSubjectToBeUpdatedList = ClassToSubjectExistingList.Where(i => ClassToSubjectList.Any(p => p.SubjectID == i.SubjectID) && i.IsDeleted == true).ToList();

            for (var i = 0; i < ClassToSubjectToBeAddedList.Count; i++)
            {
                _context.ClassToSubject.Add(ClassToSubjectToBeAddedList[i]);
                if (_context.SaveChanges() <= 0)
                { return false; }
            }
            for (var i = 0; i < ClassToSubjectToBeDeletedList.Count; i++)
            {
                ClassToSubjectToBeDeletedList[i].IsDeleted = true;
                _context.Entry(ClassToSubjectToBeDeletedList[i]).State = System.Data.Entity.EntityState.Modified;
                if (_context.SaveChanges() <= 0)
                { return false; }
            }
            for (var i = 0; i < ClassToSubjectToBeUpdatedList.Count; i++)
            {
                ClassToSubjectToBeUpdatedList[i].IsDeleted = false;
                _context.Entry(ClassToSubjectToBeUpdatedList[i]).State = System.Data.Entity.EntityState.Modified;
                if (_context.SaveChanges() <= 0)
                { return false; }
            }
            return false;
        }
        public List<DropDown> GetClassBasedSubjects(int ClassID, int CurrentSessionID)
        {
            var _context = new DatabaseContext();
            var ClassToSubjecList = _context.ClassToSubject.Where(i => i.ClassID == ClassID && i.SessionID == CurrentSessionID && i.IsDeleted == false).Select(r => r.SubjectID).ToList();
            var SubjectList = _context.DropDownSet.Where(i => i.Category == "Subject").ToList();
            if (ClassID > 0)
                SubjectList = SubjectList.Where(i => ClassToSubjecList.Contains(i.Value)).ToList();
            return SubjectList;
        }

        public DataSet GetMarksAnalysisForAdminForGraph(int examTypeID, int classID, int subjectID, int currentSessionID)
        {
            var _context = new DatabaseContext();
            DataSet ds = new DataSet(); //dataset
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                var cmd = new SqlCommand("[dbo].[Usp_Get_Teacher_Marks_Analysis]", con);
                //cmd.CommandText = "[dbo].[Usp_Get_Teacher_Marks_Analysis]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@currentSessionID", currentSessionID));
                cmd.Parameters.Add(new SqlParameter("@ExamTypeID", examTypeID));
                cmd.Parameters.Add(new SqlParameter("@ClassID", classID));
                cmd.Parameters.Add(new SqlParameter("@SubjectID", subjectID));
                SqlDataAdapter da = new SqlDataAdapter(); //adapter

                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }

            return ds;
        }
        public List<StudentPerformanceViewModel> GetGradeWiseStudentList(int examTypeID, int classID, int subjectID, string Grade, int currentSessionID)
        {
            var _context = new DatabaseContext();
            DataSet ds = new DataSet(); //dataset
            List<StudentPerformanceViewModel> StudentPerformanceViewModelList = new List<StudentPerformanceViewModel>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                var param = new DynamicParameters();
                param.Add("@currentSessionID", currentSessionID);
                param.Add("@ExamTypeID", examTypeID);
                param.Add("@ClassID", classID);
                param.Add("@SubjectID", subjectID);
                param.Add("@Grade", Grade);
                var result = con.Query<StudentPerformanceViewModel>("Usp_Get_GradeWiseStudent", param, null, true, 0, System.Data.CommandType.StoredProcedure);
                StudentPerformanceViewModelList = result.ToList();
            }
            return StudentPerformanceViewModelList;
        }
    }
}
