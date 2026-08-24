using IronGridApi.Data;
using IronGridApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IronGridApi.Repository;

public class AssetsStatusRepository: IAssetsStatusRepository
{
    private readonly IronGridDbContext _context;
    public AssetsStatusRepository(IronGridDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AllAssetsStatusDto>> GetAllAssetsWithLiveStatusAsync()
    {
        return await _context.Assets
            .Select(r => new AllAssetsStatusDto
            {
                Id = r.Id,
                UnitId = r.UnitId,
                AssetSerial = r.AssetSerial,
                AssetType = r.AssetType,
                RawValue = r.assetLive!.RawValue != null ? r.assetLive.RawValue: "",
                ProcessedStatus=r.assetLive.ProcessedStatus != null ? r.assetLive.ProcessedStatus : "Warning",
                IsVerified=r.assetLive.IsVerified != null ? r.assetLive.IsVerified : false,
                LastUpdate =r.assetLive.LastUpdate != null ? r.assetLive.LastUpdate : DateTime.Now
            })
            .ToListAsync();
    }
    public async Task<IEnumerable<AllAssetsStatusDto?>> GetAllAssetsByIdAsync(int id)
    {
        var AssetExists = _context.Assets.FirstOrDefault(a => a.Id == id);
        if (AssetExists is null) { return null; }

        var result = _context.AssetLiveStatus
           .Select(r => new AllAssetsStatusDto
           {
               Id = r.AssetId,
               UnitId = r.Asset.UnitId,
               AssetSerial = r.Asset.AssetSerial,
               AssetType = r.AssetType,
               RawValue = r.RawValue != null ? r.RawValue : "",
               ProcessedStatus = r.ProcessedStatus != null ? r.ProcessedStatus : "Warning",
               IsVerified = r.IsVerified != null ? r.IsVerified: false,
               LastUpdate = r.LastUpdate != null ? r.LastUpdate : DateTime.Now
           })
           .Where(a=>a.Id==id);
        return result;
    }

    public async Task<IEnumerable<AllAssetsStatusDto?>> GetAllAssetsByStatusAsync(string status)
    {
        var result = await _context.Assets
           .Select(r => new AllAssetsStatusDto
           {
               Id = r.Id,
               UnitId = r.UnitId,
               AssetSerial = r.AssetSerial,
               AssetType = r.AssetType,
               RawValue = r.assetLive!.RawValue != null ? r.assetLive.RawValue : "",
               ProcessedStatus = r.assetLive.ProcessedStatus != null ? r.assetLive.ProcessedStatus : "Warning",
               IsVerified = r.assetLive.IsVerified != null ? r.assetLive.IsVerified : false,
               LastUpdate = r.assetLive.LastUpdate != null ? r.assetLive.LastUpdate : DateTime.Now
           })
           .Where(s => s.ProcessedStatus == status).ToListAsync();
        return result;
    }

}
