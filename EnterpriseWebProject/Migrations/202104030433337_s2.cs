namespace EnterpriseWebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contributions", "Status", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contributions", "Status", c => c.Boolean(nullable: false));
        }
    }
}
