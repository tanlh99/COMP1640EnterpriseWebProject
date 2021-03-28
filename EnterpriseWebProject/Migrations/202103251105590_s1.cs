namespace EnterpriseWebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YearNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Magazines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        OpenDate = c.String(nullable: false),
                        CloseDate = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        AcademicYearID = c.Int(nullable: false),
                        PDFfile = c.String(),
                        ImgFile = c.String(),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYearID, cascadeDelete: true)
                .Index(t => t.AcademicYearID);
            
            CreateTable(
                "dbo.Magazine_Faculty",
                c => new
                    {
                        MagazineId = c.Int(nullable: false),
                        FacultyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MagazineId, t.FacultyId })
                .ForeignKey("dbo.Faculties", t => t.FacultyId, cascadeDelete: true)
                .ForeignKey("dbo.Magazines", t => t.MagazineId, cascadeDelete: true)
                .Index(t => t.MagazineId)
                .Index(t => t.FacultyId);
            
            CreateTable(
                "dbo.Contributions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubmitDate = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        AccountId = c.Int(nullable: false),
                        MagazineId = c.Int(nullable: false),
                        FacultyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Magazine_Faculty", t => new { t.MagazineId, t.FacultyId }, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => new { t.MagazineId, t.FacultyId });
            
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false),
                        DOB = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Avatar = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Account_Faculty",
                c => new
                    {
                        AccountId = c.Int(nullable: false),
                        FacultyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccountId, t.FacultyId })
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Faculties", t => t.FacultyId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.FacultyId);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        SubmitDate = c.String(),
                        AccountId = c.Int(),
                        ContributionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .ForeignKey("dbo.Contributions", t => t.ContributionId)
                .Index(t => t.AccountId)
                .Index(t => t.ContributionId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileType = c.String(),
                        FileName = c.String(),
                        Description = c.String(),
                        ContributionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contributions", t => t.ContributionId, cascadeDelete: true)
                .Index(t => t.ContributionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "ContributionId", "dbo.Contributions");
            DropForeignKey("dbo.Magazine_Faculty", "MagazineId", "dbo.Magazines");
            DropForeignKey("dbo.Contributions", new[] { "MagazineId", "FacultyId" }, "dbo.Magazine_Faculty");
            DropForeignKey("dbo.Contributions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Comments", "ContributionId", "dbo.Contributions");
            DropForeignKey("dbo.Comments", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Magazine_Faculty", "FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.Account_Faculty", "FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.Account_Faculty", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Magazines", "AcademicYearID", "dbo.AcademicYears");
            DropIndex("dbo.Files", new[] { "ContributionId" });
            DropIndex("dbo.Comments", new[] { "ContributionId" });
            DropIndex("dbo.Comments", new[] { "AccountId" });
            DropIndex("dbo.Account_Faculty", new[] { "FacultyId" });
            DropIndex("dbo.Account_Faculty", new[] { "AccountId" });
            DropIndex("dbo.Accounts", new[] { "RoleId" });
            DropIndex("dbo.Contributions", new[] { "MagazineId", "FacultyId" });
            DropIndex("dbo.Contributions", new[] { "AccountId" });
            DropIndex("dbo.Magazine_Faculty", new[] { "FacultyId" });
            DropIndex("dbo.Magazine_Faculty", new[] { "MagazineId" });
            DropIndex("dbo.Magazines", new[] { "AcademicYearID" });
            DropTable("dbo.Files");
            DropTable("dbo.Roles");
            DropTable("dbo.Comments");
            DropTable("dbo.Faculties");
            DropTable("dbo.Account_Faculty");
            DropTable("dbo.Accounts");
            DropTable("dbo.Contributions");
            DropTable("dbo.Magazine_Faculty");
            DropTable("dbo.Magazines");
            DropTable("dbo.AcademicYears");
        }
    }
}
