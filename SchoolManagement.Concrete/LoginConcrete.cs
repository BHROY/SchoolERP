﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Interface;
using SchoolManagement.Models;

namespace SchoolManagement.Concrete
{
    public class LoginConcrete : ILogin
    {
        public Tbl_user ValidateUser(string userName, string passWord)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var validate = (from user in _context.Tbl_user
                                    where user.Username == userName && user.Password == passWord
                                    select user).SingleOrDefault();
                    var currentSession = _context.DropDownSet.Where(i => i.Category == "Session" && DateTime.Now >= i.StartDate && DateTime.Now <= i.EndDate);

                    validate.CurrentSessionID = currentSession.FirstOrDefault().Value;

                    return validate;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //for future use
        public bool UpdatePassword(string NewPassword, int UserID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
            {
                con.Open();
                SqlTransaction sql = con.BeginTransaction();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@NewPassword", NewPassword);
                    param.Add("@UserID", UserID);
                    var result = con.Execute("Usp_Updatepassword", param, sql, 0, System.Data.CommandType.StoredProcedure);
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
                catch (Exception)
                {
                    sql.Rollback();
                    throw;
                }
            }
        }

        public string GetPasswordbyUserID(int UserID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var password = (from temppassword in _context.Tbl_user
                                    where temppassword.RegistrationID == UserID
                                    select temppassword.Password).FirstOrDefault();

                    return password;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
