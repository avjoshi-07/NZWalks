using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class RegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _nzWalksDbContext;

    public RegionRepository(NZWalksDbContext nzWalksDbContext)
    {
        _nzWalksDbContext = nzWalksDbContext;
    }
    public async Task<IEnumerable<Region>> GetAllAsync()
    {
        return await _nzWalksDbContext.Regions.ToListAsync();
    }
}