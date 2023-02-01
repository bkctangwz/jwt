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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id= Guid.NewGuid();  
            await nNZWalksDbContext.Regions.AddAsync(region);  
            await nNZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await nNZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region != null)
            {
                nNZWalksDbContext.Regions.Remove(region);
                await nNZWalksDbContext.SaveChangesAsync();
                return region;
            }
            else
            {
                return null;
            }
            

        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nNZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nNZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await nNZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null) 
            {
                return null;
            
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Area = region.Area;
            existingRegion.Population = region.Population;

            //nNZWalksDbContext.Regions.Update(existingRegion);
            await nNZWalksDbContext.SaveChangesAsync();

            return existingRegion;   
        }
    }
}
