﻿namespace Infrastructure.Exceptions;

public class UnsupportedOperationExpressionException(string opName)
    : Exception($"Operation <<{opName}>> is not supported!");
