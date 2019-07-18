// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// for now disabling error caused by not adding documentation to methods 
#pragma warning disable CS1591

namespace System.Text.Json
{
    public partial class JsonNode
    {
        public JsonElement AsJsonElement() { throw null; }

        public static JsonNode GetNode(JsonElement jsonElement) { throw null; }
        public static bool TryGetNode(JsonElement jsonElement, out JsonNode jsonNode) { throw null; }

        public static JsonNode Parse(string json) { throw null; }
        public static JsonNode DeepCopy(JsonNode jsonNode) { throw null; }
    }
}

#pragma warning restore CS1591
