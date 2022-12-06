using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    [Table("tbl_user")]
    public class Tbl_user
    {
        private DateTime createdDate = DateTime.Now;
        private bool isDelete = false;
        [Key]
        public int RegistrationID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Mobileno { get; set; }

        public string EmailID { get; set; }

        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        public DateTime? Birthdate { get; set; }

        public DateTime? DateofJoining { get; set; }
        public string Address { get; set; }

        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }

        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        public int? RoleID { get; set; }

        public int? StudentClassID { get; set; }
        public DateTime CreatedOn { get { return createdDate; } set { createdDate = value; } }

        public bool IsDleted { get { return isDelete; } set { isDelete = value; } }
        public int JoiningSessionID { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime UpdatedOn { get { return createdDate; } set { createdDate = value; } }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        //UserStatus transferred/Passout/Delete
        public int UserStatus { get; set; }
        public string UserStatusRemarks { get; set; }
        [NotMapped]
        public int CurrentSessionID { get; set; }
    }

    [Table("Tbl_StudentSessionDetails")]
    public class StudentSessionInfo
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationID { get; set; }
        public string Name { get; set; }
        public int SessionID { get; set; }
        public int ClassID { get; set; }
        public int PromotionStatus { get; set; }
        public int IsBackDateUpdation { get; set; }
        public int RollNo { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public string StudentSessionText { get; set; }
        
    }

    [NotMapped]
    public class StudentViewModel
    {
        [Key]
        public int RegistrationID { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mobileno Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobileno")]
        [Display(Name = "Parents Mobile")]
        public string Mobileno { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string EmailID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? Birthdate { get; set; }

        [Required(ErrorMessage = "Admission Date Required")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Admission Date")]
        public DateTime? DateofJoining { get; set; }
        public string Address { get; set; }

        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }

        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        public int? RoleID { get; set; }

        [Required(ErrorMessage = "Class Required")]
        [Display(Name = "Class")]
        public int? StudentClassID { get; set; }
        public int CurrentClassID { get; set; }
        public string CurrentClassName { get; set; }
        public int JoiningSessionID { get; set; }
        public int CurrentSessionID { get; set; }
        public List<DropDown> ListofClass { get; set; }
        public List<StudentSessionInfo> ListOfStudentSessionInfo { get; set; }
        public bool errorStatus { get; set; }
        public string errorMessage { get; set; }
        public List<DropDown> ListofSubDropDown { get; set; }

        public int UpdatedByUserID { get; set; }
        //UserStatus transferred/Passout/Delete
        public int UserStatus { get; set; }
        public string UserStatusRemarks { get; set; }
    }

    [NotMapped]
    public class StaffViewModel
    {
        [Key]
        public int RegistrationID { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mobileno Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobileno")]
        public string Mobileno { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string EmailID { get; set; }

        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in charaters")]
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }

        [MinLength(7, ErrorMessage = "Minimum Password must be 7 in charaters")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? Birthdate { get; set; }

        [Required(ErrorMessage = "Joining Date Required")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Joining Date")]
        public DateTime? DateofJoining { get; set; }
        public string Address { get; set; }

        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }

        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        [Display(Name = "Role")]
        public int? RoleID { get; set; }
        public int JoiningSessionID { get; set; }
        public List<Roles> ListofRoles { get; set; }
        //UserStatus transferred/Passout/Delete
        public int UserStatus { get; set; }
        public string UserStatusRemarks { get; set; }
    }

    [NotMapped]
    public class UserCount
    {
        public string RoleName { get; set; }
        public int RoleID { get; set; }
        public int countCase { get; set; }
    }



}
