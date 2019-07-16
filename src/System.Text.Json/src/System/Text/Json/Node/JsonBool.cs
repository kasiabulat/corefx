// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json
{ 
#pragma warning disable CS1591
    public partial class JsonBool : JsonNode
    {
        public JsonBool() { }
        public JsonBool(bool value) { }

        public bool GetBool() { throw null; }

        public static implicit operator JsonBool(bool value) { throw null; }
    }
#pragma warning restore CS1591
}
