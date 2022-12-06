namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_UserDocument",
                c => new
                    {
                        DocumentID = c.Int(nullable: false, identity: true),
                        DocumentPath = c.String(),
                        DocumentName = c.String(),
                        UserID = c.Int(nullable: false),
                        DocumentType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentID);
            
            CreateTable(
                "dbo.DropDown",
                c => new
                    {
                        DropDownID = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Text = c.String(),
                        Value = c.Int(nullable: false),
                        Info = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DropDownID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        Rolename = c.String(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.Tbl_StudentSessionDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RegistrationID = c.Int(nullable: false),
                        Name = c.String(),
                        SessionID = c.Int(nullable: false),
                        ClassID = c.Int(nullable: false),
                        PromotionStatus = c.Int(nullable: false),
                        IsBackDateUpdation = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SubjectPerformance",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExamTypeID = c.Int(nullable: false),
                        SessionID = c.Int(nullable: false),
                        RegistrationID = c.Int(nullable: false),
                        ClassID = c.Int(nullable: false),
                        SubjectID = c.Int(nullable: false),
                        TotalMarks = c.Int(nullable: false),
                        MarksAcquired = c.Int(nullable: false),
                        Percent = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbl_user",
                c => new
                    {
                        RegistrationID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Mobileno = c.String(),
                        EmailID = c.String(),
                        Username = c.String(),
                        Password = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        Birthdate = c.DateTime(),
                        DateofJoining = c.DateTime(),
                        Address = c.String(),
                        MotherName = c.String(),
                        FatherName = c.String(),
                        RoleID = c.Int(),
                        StudentClassID = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        IsDleted = c.Boolean(nullable: false),
                        JoiningSessionID = c.Int(nullable: false),
                        RollNo = c.Int(),
                    })
                .PrimaryKey(t => t.RegistrationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbl_user");
            DropTable("dbo.SubjectPerformance");
            DropTable("dbo.Tbl_StudentSessionDetails");
            DropTable("dbo.Roles");
            DropTable("dbo.DropDown");
            DropTable("dbo.Tbl_UserDocument");
        }
    }
}
