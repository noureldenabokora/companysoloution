using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class DataBaseEntites :IdentityDbContext<AppliactionUser>
    {

        public DataBaseEntites(DbContextOptions<DataBaseEntites> options) :base(options) 
        {
            
        }
/*        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server= DESKTOP-626PATH\\SQLEXPRESS; database =MvcApp ;Integrated Security=True; TrustServerCertificate=True;");
        }*/

      public  DbSet<Department> Departments {  get; set; }

        public DbSet<Employee> Employes { get; set; }

    }
}
