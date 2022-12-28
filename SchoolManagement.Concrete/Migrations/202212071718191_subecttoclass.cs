namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subecttoclass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassToSubject",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SessionID = c.Int(nullable: false),
                        ClassID = c.Int(nullable: false),
                        SubjectID = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClassToSubject");
        }
    }
}
