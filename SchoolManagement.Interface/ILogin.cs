using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Models;

namespace SchoolManagement.Interface
{
    public interface ILogin
    {
        Tbl_user ValidateUser(string userName, string passWord);
        bool UpdatePassword(string NewPassword, int UserID);
        string GetPasswordbyUserID(int UserID);
    }
}
