namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsdeletedStudentSessionModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StudentSessionDetails", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StudentSessionDetails", "IsDeleted");
        }
    }
}
