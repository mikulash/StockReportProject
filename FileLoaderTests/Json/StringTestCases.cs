namespace FileLoaderTests.Json;

public class StringTestCases
{
    public const string RandomJson = "[{\"1\":\"R\",\"2\":\"R\",\"3\":\"R\",\"4\":\"R\",\"5\":\"R\",\"6\":\"R\"," +
                                     "\"7\":\"R\",\"8\":\"R\"},{\"1\":\"T\",\"2\":\"T\",\"3\":\"T\",\"4\":\"T\"," +
                                     "\"5\":\"T\",\"6\":\"T\",\"7\":\"T\",\"8\":\"T\"},{\"1\":\"Q\",\"2\":\"Q\"," +
                                     "\"3\":\"Q\",\"4\":\"Q\",\"5\":\"Q\",\"6\":\"Q\",\"7\":\"Q\",\"8\":\"Q\"}]";

    public const string EmptyJsonArray = "[]";

    public const string RandomString = "Not a Json file format!";

    public const string ValidJson = 
        "[{\"date\":\"03/08/2024\",\"fund\":\"ARKK\",\"company\":\"COINBASE GLOBAL INC -CLASS A\"," +
        "\"ticker\":\"COIN\",\"cusip\":\"19260Q107\",\"shares\":\"3,660,756\"," +
        "\"market value ($)\":\"$888,172,620.72\",\"weight (%)\":\"11.27%\"},{\"date\":\"03/08/2024\"," +
        "\"fund\":\"ARKK\",\"company\":\"TESLA INC\",\"ticker\":\"TSLA\",\"cusip\":\"88160R101\"," +
        "\"shares\":\"3,434,674\",\"market value ($)\":\"$613,604,510.10\",\"weight (%)\":\"7.78%\"}]";

    public const string ValidJsonWithNewKeys = 
        "[{\"date\":\"03/08/2024\",\"fund\":\"ARKK\",\"company\":\"COINBASE GLOBAL INC -CLASS A\"," +
        "\"ticker\":\"COIN\",\"cusip\":\"19260Q107\",\"shares\":\"3,660,756\"," +
        "\"market value ($)\":\"$888,172,620.72\",\"weight (%)\":\"11.27%\", \"somethingNew\":\"NEW\"}]";
}