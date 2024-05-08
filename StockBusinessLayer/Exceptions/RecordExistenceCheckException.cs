namespace StockBusinessLayer.Exceptions;

public class RecordExistenceCheckException(string fundName, DateOnly date) 
    : Exception($"Record with Fund <<{fundName}>> and for date <<{date}>> were already uploaded");