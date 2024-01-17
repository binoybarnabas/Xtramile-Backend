﻿using XtramileBackend.Data;
using XtramileBackend.Repositories.EmployeeRepository;
using XtramileBackend.Repositories.PriorityRepository;
using XtramileBackend.Repositories.ProjectRepository;

namespace XtramileBackend.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IPriorityRepository PriorityRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public int Complete();
        //public void Dispose();
    }
}
