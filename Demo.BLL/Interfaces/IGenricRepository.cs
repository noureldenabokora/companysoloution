﻿using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenricRepository<T> where T : class
    {
       Task<IEnumerable<T>> GetAll();
       Task<T> Get(int id);

        Task Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
