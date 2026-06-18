using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmployeeManagement.DataAccess
{
    public class EmployeeDbContext : IdentityDbContext<ApplicationModel>
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeModel> Employees { get; set; }
        public class EmployeeDbContextFactory : IDesignTimeDbContextFactory<EmployeeDbContext>
        {
            public EmployeeDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<EmployeeDbContext>();

                optionsBuilder.UseSqlServer(
                    "Server=.;Database=EmployeeManagementDb;Trusted_Connection=True;TrustServerCertificate=True;");

                return new EmployeeDbContext(optionsBuilder.Options);
            }
        }
    }
}