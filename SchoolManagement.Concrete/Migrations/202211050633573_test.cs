namespace SchoolManagement.Concrete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_user", "RollNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_user", "RollNo", c => c.Int());
        }
    }
}
