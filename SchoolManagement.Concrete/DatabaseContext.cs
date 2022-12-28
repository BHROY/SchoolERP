using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Models;

namespace SchoolManagement.Concrete
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=SchoolDBEntities")
        {
        }

        //public DbSet<Registration> Registration { get; set; }
        public DbSet<Roles> Role { get; set; }
        //public DbSet<ProjectMaster> ProjectMaster { get; set; }
        //public DbSet<TimeSheetMaster> TimeSheetMaster { get; set; }
        //public DbSet<TimeSheetDetails> TimeSheetDetails { get; set; }
        //public DbSet<ExpenseModel> ExpenseModel { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<DropDown> DropDownSet { get; set; }
        //public DbSet<TimeSheetAuditTB> TimeSheetAuditTB { get; set; }
        //public DbSet<ExpenseAuditTB> ExpenseAuditTB { get; set; }
        //public DbSet<AuditTB> AuditTB { get; set; }
        //public DbSet<DescriptionTB> DescriptionTB { get; set; }
        //public DbSet<AssignedRoles> AssignedRoles { get; set; }

        //public System.Data.Entity.DbSet<SchoolManagement.Models.NotificationsTB> NotificationsTBs { get; set; }

        //------------------------------------------------------------------------
        public DbSet<Tbl_user> Tbl_user { get; set; }

        public DbSet<StudentSessionInfo> StudentSessionInfo { get; set; }
        public DbSet<SessionExam> SessionExam { get; set; }
        public DbSet<StudentExamPerformance> StudentExamPerformance { get; set; }
        public DbSet<ClassToSubject> ClassToSubject { get; set; }
    }
}
