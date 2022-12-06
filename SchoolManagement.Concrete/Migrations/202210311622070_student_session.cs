namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class student_session : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StudentSessionDetails", "RollNo", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_StudentSessionDetails", "UpdatedBy", c => c.Int());
            AddColumn("dbo.Tbl_StudentSessionDetails", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.tbl_user", "RegistrationNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_user", "RegistrationNo");
            DropColumn("dbo.Tbl_StudentSessionDetails", "CreatedDate");
            DropColumn("dbo.Tbl_StudentSessionDetails", "UpdatedBy");
            DropColumn("dbo.Tbl_StudentSessionDetails", "RollNo");
        }
    }
}
