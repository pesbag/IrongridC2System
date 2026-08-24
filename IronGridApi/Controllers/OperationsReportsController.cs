using IronGridApi.Dtos;
using IronGridApi.Models;
using IronGridApi.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IronGridApi.Controllers;

[ApiController]
[Route("api/reports")]
public class OperationsReportsController : ControllerBase
{
    private readonly ICriticalAssetRepository _repo;
    public OperationsReportsController(ICriticalAssetRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("critical-assets")]
    public async Task<ActionResult<IEnumerable<CriticalAssetsDto>>> GetAllCriticalAssetsAsync()
    {
        return Ok(await _repo.GetAllCriticalAssetsAsync());
    }

    //[HttpGet("unit/{unitid}/assets")]
    //public async Task<ActionResult<IEnumerable<UnitIdAssetsDto>>> GetAllCriticalUnitsAssetsAsync(int unitid)
    //{
    //    var result= await _repo.GetAllCriticalUnitAssetsAsync(unitid);
    //    if(result is null) { return NotFound(); }
    //    return Ok(result);
    //}
}
