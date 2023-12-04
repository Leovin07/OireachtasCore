using OireachtasAPI;

[TestClass]
public class JsonLoaderTests
{
    private JsonLoader _jsonLoader;

    [TestInitialize]
    public void Setup()
    {
        _jsonLoader = new JsonLoader();
    }

    [TestMethod]
    public async Task LoadJson_ValidFilename_ReturnsLoadedJsonData()
    {
        // Arrange
        string filename = "test.json";

        // Act
        dynamic result = await _jsonLoader.LoadJson(filename);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task LoadJson_NullFilename_ThrowsArgumentNullException()
    {
        // Arrange
        string filename = null;

        // Act
        await _jsonLoader.LoadJson(filename);

        // Assert
        // Expects ArgumentNullException to be thrown
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public async Task LoadJson_NonExistentFilename_ThrowsFileNotFoundException()
    {
        // Arrange
        string filename = "nonexistent.json";

        // Act
        await _jsonLoader.LoadJson(filename);

        // Assert
        // Expects FileNotFoundException to be thrown
    }

    [TestMethod]
    public async Task LoadJsonData_ValidInput_ReturnsLoadedJsonData()
    {
        // Arrange
        string input = "test.json";

        // Act
        dynamic result = await _jsonLoader.LoadJsonData(input);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task LoadJsonData_NullInput_ThrowsArgumentNullException()
    {
        // Arrange
        string input = null;

        // Act
        await _jsonLoader.LoadJsonData(input);

        // Assert
        // Expects ArgumentNullException to be thrown
    }

    [TestMethod]
    public async Task LoadJsonData_ValidUrlInput_ReturnsLoadedJsonData()
    {
        // Arrange
        string input = "https://sample.com/sample.json";

        // Act
        dynamic result = await _jsonLoader.LoadJsonData(input);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task LoadJsonData_ExpiredCache_ReturnsLoadedJsonData()
    {
        // Arrange
        string input = "test.json";
        _jsonLoader.LoadJsonData(input).Wait(); // Load data to cache
        _jsonLoader.ClearCache(); // Clear cache to simulate expired cache

        // Act
        dynamic result = await _jsonLoader.LoadJsonData(input);

        // Assert
        Assert.IsNotNull(result);
    }
}