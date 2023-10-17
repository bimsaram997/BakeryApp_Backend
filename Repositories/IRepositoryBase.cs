using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Repositories
{
    public interface IRepositoryBase<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        int Add(TEntity entity);
        int DeleteById(int id);
        int UpdateById(TEntity entity);
    }
}