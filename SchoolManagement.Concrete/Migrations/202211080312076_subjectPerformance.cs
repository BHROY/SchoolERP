namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subjectPerformance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubjectPerformance", "MarksAcquiredOnCreation", c => c.Int(nullable: false));
            AddColumn("dbo.SubjectPerformance", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.SubjectPerformance", "UpdatedOn", c => c.DateTime());
            AddColumn("dbo.SubjectPerformance", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.SubjectPerformance", "UpdatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_user", "UpdatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.tbl_user", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_user", "UpdatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_user", "UserStatus", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_user", "UserStatusRemarks", c => c.String());
            DropColumn("dbo.SubjectPerformance", "Percent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubjectPerformance", "Percent", c => c.Single(nullable: false));
            DropColumn("dbo.tbl_user", "UserStatusRemarks");
            DropColumn("dbo.tbl_user", "UserStatus");
            DropColumn("dbo.tbl_user", "UpdatedBy");
            DropColumn("dbo.tbl_user", "CreatedBy");
            DropColumn("dbo.tbl_user", "UpdatedOn");
            DropColumn("dbo.SubjectPerformance", "UpdatedBy");
            DropColumn("dbo.SubjectPerformance", "CreatedBy");
            DropColumn("dbo.SubjectPerformance", "UpdatedOn");
            DropColumn("dbo.SubjectPerformance", "CreatedOn");
            DropColumn("dbo.SubjectPerformance", "MarksAcquiredOnCreation");
        }
    }
}
