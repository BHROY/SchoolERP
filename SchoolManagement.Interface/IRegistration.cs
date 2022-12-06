using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Models;

namespace SchoolManagement.Interface
{
    public interface ITbl_user
    {
        bool CheckUserNameExists(string Username);
        int AddUser(Tbl_user entity);
        int EditUser(Tbl_user entity);
        bool UpdatePassword(string RegistrationID, string Password);
        int AddUserDocuments(Documents entity);
        //------------------------
        IQueryable<Tbl_user> ListofRegisteredUser(string sortColumn, string sortColumnDir, string Search);
        bool UpdateUserDocuments(Documents userDocuments, bool IsDelete);
        int CheckUserDocExists(int doctype, int userID);
        int AddStudent(Tbl_user tbl_);
    }
}
