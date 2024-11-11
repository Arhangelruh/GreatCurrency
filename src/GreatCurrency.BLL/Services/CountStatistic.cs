using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Services
{
	public static class CountStatistic
	{
		/// <summary>
		/// Get percent statistic from all time and currency time.
		/// </summary>
		/// <param name="list">List when currense were best.</param>
		/// <param name="alltime">Time period.</param>
		/// <returns>%</returns>
		public static int CountStatic(List<TimeRates> list, TimeSpan alltime)
		{
			TimeSpan countTime = TimeSpan.Zero;

			foreach (var line in list)
			{
				var lineTime = line.endTime - line.startTime;
				countTime += lineTime;
			}

			var allTimeInMinutes = alltime.TotalMinutes;
			var ourBestTime = countTime.TotalMinutes;

			return (int)Math.Round(ourBestTime * 100 / allTimeInMinutes);
		}
	}
}
