using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Repositories
{
    public class LandRepository : ILandRepository
    {
        private readonly AgroMindContext _context; // Use your actual DbContext

        public LandRepository(AgroMindContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Land> CreateLandAsync(Land land)
        {
            if (land == null)
                throw new ArgumentNullException(nameof(land), "Land cannot be null");

            await _context.AddAsync(land);
            await _context.SaveChangesAsync();
            return land;
        }

        public async Task<Land?> GetLandByIdAsync(int LandId)
        {
            return await _context.FindAsync<Land>(LandId);
        }

        public async Task<Land?> UpdateLandtAsync(Land land)
        {
            if (land == null)
                throw new ArgumentNullException(nameof(land), "Land cannot be null");

            var existingLand = await _context.FindAsync<Land>(land.Id);
            if (existingLand == null)
                return null;

            _context.Entry(existingLand).CurrentValues.SetValues(land);
            await _context.SaveChangesAsync();
            return existingLand;
        }

        public async Task<bool> DeleteLandByIdAsync(int LandId)
        {
            var land = await _context.FindAsync<Land>(LandId);
            if (land == null)
                return false;

            _context.Remove(land);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Land>> GetAllLandsAsync()
        {
            return await _context.Set<Land>().ToListAsync();
        }
    }
}
