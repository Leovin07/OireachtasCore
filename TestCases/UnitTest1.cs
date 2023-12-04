using OireachtasAPI;

[TestClass]
public class BillsSponsoredByFilterTests
{
    [TestMethod]
    public async Task Filter_ReturnsEmptyList_WhenDataIsNull()
    {
        // Arrange
        dynamic data = null;
        var filter = new BillsSponsoredByFilter("sponsorId");

        // Act
        var result = await filter.Filter(data);

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Filter_ReturnsEmptyList_WhenDataIsEmpty()
    {
        // Arrange
        dynamic data = Enumerable.Empty<dynamic>();
        var filter = new BillsSponsoredByFilter("sponsorId");

        // Act
        var result = await filter.Filter(data);

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task Filter_ReturnsSponsoredBills_WhenSponsorIdIsNull()
    {
        // Arrange
        dynamic data = new List<dynamic>
        {
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorName" } } } } } },
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorName" } } } } } },
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorName" } } } } } }
        };
        var filter = new BillsSponsoredByFilter(null);

        // Act
        var result = await filter.Filter(data);

        // Assert
        Assert.AreEqual(3, result.Count());
    }

    [TestMethod]
    public async Task Filter_ReturnsSponsoredBills_WhenSponsorIdMatches()
    {
        // Arrange
        dynamic data = new List<dynamic>
        {
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorName" } } } } } },
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorId" } } } } } },
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorName" } } } } } }
        };
        var filter = new BillsSponsoredByFilter("sponsorId");

        // Act
        var result = await filter.Filter(data);

        // Assert
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public async Task Filter_ReturnsEmptyList_WhenSponsorIdDoesNotMatch()
    {
        // Arrange
        dynamic data = new List<dynamic>
        {
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorName" } } } } } },
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorName" } } } } } },
            new { bill = new { sponsors = new List<dynamic> { new { sponsor = new { by = new { showAs = "sponsorName" } } } } } }
        };
        var filter = new BillsSponsoredByFilter("sponsorId");

        // Act
        var result = await filter.Filter(data);

        // Assert
        Assert.AreEqual(0, result.Count());
    }
}