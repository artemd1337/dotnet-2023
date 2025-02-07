﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NonResidentialFund.Model;
using NonResidentialFund.Server.Dto;

namespace NonResidentialFund.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DistrictController : ControllerBase
{
    private readonly IDbContextFactory<NonResidentialFundContext> _contextFactory;
    private readonly ILogger<DistrictController> _logger;
    private readonly IMapper _mapper;

    public DistrictController(IDbContextFactory<NonResidentialFundContext> contextFactory, ILogger<DistrictController> logger, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns all districts
    /// </summary>
    /// <returns>List of districts</returns>
    [HttpGet]
    public async Task<IEnumerable<DistrictGetDto>> Get()
    {
        _logger.LogInformation("Get all districts");
        using var ctx = await _contextFactory.CreateDbContextAsync();
        var districts = await ctx.Districts.ToListAsync();
        return _mapper.Map<IEnumerable<DistrictGetDto>>(districts);
    }

    /// <summary>
    /// Returns the district by the specified id
    /// </summary>
    /// <param name="id">id of the district</param>
    /// <returns>Result of operation and district object</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<DistrictGetDto>> Get(int id)
    {
        using var ctx = await _contextFactory.CreateDbContextAsync();
        var district = await ctx.Districts.FirstOrDefaultAsync(district => district.DistrictId == id);
        if (district == null)
        {
            _logger.LogInformation("Not found district with id: {id}", id);
            return NotFound();
        }
        else
        {
            _logger.LogInformation("Get district with id: {id}", id);
            return Ok(_mapper.Map<DistrictGetDto>(district));
        }
    }

    /// <summary>
    /// Creates new district
    /// </summary>
    /// <param name="district">District to be created</param>
    [HttpPost]
    public async Task<ActionResult<DistrictGetDto>> Post([FromBody] DistrictPostDto district)
    {
        using var ctx = await _contextFactory.CreateDbContextAsync();
        var districtToAdd = _mapper.Map<District>(district);
        ctx.Districts.Add(districtToAdd);
        await ctx.SaveChangesAsync();
        return Ok(_mapper.Map<DistrictGetDto>(districtToAdd));
    }

    /// <summary>
    /// Changes the district by the specified id
    /// </summary>
    /// <param name="id">Id of the district to be changed</param>
    /// <param name="districtToPut">New district data</param>
    /// <returns>Result of operation</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<DistrictGetDto>> Put(int id, [FromBody] DistrictPostDto districtToPut)
    {
        using var ctx = await _contextFactory.CreateDbContextAsync();
        var district = await ctx.Districts.FirstOrDefaultAsync(district => district.DistrictId == id);
        if (district == null)
        {
            _logger.LogInformation("Not found district {id}", id);
            return NotFound();
        }
        else
        {
            ctx.Districts.Update(_mapper.Map(districtToPut, district));
            await ctx.SaveChangesAsync();
            return Ok(_mapper.Map<DistrictGetDto>(district));
        }
    }

    /// <summary>
    /// Removes the district by the specified id
    /// </summary>
    /// <param name="id">Id of the district to be removed</param>
    /// <returns>Result of operation</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        using var ctx = await _contextFactory.CreateDbContextAsync();
        var district = await ctx.Districts.FirstOrDefaultAsync(district => district.DistrictId == id);
        if (district == null)
        {
            _logger.LogInformation("Not found district with id: {id}", id);
            return NotFound();
        }
        else
        {
            ctx.Districts.Remove(district);
            await ctx.SaveChangesAsync();
            return Ok();
        }
    }
}
