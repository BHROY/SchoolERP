using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    [Table("Tbl_UserDocument")]
    public class Documents
    {
        [Key]
        public int DocumentID { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public int UserID { get; set; }
        public int DocumentType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }

        [NotMapped]
        public string DocumentTypeName { get; set; }

    }

    [Table("DropDown")]
    public class DropDown
    {
        [Key]
        public int DropDownID { get; set; }
        public string Category { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
        public string Info { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [NotMapped]
        [Required]
        public int PromotionalStatus { get; set; }
        [NotMapped]
        public List<DropDown> ListofPromotionalStatus { get; set; }
        [NotMapped]
        public bool isCheck { get; set; }
    }
}
