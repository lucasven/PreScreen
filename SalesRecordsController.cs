using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace PreScreen
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesRecordsController : ControllerBase
    {
        public decimal CalculateMedianUnitCost(List<SaleRecord> records)
        {
            var orderedRecordsByUnitCost = records.Select(c => c.UnitCost).Order().ToList();
            return orderedRecordsByUnitCost.Count % 2 == 0
                ? ((orderedRecordsByUnitCost[orderedRecordsByUnitCost.Count / 2 - 1] + orderedRecordsByUnitCost[orderedRecordsByUnitCost.Count / 2]) / 2)
                : orderedRecordsByUnitCost[orderedRecordsByUnitCost.Count / 2];
        }

        public SalesSummary CalculateSalesSummary(List<SaleRecord> records)
        {
            if(records == null || records.Count == 0)
                return new SalesSummary();

            var medianUnitCost = CalculateMedianUnitCost(records);
            var mostCommonRegion = records.GroupBy(x => x.Region).OrderByDescending(x => x.Count()).ThenBy(x => x.Key).First().Key;
            var firstOrderDate = records.Min(x => x.OrderDate);
            var lastOrderDate = records.Max(x => x.OrderDate);
            var daysBetween = Math.Round((lastOrderDate - firstOrderDate).TotalDays);
            var totalRevenue = records.Sum(x => x.TotalRevenue);

            return new SalesSummary
            {
                MedianUnitCost = medianUnitCost,
                MostCommonRegion = mostCommonRegion,
                FirstOrderDate = firstOrderDate,
                LastOrderDate = lastOrderDate,
                DaysBetween = daysBetween,
                TotalRevenue = totalRevenue
            };
        }

        [HttpGet]
        public IActionResult GetSummary()
        {
            using (var reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Input\\SalesRecords.csv")))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<SaleRecord>().ToList();
                var summary = CalculateSalesSummary(records);
                return Ok(summary);
            }
        }
    }
}
