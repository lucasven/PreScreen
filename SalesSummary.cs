using System.Diagnostics.CodeAnalysis;

namespace PreScreen
{
    [ExcludeFromCodeCoverage]
    public class SalesSummary
    {
        public decimal MedianUnitCost { get; set; }
        public string MostCommonRegion { get; set; }
        public DateTime FirstOrderDate { get; set; }
        public DateTime LastOrderDate { get; set; }
        public double DaysBetween { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
