using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    [Table("SessionExam")]
    public class SessionExam
    {
        private DateTime createdDate = DateTime.Now;
        [Key]
        public int ID { get; set; }
        public int ExamTypeID { get; set; }
        public int SessionID { get; set; }
        public string SessionName { get; set; }
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int TotalMarks { get; set; }
        public DateTime CreatedOn { get { return createdDate; } set { createdDate = value; } }
        public DateTime? UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    [Table("StudentExamPerformance")]
    public class StudentExamPerformance
    {
        private DateTime createdDate = DateTime.Now;
        [Key]
        public int ID { get; set; }
        public int SessionExamID { get; set; }
        public int RegistrationID { get; set; }
        public string StudentName { get; set; }
        public int MarksAcquired { get; set; }
        public int MarksAcquiredOnCreation { get; set; }
        public DateTime CreatedOn { get { return createdDate; } set { createdDate = value; } }
        public DateTime? UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int TotalMarks { get; set; }
    }

    [NotMapped]
    public class SubjectPerformanceViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Exam Type")]
        public int ExamTypeID { get; set; }
        public string ExamTypeName { get; set; }
        public List<DropDown> ListofExamType { get; set; }
        [Display(Name = "Session")]
        public int SessionID { get; set; }
        public string SessionName { get; set; }

        [Display(Name = "Class")]
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public List<DropDown> ListofClass { get; set; }

        [Display(Name = "Subject")]
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public List<DropDown> ListofSubjects { get; set; }

        [Display(Name = "Total Marks")]
        public int TotalMarks { get; set; }
        public int SessionExamID { get; set; }
        public int CreatedBy { get; set; }
        public List<StudentPerformanceViewModel> ListofStudent { get; set; }
    }

    [NotMapped]
    public class StudentPerformanceViewModel
    {
        public int ID { get; set; }
        public int RegistrationID { get; set; }
        public string StudentName { get; set; }
        public int MarksAcquired { get; set; }
        public int TotalMarks { get; set; }
        public string ClassName { get; set; }
        public float Percentage { get; set; }
    }
}
