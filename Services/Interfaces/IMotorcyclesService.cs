using Moto.Controllers.DTO;
using Moto.DB.Models.Products;

namespace Moto.Services.Interfaces
{
    public interface IMotorcyclesService
    {
        public Task<IEnumerable<Motorcycle>> GetAll();
        public Task<Motorcycle> GetById(Guid id);
        public Task<bool> DeleteById(Guid id);
        public Task<IEnumerable<Motorcycle>> GetByPage(PageQueryParameters parameters);
        public Task<bool> UpdateById(Guid id, MotorcyclePostData changeData);
        public Task<bool> Create(MotorcyclePostData createData);
        public Task<int> GetCount(PageQueryParameters parameters);
    }
}
