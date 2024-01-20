using Models.Data;
using Models.ViewModels.FoodType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Repositories
{
    public interface IRepositoryBase<TEntity>
    {
     
        TEntity GetById(int id);
        int Add(TEntity entity);
        int DeleteById(int id);
        int UpdateById(int id, TEntity entity);
        
    }

    public interface IFoodTypeRepository
    {
        int UpdateFoodTypeCountByFoodTypeId(int Id);
        FoodTypeVM[] ListSimpeleFoodTypes();
    }

   
}