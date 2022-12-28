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

    public async Task<Region> AddAsync(Region region)
    {
        region.Id = Guid.NewGuid();
        await _nzWalksDbContext.AddAsync(region);
        await _nzWalksDbContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> DeleteAsync(Guid id)
    {
        var wasTheRegionFound = await _nzWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (wasTheRegionFound == null)
        {
            return null;
        }

        _nzWalksDbContext.Regions.Remove(wasTheRegionFound);
        await _nzWalksDbContext.SaveChangesAsync();
        return wasTheRegionFound;
    }

    public async Task<IEnumerable<Region>> GetAllAsync()
    {
        return await _nzWalksDbContext.Regions.ToListAsync();
    }

    public async Task<Region> GetAsync(Guid id)
    {
        return await _nzWalksDbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
    }

    public async Task<Region> GetByNameAsync(string name)
    {
        return await _nzWalksDbContext.Regions.FirstOrDefaultAsync(region => region.Name == name);
    }

    public async Task<Region?> UpdateAsync(Guid id, Region region)
    {
        var wasRegionFound = await _nzWalksDbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);

        if (wasRegionFound == null)
        {
            return null;
        }

        wasRegionFound.Area = region.Area;
        wasRegionFound.Walks = region.Walks;
        wasRegionFound.Name = region.Name;
        wasRegionFound.Lat = region.Lat;
        wasRegionFound.Long = region.Long;
        wasRegionFound.Population = region.Population;

        await _nzWalksDbContext.SaveChangesAsync();

        return wasRegionFound;
    }
}