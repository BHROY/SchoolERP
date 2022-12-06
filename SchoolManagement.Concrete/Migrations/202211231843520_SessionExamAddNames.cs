namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionExamAddNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionExam", "SessionName", c => c.String());
            AddColumn("dbo.SessionExam", "ClassName", c => c.String());
            AddColumn("dbo.SessionExam", "SubjectName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionExam", "SubjectName");
            DropColumn("dbo.SessionExam", "ClassName");
            DropColumn("dbo.SessionExam", "SessionName");
        }
    }
}
