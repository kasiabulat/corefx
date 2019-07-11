using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.VisualBasic;
using Xunit;

namespace System.Text.Json.Tests
{
    public static class WritableJsonApiTests
    {
        private static class EmployeesDatabase
        {
            private static int Id = 0;
            public static KeyValuePair<string, JsonNode> GetNextEmployee()
            {
                return new KeyValuePair<string, JsonNode>("employee" + Id++, new JsonObject());
            }

            public static IEnumerable<KeyValuePair<string, JsonNode>> GetTenBestEmployees()
            {
                for (int i = 0; i < 10; i++)
                    yield return GetNextEmployee();
            }
        }

        /// <summary>
        /// Creating simple Json object
        /// </summary>
        [Fact]
        public static void TestCreatingJsonObject()
        {
            var developer = new JsonObject
            {
                { "name", "Kasia" },
                { "age", 22 },
                { "is developer", true },
                /*  TODO: { "null property", null }, error: ambiguous call
                    possible solutions: 
                        "null" -> string
                        nullable annotation only to one of the types possible as arguments? */  
            };
        }

        /// <summary>
        /// Creating nested Json object
        /// </summary>
        [Fact]
        public static void TestCreatingNestedJsonObject()
        {
            var person = new JsonObject
            {
                { "name", "John" },
                { "surname", "Smith" },
                {
                    "phone numbers", new JsonObject()
                    {
                        { "work", "123-456-7890" },
                        { "home", "123-456-7890" }
                    }
                }
            };
        }

        /// <summary>
        /// Adding KeyValuePair from external library
        /// </summary>
        [Fact]
        public static void TestAddingKeyValuePair()
        {
            var employees = new JsonObject
            {
                EmployeesDatabase.GetNextEmployee(),
                EmployeesDatabase.GetNextEmployee(),
                EmployeesDatabase.GetNextEmployee(),
                EmployeesDatabase.GetNextEmployee(),
            };
        }

        /// <summary>
        /// Adding KeyValuePair from external library
        /// </summary>
        [Fact]
        public static void TestAddingKeyValuePairAfterInitialization()
        {
            var employees = new JsonObject();
            foreach(var employee in EmployeesDatabase.GetTenBestEmployees())
            {
                employees.Add(employee);
            }
        }
    }
}
