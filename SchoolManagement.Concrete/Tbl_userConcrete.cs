using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Interface;
using SchoolManagement.Models;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;

namespace SchoolManagement.Concrete
{
    public class Tbl_userConcrete : ITbl_user
    {
        //all clear
        public bool CheckUserNameExists(string Username)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from user in _context.Tbl_user
                                  where user.Username == Username
                                  select user).Count();

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CheckUserDocExists(int DocType, int UserID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from doc in _context.Documents
                                  where doc.DocumentType == DocType && doc.UserID == UserID
                                  select doc).FirstOrDefault();

                    if (result != null)
                    {
                        return result.DocumentID;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int AddUser(Tbl_user entity)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.Tbl_user.Add(entity);
                    return _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EditUser(Tbl_user entity)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.Entry(entity).State=System.Data.Entity.EntityState.Modified;
                    return _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddUserDocuments(Documents entity)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.Documents.Add(entity);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUserDocuments(Documents entity, bool IsDelete)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    Documents UserDoc = _context.Documents.Find(entity.DocumentID);

                    if (IsDelete == false)
                    {
                        UserDoc.DocumentPath = entity.DocumentPath;
                        UserDoc.DocumentType = entity.DocumentType;
                        UserDoc.DocumentName = entity.DocumentName;
                        UserDoc.CreatedOn = entity.CreatedOn;
                        UserDoc.IsDeleted = IsDelete;
                    }
                    else
                        UserDoc.IsDeleted = IsDelete;
                    _context.Entry(UserDoc).State = System.Data.Entity.EntityState.Modified;
                    if (_context.SaveChanges() > 0)
                    { return true; }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<Tbl_user> ListofRegisteredUser(string sortColumn, string sortColumnDir, string Search)
        {
            try
            {
                var _context = new DatabaseContext();

                var IQueryableRegistered = (from register in _context.Tbl_user
                                            select register
                                );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    IQueryableRegistered = IQueryableRegistered.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    IQueryableRegistered = IQueryableRegistered.Where(m => m.Username.Contains(Search) || m.Name.Contains(Search));
                }

                return IQueryableRegistered;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePassword(string RegistrationID,string Password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
                {
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", RegistrationID);
                    param.Add("@Password", Password);
                    var result = con.Execute("Usp_UpdatePasswordbyRegistrationID", param, null, 0, System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return true; 
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddStudent(Tbl_user entity)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
                {
                    var param = new DynamicParameters();
                    param.Add("@ID", entity.RegistrationID);
                    param.Add("@Name", entity.Name);
                    param.Add("@Password", entity.Password);
                    param.Add("@Gender", entity.Gender);
                    param.Add("@CreatedOn", entity.CreatedOn);
                    param.Add("@DateofJoining", entity.DateofJoining);
                    param.Add("@Birthdate", entity.Birthdate);
                    param.Add("@EmailID", entity.EmailID);
                    param.Add("@FatherName", entity.FatherName);
                    param.Add("@MotherName", entity.MotherName);
                    param.Add("@Mobileno", entity.Mobileno);
                    param.Add("@Address", entity.Address);
                    param.Add("@RoleID", entity.RoleID);
                    param.Add("@IsDleted", entity.IsDleted);
                    param.Add("@StudentClassID", entity.StudentClassID);
                    var result = con.Query<int>("Usp_InsertStudentInfo", param, null, true, 0, System.Data.CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public List<StudentSessionInfo> SelectSessionForBackAdmission(DateTime DateofJoining)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDBEntities"].ToString()))
        //        {
        //            List<StudentSessionInfo> StudentSessionList = new List<StudentSessionInfo>();
        //            var param = new DynamicParameters();
        //            param.Add("@DateofJoining", DateofJoining);
        //            var result = con.Query<DropDown>("Usp_SelectSessionForBackAdmission", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
        //            foreach (DropDown res in result)
        //            {
        //                StudentSessionInfo SessionInfo = new StudentSessionInfo();
        //                SessionInfo.SessionID = res.Value;
        //                SessionInfo.StudentSessionText = res.Text;
        //                StudentSessionList.Add(SessionInfo);
        //            }
        //            return StudentSessionList;
        //            //var param = new DynamicParameters();
        //            //param.Add("@DateofJoining", DateofJoining);
        //            //SqlDataReader result = con.ExecuteReader("Usp_SelectSessionForBackAdmission", param, null, 0, System.Data.CommandType.StoredProcedure);
        //            //if (result > 0)
        //            //{
        //            //    return true;
        //            //}
        //            //else
        //            //{
        //            //    return false;
        //            //}
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

     }
}
