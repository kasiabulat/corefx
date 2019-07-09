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
                //todo: { "null property", null }, error: ambiguous call
            };  
        }

        [Fact]
        public static void TestCreatingNestedJsonObject()
        {
            var nestedJsonObject = new JsonObject
            {
                { "regular", "property" },
                {
                    "nested", new JsonObject()
                    {
                        { "inner", "property" }
                    }
                }
            };
        }
    }
}
