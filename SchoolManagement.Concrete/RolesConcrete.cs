using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Interface;
using SchoolManagement.Models;

namespace SchoolManagement.Concrete
{
    public class RolesConcrete : IRoles
    {
        //All clear
        public int getRolesofUserbyRolename(string Rolename)
        {
            using (var _context = new DatabaseContext())
            {
                var roleID = (from role in _context.Role
                              where role.Rolename == Rolename
                              select role.RoleID).SingleOrDefault();

                return roleID;
            }
        }

        public List<Roles> ListofRole()
        {
            var _context = new DatabaseContext();
            {
                try
                {
                    List<Roles> result = _context.Role.Where(i => i.RoleID != 2).ToList();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
