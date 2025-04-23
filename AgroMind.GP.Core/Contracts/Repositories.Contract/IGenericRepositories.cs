using AgroMind.GP.Core.Contracts.Specifications.Contract;
using AgroMind.GP.Core.Entities;

namespace AgroMind.GP.Core.Contracts.Repositories.Contract
{
    public interface IGenericRepositories<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
        //To Mathc Any Type of Key(String or Int)
    {

        //IReadOnlyList is best Performance Rather Than IEnumerable
        //if we use to Retreive data only and not to iterate on the list
        #region withoutSpec
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Tkey id);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        #endregion

        #region With Specification
        Task<IReadOnlyList<TEntity>> GetAllWithSpecASync(ISpecification<TEntity, Tkey> spec);

        Task<TEntity> GetByIdAWithSpecAsync(ISpecification<TEntity, Tkey> spec);

        //   Task DeleteWithSpecAsync(ISpecification<TEntity, Tkey> spec);

        //Task UpdateWithSpecAsync(ISpecification<TEntity, Tkey> spec ,Action<TEntity> action);

        #endregion


    }
}
