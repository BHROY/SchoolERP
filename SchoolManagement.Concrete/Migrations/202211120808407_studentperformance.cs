namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class studentperformance : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SubjectPerformance", newName: "SessionExam");
            CreateTable(
                "dbo.StudentExamPerformance",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SessionExamID = c.Int(nullable: false),
                        RegistrationID = c.Int(nullable: false),
                        StudentName = c.String(),
                        MarksAcquired = c.Int(nullable: false),
                        MarksAcquiredOnCreation = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        TotalMarks = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.SessionExam", "RegistrationID");
            DropColumn("dbo.SessionExam", "MarksAcquired");
            DropColumn("dbo.SessionExam", "MarksAcquiredOnCreation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SessionExam", "MarksAcquiredOnCreation", c => c.Int(nullable: false));
            AddColumn("dbo.SessionExam", "MarksAcquired", c => c.Int(nullable: false));
            AddColumn("dbo.SessionExam", "RegistrationID", c => c.Int(nullable: false));
            DropTable("dbo.StudentExamPerformance");
            RenameTable(name: "dbo.SessionExam", newName: "SubjectPerformance");
        }
    }
}
