using FoxTrack.DTO;
using System.Collections.Generic;

public class EE2Manager
{
    private readonly EE2DAO _dao;

    public EE2Manager(EE2DAO dao)
    {
        _dao = dao;
    }

    public async Task<EE2DTO> CalculateEE2Async()
    {
        double injectionSum = await _dao.GetSumInjectionAsync();
        double consumptionSum = await _dao.GetSumConsumptionAsync();

        double ee2 = (injectionSum <= consumptionSum) ? 0 : (consumptionSum - injectionSum);

        if (ee2 > 0)
        {
            List<DateTime> timestampsWithSurplus = await GetTimestampsWithSurplus();
            double totalExcessCost = 0;

            foreach (var timestamp in timestampsWithSurplus)
            {
                double hourlyRate = await _dao.GetHourlyRateAsync(timestamp);
                totalExcessCost += hourlyRate;
            }

            ee2 = totalExcessCost;
        }

        return new EE2DTO
        {
            InjectionSum = injectionSum,
            ConsumptionSum = consumptionSum,
            EE2 = ee2,
            Timestamp = DateTime.Now
        };
    }

    private async Task<List<DateTime>> GetTimestampsWithSurplus()
    {
        return new List<DateTime> { DateTime.Now.AddHours(-1), DateTime.Now };
    }
}

