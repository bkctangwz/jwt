using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Models.Domain;

namespace NZwalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nNZWalksDbContext;

        public RegionRepository(NZWalksDbContext nNZWalksDbContext)
        {
            this.nNZWalksDbContext = nNZWalksDbContext;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nNZWalksDbContext.Regions.ToListAsync();
        }
    }
}
