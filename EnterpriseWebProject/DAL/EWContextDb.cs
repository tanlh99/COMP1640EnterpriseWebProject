using EnterpriseWebProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.DAL
{
    public class EWContextDb : DbContext
    {
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Magazine_Faculty> Magazines_Faculties { get; set; }
        public DbSet<Account_Faculty> Accounts_Faculties { get; set; }

    }
}