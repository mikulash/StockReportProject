namespace FileLoader.Extensions;

public static class StringExtensions
{
    public static string ExcludeCharacters(this string source, char[] charactersToExclude)
    {
        return new string(source
            .Where(c => !charactersToExclude.Contains(c)).ToArray());
    }
}