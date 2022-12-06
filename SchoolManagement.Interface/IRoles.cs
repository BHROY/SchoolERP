using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Interface
{
    public interface IRoles
    {
        int getRolesofUserbyRolename(string Rolename);
        List<Roles> ListofRole();
    }
}
