using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Models;

namespace SchoolManagement.Interface
{
    public interface IUsers
    {
        //br
        List<UserCount> GetAllCount();
        Tbl_user GetUserDetailsByRegistrationID(int? RegistrationID);
        IQueryable<RegistrationViewSummaryModel> ShowallUsers(string sortColumn, string sortColumnDir, string Search, int currentSessionID);
        IQueryable<RegistrationViewSummaryModel> ShowallAlumni(string sortColumn, string sortColumnDir, string Search, int currentSessionID);
        IQueryable<RegistrationViewSummaryModel> ShowAllTeachers(string sortColumn, string sortColumnDir, string Search);
        List<DropDown> GetDropDownList(string Category);
        List<Documents> GetUserDocument(int UserID);
        StudentViewModel GetStudentDetailsByRegistrationID(int? RegistrationID);
        StudentViewModel GetStudentAvailableAcademicSession(int? RegistrationID,string AdmissionDate);

        bool UpdateStudentSession(StudentViewModel registration);
    }
}
