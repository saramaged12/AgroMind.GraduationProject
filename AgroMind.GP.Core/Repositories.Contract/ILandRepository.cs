using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Repositories.Contract
{
	public interface ILandRepository
	{
        Task<Land> CreateLandAsync(Land land);

        Task<Land?> UpdateLandtAsync(Land land);

        Task<bool> DeleteLandByIdAsync(int landId);

        //Task<Land?> GetAllLandsAsync(int farmerId);

        Task<Land?> GetLandByIdAsync(int LandId);

        

    }
}
