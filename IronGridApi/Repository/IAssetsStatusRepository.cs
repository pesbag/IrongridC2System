using IronGridApi.Dtos;

namespace IronGridApi.Repository;

public interface IAssetsStatusRepository
{
    Task<IEnumerable<AllAssetsStatusDto>> GetAllAssetsWithLiveStatusAsync();
    Task<IEnumerable<AllAssetsStatusDto?>> GetAllAssetsByIdAsync(int id);
    Task<IEnumerable<AllAssetsStatusDto?>> GetAllAssetsByStatusAsync(string status);
}
