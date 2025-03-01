﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TrinoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.FunctionKind.java
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FunctionKind
    {
        SCALAR,
        AGGREGATE,
        WINDOW
    }
}
