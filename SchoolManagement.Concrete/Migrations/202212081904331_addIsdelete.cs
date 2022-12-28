namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsdelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassToSubject", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.SessionExam", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionExam", "IsDeleted");
            DropColumn("dbo.ClassToSubject", "IsDeleted");
        }
    }
}
