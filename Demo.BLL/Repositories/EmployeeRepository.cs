using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenricRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DataBaseEntites dbcontext):base(dbcontext) 
        {
            
        }
    }
}
