using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Models;
using System.Linq.Dynamic;
using SchoolManagement.Interface;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace SchoolManagement.Concrete
{
    public class UsersConcrete : IUsers
    {
        //all clear
        public List<UserCount> GetAllCount() //br
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var result = con.Query<UserCount>("Usp_GetAllCount", null, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public IQueryable<RegistrationViewSummaryModel> ShowallUsers(string sortColumn, string sortColumnDir, string Search, int currentSessionID)
        {
            var _context = new DatabaseContext();
            //var currentSession = _context.DropDownSet.Where(i => i.Category == "Session" && DateTime.Now>=i.StartDate && DateTime.Now<=i.EndDate);

            var IQueryableStudent = (from registration in _context.Tbl_user
                                     join studentSessionInfo in _context.StudentSessionInfo on
                                     new { key1 = registration.RegistrationID, key2 = currentSessionID } equals
                                     new { key1 = studentSessionInfo.RegistrationID, key2 = studentSessionInfo.SessionID }
                                     join ClassName in _context.DropDownSet on
                                     new { key1 = "Class", key2 = studentSessionInfo.ClassID } equals
                                     new { key1 = ClassName.Category, key2 = ClassName.Value }
                                     select new RegistrationViewSummaryModel
                                     {
                                         Name = registration.Name,
                                         RegistrationID = registration.RegistrationID,
                                         ClassName = ClassName.Text,
                                         Mobileno = registration.Mobileno,
                                         Username = registration.Username
                                     });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableStudent = IQueryableStudent.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableStudent = IQueryableStudent.Where(m => m.Name == Search);
            }

            return IQueryableStudent;

        }

        public IQueryable<RegistrationViewSummaryModel> ShowallAlumni(string sortColumn, string sortColumnDir, string Search, int currentSessionID)
        {
            var _context = new DatabaseContext();
            var currentStudent = _context.StudentSessionInfo.Where(i => i.SessionID == currentSessionID);
            var IQueryableStudent = (from registration in _context.Tbl_user.Where(p => !currentStudent.Any(p2 => p2.RegistrationID == p.RegistrationID) && p.RoleID == ConstantVariable.StudentRoleID)
                                         //join studentSessionInfo in _context.StudentSessionInfo.Where(i=>i.SessionID!=currentSessionID ) on
                                         //new { key1 = registration.RegistrationID, key2 = registration.JoiningSessionID } equals
                                         //new { key1 = studentSessionInfo.RegistrationID, key2 = studentSessionInfo.SessionID }
                                     select new RegistrationViewSummaryModel
                                     {
                                         Name = registration.Name,
                                         RegistrationID = registration.RegistrationID,
                                         JoiningDate = registration.DateofJoining,
                                         Mobileno = registration.Mobileno,
                                         Username = registration.Username
                                     });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableStudent = IQueryableStudent.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableStudent = IQueryableStudent.Where(m => m.Name == Search);
            }

            return IQueryableStudent;

        }

        public StudentViewModel GetStudentDetailsByRegistrationID(int? RegistrationID)
        {
            try
            {
                var _context = new DatabaseContext();
                var currentSession = _context.DropDownSet.Where(i => i.Category == "Session" && DateTime.Now >= i.StartDate && DateTime.Now <= i.EndDate);

                var IQueryableStudent = (from registration in _context.Tbl_user
                                         join studentSessionInfo in _context.StudentSessionInfo on
                                         new { key1 = registration.RegistrationID, key2 = currentSession.FirstOrDefault().Value } equals
                                         new { key1 = studentSessionInfo.RegistrationID, key2 = studentSessionInfo.SessionID }
                                         join ClassName in _context.DropDownSet on
                                         new { key1 = "Class", key2 = studentSessionInfo.ClassID } equals
                                         new { key1 = ClassName.Category, key2 = ClassName.Value }
                                         where registration.RegistrationID == RegistrationID
                                         select new StudentViewModel
                                         {
                                             Name = registration.Name,
                                             RegistrationID = registration.RegistrationID,
                                             CurrentClassName = ClassName.Text,
                                             CurrentClassID = ClassName.Value,
                                             Mobileno = registration.Mobileno,
                                             Username = registration.Username,
                                             Birthdate = registration.Birthdate,
                                             DateofJoining = registration.DateofJoining,
                                             Gender = registration.Gender,
                                             FatherName = registration.FatherName,
                                             MotherName = registration.MotherName,
                                             StudentClassID = registration.StudentClassID,
                                             JoiningSessionID = registration.JoiningSessionID
                                         });
                return IQueryableStudent.SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DropDown> GetDropDownList(string Category)
        {
            using (var _context = new DatabaseContext())
            {
                var result = (from DropDownSet in _context.DropDownSet
                              where DropDownSet.Category == Category
                              select DropDownSet).ToList();

                return result;
            }
        }

        public List<Documents> GetUserDocument(int UserID)
        {
            var _context = new DatabaseContext();
            List<Documents> _userDet;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@Category", "UserDocument");
                    param.Add("@UserID", UserID);
                    _userDet = con.Query<Documents>("Usp_GetDocuments", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return _userDet;
        }

        public IQueryable<RegistrationViewSummaryModel> ShowAllTeachers(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from registration in _context.Tbl_user.Where(i => i.IsDleted == false && i.RoleID==1)
                                       select new RegistrationViewSummaryModel
                                       {
                                           Name = registration.Name,
                                           RegistrationID = registration.RegistrationID,
                                           EmailID = registration.EmailID,
                                           Mobileno = registration.Mobileno,
                                           Username = registration.Username
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.Name.Contains(Search) || m.Username == Search);
            }

            return IQueryabletimesheet;

        }

        public Tbl_user GetUserDetailsByRegistrationID(int? RegistrationID)
        {
            var _context = new DatabaseContext();
            Tbl_user _userDet = _context.Tbl_user.FirstOrDefault(registrationID => registrationID.RegistrationID == RegistrationID);
           
            return _userDet;
        }

        public StudentViewModel GetStudentAvailableAcademicSession(int? RegistrationID, string AdmissionDate)
        {
            try
            {
                var _context = new DatabaseContext();
                int earliestSessionID;
                int registrationID = Convert.ToInt32(RegistrationID);
                DateTime admissionDate = Convert.ToDateTime(AdmissionDate);
                StudentViewModel studentViewModel = new StudentViewModel();
                var academicSession = _context.DropDownSet.Where(i => i.Category == "Session");
                var StudentSession = _context.StudentSessionInfo.Where(i => i.RegistrationID == registrationID).OrderBy(i => i.SessionID).FirstOrDefault();
                //.OrderBy(i => i.SessionID).FirstOrDefault();

                var AdmDateSession = academicSession.Where(i => i.Category == "Session" && admissionDate >= i.StartDate && admissionDate <= i.EndDate).FirstOrDefault().Value;
                var currentSession = academicSession.Where(i => i.Category == "Session" && DateTime.Now >= i.StartDate && DateTime.Now <= i.EndDate);

                if (RegistrationID == 0)
                {
                    if (AdmDateSession == currentSession.FirstOrDefault().Value)
                        studentViewModel.errorStatus = false;
                    else
                    {
                        studentViewModel.errorStatus = true;
                        studentViewModel.errorMessage = "Admission Date should be within current session";
                    }
                }
                else
                {
                    if (StudentSession == null)
                    {
                        earliestSessionID = currentSession.FirstOrDefault().Value;
                    }
                    else
                        earliestSessionID = StudentSession.SessionID;
                    studentViewModel.CurrentSessionID = currentSession.FirstOrDefault().Value;

                    studentViewModel.RegistrationID = Convert.ToInt32(RegistrationID);
                    studentViewModel.DateofJoining = admissionDate;
                    if (AdmDateSession == earliestSessionID)
                        studentViewModel.errorStatus = false;
                    else if (AdmDateSession < earliestSessionID)
                    {
                        var promotionalStatus = _context.DropDownSet.Where(i => i.Category == "StudentPromotionalStatus" && i.Value != 3).ToList();
                        studentViewModel.errorStatus = false;
                        //List<DropDown> listofSubDropDown = new List<DropDown>();
                        //List<DropDown> listofSubDropDowntemp = new List<DropDown>();
                        //if (StudentSession == null)
                        //   studentViewModel.ListofSubDropDown = academicSession.Where(i => i.Value < earliestSessionID).ToList();
                        //else
                        //    studentViewModel.ListofSubDropDown = academicSession.Where(i => i.Value <= earliestSessionID).ToList();
                        studentViewModel.ListofSubDropDown = academicSession.Where(i => i.Value < earliestSessionID).ToList();
                        studentViewModel.ListofSubDropDown = studentViewModel.ListofSubDropDown.Where(i => i.Value >= AdmDateSession).ToList();
                        //studentViewModel.ListofSubDropDown = listofSubDropDowntemp;
                        //listofSubDropDown = academicSession.Where(i => i.Value >= AdmDateSession && ((i.Value < earliestSessionID && StudentSession == null) || (i.Value <= earliestSessionID && StudentSession != null))).Select(x =>
                        //   new DropDown
                        //   {
                        //       Value = x.Value,
                        //       Text=x.Text
                        //   }).ToList();
                        //studentViewModel.ListofSubDropDown = academicSession.Where(i => i.Value >= AdmDateSession && ((i.Value < earliestSessionID && StudentSession == null) || (i.Value <= earliestSessionID && StudentSession != null))).ToList();
                        foreach (DropDown dropDown in studentViewModel.ListofSubDropDown)
                            dropDown.ListofPromotionalStatus = promotionalStatus;
                    }
                    else
                    {
                        studentViewModel.errorStatus = true;
                        studentViewModel.errorMessage = "Cannot proceed! Student Academic Session Data Already Exisis!";
                    }
                }
                return studentViewModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UpdateStudentSession(StudentViewModel student)
        {
            var _context = new DatabaseContext();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    if (student.ListofSubDropDown.Count > 0)
                    {

                        var param = new DynamicParameters();
                        param.Add("@RegistrationID", student.RegistrationID);
                        bool isStudentsessionDeleted = con.Query<bool>("Usp_DeleteStudentSession", param, null, true, 0, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                        var StudentRegistrationDetails = _context.Tbl_user.Where(i => i.RegistrationID == student.RegistrationID);
                        int result = 0;
                        int p = 0;
                        //var StudentSession = _context.StudentSessionInfo.Where(i => i.RegistrationID == student.RegistrationID);
                        //List<StudentSessionInfo> SessionDeleteList = StudentSession.Where(i=> student.ListofSubDropDown.All(p=>p.Value==i.SessionID)).ToList();
                        //List<DropDown> SessionAddList=student.ListofSubDropDown.Where(i=> StudentSession.All(p=>p.SessionID!=i.Value)).OrderBy(i=>i.Value).ToList();
                        for (p = 0; p < student.ListofSubDropDown.Count; p++)
                        {
                            StudentSessionInfo studentSessionInfo = new StudentSessionInfo();
                            studentSessionInfo.RegistrationID = student.RegistrationID;
                            studentSessionInfo.Name = StudentRegistrationDetails.FirstOrDefault().Name;
                            studentSessionInfo.IsBackDateUpdation = 0;
                            studentSessionInfo.PromotionStatus = student.ListofSubDropDown[p].PromotionalStatus;
                            studentSessionInfo.SessionID = student.ListofSubDropDown[p].Value;
                            studentSessionInfo.ClassID = Convert.ToInt32(student.StudentClassID) + p;
                            studentSessionInfo.CreatedDate = DateTime.Now;
                            studentSessionInfo.UpdatedBy = student.UpdatedByUserID;
                            _context.StudentSessionInfo.Add(studentSessionInfo);
                            result = _context.SaveChanges();
                        }
                        if (p == student.ListofSubDropDown.Count)
                            return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
