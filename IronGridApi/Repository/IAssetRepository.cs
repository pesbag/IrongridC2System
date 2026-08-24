using IronGridApi.Dtos;
using IronGridApi.Models;

namespace IronGridApi.Repository;

public interface IAssetRepository
{
    Task<Asset?> GetByAssetIdAsync(int id);
    Task<Unit?> GetByUnitId(int id);
    Task<CreateUnitDto> CreateUnitAsync(CreateUnitDto newUnit);
    Task DeleteAsync(Asset asset);
    Task UpdateAsync(Asset asset);
}
