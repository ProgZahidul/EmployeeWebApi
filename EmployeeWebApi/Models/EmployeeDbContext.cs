using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EmployeeWebApi.Models
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }
        public DbSet<EmployeeInfo> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeInfo>().HasData(
                new EmployeeInfo { Id = 1, Name = "Alice Johnson", Department = "HR", IsParmanent = true, Salary = 60000, ImgPath = null },
                new EmployeeInfo { Id = 2, Name = "Bob Smith", Department = "IT", IsParmanent = false, Salary = 55000, ImgPath = null }

            );
        }
    }
}
