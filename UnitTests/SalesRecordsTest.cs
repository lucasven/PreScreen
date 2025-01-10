namespace UnitTests
{
    using Moq;
    using PreScreen;

    public class SalesRecordsTest
    {
        [Fact]
        public void ProcessSalesRecordsSummary_Returns_CorrectDataDefault()
        {
            //Arrange
            var controller = new SalesRecordsController();
            var data = new List<SaleRecord>
            {
                new SaleRecord(){ OrderDate = DateTime.Now, TotalRevenue = 350, UnitCost = 57, Region = "South" },
                new SaleRecord(){ OrderDate = DateTime.Now.AddDays(-5), TotalRevenue = 200, UnitCost = 83, Region = "North" },
                new SaleRecord(){ OrderDate = DateTime.Now.AddDays(-10), TotalRevenue = 78, UnitCost = 42, Region = "North" },
                new SaleRecord(){ OrderDate = DateTime.Now.AddDays(-3), TotalRevenue = 89, UnitCost = 35, Region = "North" },
            };

            //Act
            var response = controller.CalculateSalesSummary(data);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("North", response.MostCommonRegion);
            Assert.Equal(10, response.DaysBetween);
            Assert.Equal(DateTime.Now.AddDays(-10).Date, response.FirstOrderDate.Date);
            Assert.Equal(DateTime.Now.Date, response.LastOrderDate.Date);
            Assert.Equal(data.Sum(c => c.TotalRevenue), response.TotalRevenue);
        }

        /// <summary>
        /// In case we have 2 regions with the same ammount of data
        /// we will return the one that comes first because of their name order
        /// </summary>
        [Fact]
        public void ProcessSalesRecordsSummary_Returns_CorrectCommonRegion()
        {
            //Arrange
            var controller = new SalesRecordsController();
            var data = new List<SaleRecord>
            {
                new SaleRecord(){ OrderDate = DateTime.Now, TotalRevenue = 350, UnitCost = 57, Region = "South" },
                new SaleRecord(){ OrderDate = DateTime.Now, TotalRevenue = 78, UnitCost = 42, Region = "South" },
                new SaleRecord(){ OrderDate = DateTime.Now, TotalRevenue = 89, UnitCost = 35, Region = "South" },
                new SaleRecord(){ OrderDate = DateTime.Now.AddDays(-5), TotalRevenue = 200, UnitCost = 83, Region = "North" },
                new SaleRecord(){ OrderDate = DateTime.Now.AddDays(-10), TotalRevenue = 78, UnitCost = 42, Region = "North" },
                new SaleRecord(){ OrderDate = DateTime.Now.AddDays(-3), TotalRevenue = 89, UnitCost = 35, Region = "North" },
            };

            //Act
            var response = controller.CalculateSalesSummary(data);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("North", response.MostCommonRegion);
        }

        [Fact]
        public void CalculateMedianUnitCost_Returns_CorrectCalculationForEvenLenghtArray()
        {
            //act
            var controller = new SalesRecordsController();
            var data = new List<SaleRecord>
            {
                new SaleRecord(){ UnitCost = 35 },
                new SaleRecord(){ UnitCost = 42 },//median is the average of this value
                new SaleRecord(){ UnitCost = 57 },//and this one
                new SaleRecord(){ UnitCost = 83 },
            };
            var calculatedValue = (data[1].UnitCost + data[2].UnitCost) / 2;

            //arrange
            var response = controller.CalculateMedianUnitCost(data);

            //assert
            Assert.Equal(calculatedValue, response);
        }

        [Fact]
        public void CalculateMedianUnitCost_Returns_CorrectCalculationForOddLenghtArray()
        {
            //act
            var controller = new SalesRecordsController();
            var data = new List<SaleRecord>
            {
                new SaleRecord(){ UnitCost = 35 },
                new SaleRecord(){ UnitCost = 42 },
                new SaleRecord(){ UnitCost = 57 },//this is the median
                new SaleRecord(){ UnitCost = 83 },
                new SaleRecord(){ UnitCost = 93 },
            };

            //arrange
            var response = controller.CalculateMedianUnitCost(data);

            //assert
            Assert.Equal(data[2].UnitCost, response);
        }

        [Theory]
        [ClassData(typeof(SalesSummaryInputData))]
        public void CalculateSalesSummary_Returns_Empty_When_InputIsEmptyOrNull(List<SaleRecord> data)
        {
            //arrange
            var controller = new SalesRecordsController();

            //var data = new List<SaleRecord>();
            var comparableResult = new SalesSummary();

            //act
            var response = controller.CalculateSalesSummary(data);

            //assert
            Assert.Equal(comparableResult.DaysBetween, response.DaysBetween);
            Assert.Equal(comparableResult.TotalRevenue, response.TotalRevenue);
            Assert.Equal(comparableResult.MedianUnitCost, response.MedianUnitCost);
            Assert.Equal(comparableResult.MostCommonRegion, response.MostCommonRegion);
            Assert.Equal(comparableResult.FirstOrderDate, response.FirstOrderDate);
            Assert.Equal(comparableResult.LastOrderDate, response.LastOrderDate);
        }
    }
}
