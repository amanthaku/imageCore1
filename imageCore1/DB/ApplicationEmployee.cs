using imageCore1.Models;
using Microsoft.EntityFrameworkCore;

namespace imageCore1.DB
{
    public class ApplicationEmployee : DbContext
    {
        public ApplicationEmployee(DbContextOptions<ApplicationEmployee> options) : base(options)

        {




        }

        public DbSet<EmployTable> Employees { get; set; }

       
    }
}
