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

        //public async Task<Land> CreateLandAsync(Land land)
        //{
        //    if (land == null)
        //        throw new ArgumentNullException(nameof(land), "Land not found");

        //    await _context.AddAsync(land);
        //    await _context.SaveChangesAsync();
        //    return land;
        //}

        public async Task<Land?> GetLandByIdAsync(int landId)
        {
            return await _context.FindAsync<Land>(landId);
        }

        public async Task<Land> CreateOrUpdateLandtAsync(Land land)
        {
            if (land == null)
                throw new ArgumentNullException(nameof(land), "Land Id can not null or empty");

            var existingLand = await _context.FindAsync<Land>(land.Id);
            if (existingLand == null)
                return null;

            _context.Entry(existingLand).CurrentValues.SetValues(land);
            await _context.SaveChangesAsync();
            return existingLand;
        }

        public async Task<bool> DeleteLandByIdAsync(int landId)
        {
            var land = await _context.FindAsync<Land>(landId);
            if (land == null)
                return false;

            _context.Remove(land);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<double> CalculateUsableAreaAsync(double areaSize, int landId)//??????
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddCropToHistoryAsync(string cropName)   //relation
        { 
            throw new NotImplementedException();
        }

        public async Task ConvertToM2Async(int landId)
        {
            if (landId == 0)
                throw new ArgumentException("Land ID cannot be null or zero.", nameof(landId));

            var existingLand = await _context.FindAsync<Land>(landId);
            if (existingLand == null)
                throw new KeyNotFoundException($"Land with ID {landId} not found.");

            if (existingLand.areaSize <= 0)
                throw new InvalidOperationException("Land Area Size cannot be zero or negative.");

            existingLand.areaSizeInM2 = existingLand.areaSize * 10000;

            _context.Update(existingLand);
            await _context.SaveChangesAsync();
        }


        //public async Task<List<Land>> GetAllLandsAsync(int farmerId)
        //{
        //    return await _context.Set<Land>().ToListAsync();
        //}
    }
}
