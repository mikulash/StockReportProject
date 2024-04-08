namespace FileLoaderTests.TestUtilities;

public static class ListUtilities
{
    public static void CheckAllNonNullPropertiesInList<T>(List<T> list)
    {
        foreach (var property in typeof(T).GetProperties())
        {
            Assert.That(
                list.Select(item => item.GetType().GetProperty(property.Name).GetValue(item)), 
                Is.Not.Null
            );
        }
    }
}