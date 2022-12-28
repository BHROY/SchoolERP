using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Models
{
    [NotMapped]
    public class RegistrationViewSummaryModel
    {
        public int RegistrationID { get; set; }
        public string Name { get; set; }
        public string Mobileno { get; set; }
        public string EmailID { get; set; }
        public string Username { get; set; }
        public string ClassName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string RegistrationNo { get; set; }
        public int RollNo { get; set; }
    }
}
