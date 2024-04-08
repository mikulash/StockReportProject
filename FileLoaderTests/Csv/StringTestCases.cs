namespace FileLoaderTests.Csv;

public static class StringTestCases
{
    public const string OnlyCsvHeader = "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"";

    public const string RandomString = "This is definitely not a csv file";
    
    public const string RandomCsvHeader = "random,header,in,csv,format";

    public const string RandomCsv = "1,2,3,4,5,6,7,8\nR,R,R,R,R,R,R,R,R\nT,T,T,T,T,T,T,T\nQ,Q,Q,Q,Q,Q,Q,Q";
    
    public const string AlmostCorrectCsvHeader = "date,fund,company,ticker,cusip,shares,\"market value\",\"weight\"";

    public const string IncorrectHeaderCorrectData =
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"WEIGHT\"\n" +
        "04/03/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%\n" +
        "04/03/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"4,000,201\",\"$666,553,492.63\",9.18%\n";

    public const string CorrectHeaderCorrectData = 
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
        "04/03/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%\n" +
        "04/03/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"4,000,201\",\"$666,553,492.63\",9.18%\n" +
        "04/03/2024,ARKK,\"ROKU INC\",ROKU,77543R102,\"8,878,602\",\"$558,020,135.70\",7.69%\n" +
        "04/03/2024,ARKK,\"BLOCK INC\",SQ,852234103,\"6,295,885\",\"$498,697,050.85\",6.87%";

    public const string CorrectHeaderCorrectDataMixed =
        "fund,date,weight (%),shares,company,ticker,cusip,market value ($)\n" +
        "ARKK,04/03/2024,9.66%,\"2,851,172\",COINBASE GLOBAL INC -CLASS A,COIN,19260Q107,\"$700,932,124.48\"\n" +
        "ARKK,04/03/2024,9.18%,\"4,000,201\",TESLA INC,TSLA,88160R101,\"$666,553,492.63\"\n" +
        "ARKK,04/03/2024,7.69%,\"8,878,602\",ROKU INC,ROKU,77543R102,\"$558,020,135.70\"\n" +
        "ARKK,04/03/2024,6.87%,\"6,295,885\",BLOCK INC,SQ,852234103,\"$498,697,050.85\"";

    public const string CorrectHeaderDateColumnCorrupted =
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
        "SomeInvalidDate,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%\n" +
        "04/03/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"4,000,201\",\"$666,553,492.63\",9.18%";
    
    public const string CorrectHeaderSharesColumnCorrupted =
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
        "04/03/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%\n" +
        "04/03/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"ShareNotFound\",\"$666,553,492.63\",9.18%";
    
    public const string CorrectHeaderMarketValueColumnCorrupted =
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
        "04/03/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%\n" +
        "04/03/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"4,000,201\",\"CannotPrintThisValue\",9.18%";
    
    public const string CorrectHeaderWeightColumnCorrupted =
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
        "04/03/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%\n" +
        "04/03/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"4,000,201\",\"$666,553,492.63\",9.18PERCENTAGE";
    
    public const string CorrectHeaderOneInvalidRecord =
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
        "04/03/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%\n" +
        "Surely,not,a,valid,record,for,this,header,format,.";
    
    public const string CorrectHeaderRotateColumnValue =
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
        "ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%,04/03/2024\n";
    
    public const string CorrectHeaderNewColumnInRecord =
        "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n" +
        "04/03/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,851,172\",\"$700,932,124.48\",9.66%,10.12\n" + 
        "04/03/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"4,000,201\",\"$666,553,492.63\",9.18%";
}
