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
    public class DepartmentRepository :GenricRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DataBaseEntites dbcontext):base(dbcontext)
        {
            
        }

    }
}
