using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.Text.Json.Tests
{
    public static class WritableJsonApiTests
    {
        [Fact]
        public static void TestCreatingJsonObject()
        {
            var simpleJsonObject = new JsonObject
            {
                { "name", "Kasia" },
                { "age", 22 },
                { "is developer", true },
                { "null property", null },
            };
            
        }
    }
}
