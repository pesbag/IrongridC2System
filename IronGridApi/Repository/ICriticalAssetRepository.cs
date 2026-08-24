using IronGridApi.Dtos;

namespace IronGridApi.Repository;

public interface ICriticalAssetRepository
{
    Task<IEnumerable<CriticalAssetsDto>> GetAllCriticalAssetsAsync();
    //Task<IEnumerable<UnitIdAssetsDto?>> GetAllCriticalUnitAssetsAsync(int unitId);
}
