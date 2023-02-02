using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Models.Domain;

namespace NZwalks.API.Repositories
{
    public class WalkDifficultyRepos : IWalkDifficultyRepos
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepos(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            await nZWalksDbContext.WalksDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var walkDifficulty = await nZWalksDbContext.WalksDifficulty.FindAsync(id);
            if(walkDifficulty == null)
            {
                return null;
            }
            nZWalksDbContext.WalksDifficulty.Remove(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            var walksDifficulity = await nZWalksDbContext.WalksDifficulty.ToListAsync();
            return walksDifficulity;
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            var walkDifficulity = await nZWalksDbContext.WalksDifficulty.FindAsync(id);
            return walkDifficulity;
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await nZWalksDbContext.WalksDifficulty.FindAsync(id);
            if(existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code = walkDifficulty.Code;

            await nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;
        }

    }
}
