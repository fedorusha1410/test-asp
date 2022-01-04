using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T: class
    {
        protected RepositoryContext RepositoryContext;
        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

    }
}
