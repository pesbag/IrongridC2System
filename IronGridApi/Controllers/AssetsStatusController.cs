using IronGridApi.Dtos;
using IronGridApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace IronGridApi.Controllers;

[ApiController]
[Route("api/assets-status")]
public class AssetsStatusController : ControllerBase
{
    private readonly IAssetsStatusRepository _repo;
    public AssetsStatusController(IAssetsStatusRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AllAssetsStatusDto>>> GetAllAssetsStatusAsync()
    {
        return Ok(await _repo.GetAllAssetsWithLiveStatusAsync());
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AllAssetsStatusDto>> GetAssetsStatusById(int id)
    {
        var result = await _repo.GetAllAssetsByIdAsync(id);
        if(result is null) { return NotFound(); }
        return Ok(result);
    }
    [HttpGet("status")]
    public async Task<ActionResult<IEnumerable<AllAssetsStatusDto>>> GetAllByStatusAsync([FromQuery] string status)
    {
        return Ok(await _repo.GetAllAssetsByStatusAsync(status));
    }
}
