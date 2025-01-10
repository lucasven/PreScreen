namespace UnitTests
{
    using PreScreen;

    public class SalesSummaryInputData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new List<SaleRecord>() };
            yield return new object[] { null };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
