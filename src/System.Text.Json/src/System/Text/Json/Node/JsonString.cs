// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.System.Text
{
    public partial class JsonString : JsonNode
    {
        public JsonString() { }
        public JsonString(string value) { }

        public string GetString() { throw null; }

        public static implicit operator JsonString(string value) { return new JsonString(value); }
    }
}
