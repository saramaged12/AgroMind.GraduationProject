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
        //Task<Land> CreateLandAsync(Land land);

        Task<Land> CreateOrUpdateLandtAsync(Land land);

        Task<bool> DeleteLandByIdAsync(int landId);

        //Task<List<Land>> GetAllLandsAsync(int farmerId);

        Task<Land?> GetLandByIdAsync(int landId);

        Task<double> CalculateUsableAreaAsync(double areaSize, int landId);

        Task<bool> AddCropToHistoryAsync(string cropName);

        //Task<bool> UpdateStatusAsync(string newStatus, int landId);   ??????????

        //Task<string> UpdateWeatherConditionAsync(string weatherCondition, int landId);  ??????

        Task ConvertToM2Async(int landId);   // void ---> return type




    }
}
