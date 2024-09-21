using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseEntites _dbcontext;
        public IEmployeeRepository EmployeeRepository { get ; set; }
        public IDepartmentRepository DepartmentRepository { get; set ; }

        public UnitOfWork(DataBaseEntites dbcontext)
        {
            EmployeeRepository = new EmployeeRepository(dbcontext);
            
            DepartmentRepository = new DepartmentRepository(dbcontext);
            _dbcontext = dbcontext;
        }
        public async Task<int> Complete()
        {
            return await _dbcontext.SaveChangesAsync();    
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}
