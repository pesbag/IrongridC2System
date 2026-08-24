using IronGridApi.Data;
using IronGridApi.Dtos;
using IronGridApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IronGridApi.Repository;

public class CriticalAssetRepository : ICriticalAssetRepository
{
    private readonly IronGridDbContext _context;
    public CriticalAssetRepository(IronGridDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<CriticalAssetsDto>> GetAllCriticalAssetsAsync()
    {
        var result = _context.AssetLiveStatus
           .Select(r => new CriticalAssetsDto
           {
               AssetId = r.Asset.Id,
               AssetSerial = r.Asset.AssetSerial,
               AssetType = r.AssetType,
               UnitName=r.Asset.Unit.UnitName,
               Sector=r.Asset.Unit.Sector,
               ProcessedStatus = r.ProcessedStatus,
               IsVerified = r.IsVerified,
               LastUpdate = r.LastUpdate
           })
           .Where(r=>r.ProcessedStatus=="Warning" || r.IsVerified==false);
        return await result.ToListAsync();
    }
    //public async Task<IEnumerable<UnitIdAssetsDto?>> GetAllCriticalUnitAssetsAsync(int unitId)
    //{
    //    var exists = _context.Units.FirstOrDefault(u => u.Id == unitId);
    //    if(exists is null) { return null; }
    //    var result = _context.AssetLiveStatus
    //       .Select(r => new UnitIdAssetsDto
    //       {
    //           AssetId = r.AssetId,
    //           AssetSerial = r.Asset.AssetSerial,
    //           AssetType = r.AssetType,
    //           ProcessedStatus = r.ProcessedStatus,
    //           IsVerified = r.IsVerified,
    //           LastUpdate = r.LastUpdate
    //       }).Where(r=>r.Asset.UnitId==r.unitId);
           
    //    return await result.ToListAsync();
    //}
}
