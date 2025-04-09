using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Domain.Common.Interfaces;

namespace NeuroMedia.Application.Interfaces.Repositories
{
    public interface IUserInfoRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        Task<T?> GetByOidAsync(string oid);
        Task<T?> GetByEmailAsync(string email);
    }
}
