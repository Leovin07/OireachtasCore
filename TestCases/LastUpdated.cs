using OireachtasAPI;

[TestClass]
public class BillsByLastUpdatedFilterTests
{
    [TestMethod]
    public async Task Filter_ReturnsEmptyList_WhenDataIsNull()
    {
        // Arrange
        var filter = new BillsByLastUpdatedFilter(DateTime.Now, DateTime.Now);

        // Act
        var result = await filter.Filter(null);

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Filter_ReturnsEmptyList_WhenDataIsEmpty()
    {
        // Arrange
        var filter = new BillsByLastUpdatedFilter(DateTime.Now, DateTime.Now);

        // Act
        var result = await filter.Filter(Enumerable.Empty<dynamic>());

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Filter_ReturnsFilteredData_WhenDataContainsBillsWithinDateRange()
    {
        // Arrange
        var since = new DateTime(2021, 1, 1);
        var until = new DateTime(2021, 1, 31);
        var data = new List<dynamic>
        {
            new { bill = new { lastUpdated = new DateTime(2021, 1, 15) } },
            new { bill = new { lastUpdated = new DateTime(2021, 2, 1) } },
            new { bill = new { lastUpdated = new DateTime(2020, 12, 31) } }
        };
        var filter = new BillsByLastUpdatedFilter(since, until);

        // Act
        var result = await filter.Filter(data);

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(new DateTime(2021, 1, 15), result.First().lastUpdated);
    }

    [TestMethod]
    public async Task Filter_ReturnsEmptyList_WhenDataDoesNotContainBillsWithinDateRange()
    {
        // Arrange
        var since = new DateTime(2021, 1, 1);
        var until = new DateTime(2021, 1, 31);
        var data = new List<dynamic>
        {
            new { bill = new { lastUpdated = new DateTime(2020, 12, 31) } },
            new { bill = new { lastUpdated = new DateTime(2021, 2, 1) } }
        };
        var filter = new BillsByLastUpdatedFilter(since, until);

        // Act
        var result = await filter.Filter(data);

        // Assert
        Assert.AreEqual(0, result.Count());
    }
}