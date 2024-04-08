using CsvHelper;

namespace FileLoader.Extensions;

public static class CsvReaderExtensions
{
    public static bool TryValidateHeader(this CsvReader reader, Type type)
    {
        try
        {
            if (reader.Configuration.HasHeaderRecord && reader.HeaderRecord == null)
            {
                if (!reader.Read())
                {
                    return false;
                }

                if (!reader.ReadHeader())
                {
                    return false;
                }
                
                reader.ValidateHeader(type);
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public static bool TryValidateHeader<T>(this CsvReader reader) => reader.TryValidateHeader(typeof(T));
}