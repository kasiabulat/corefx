using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.VisualBasic;
using Xunit;
using System.Linq;

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

        private enum AvailableStateCodes
        {
            WA,
            CA,
            NY,
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
                },
                { 
                    "addresses", new JsonObject()
                    {
                        {
                            "office", new JsonObject()
                            {
                                {  "address line 1", "One Microsoft Way" },
                                {  "city" , "Redmond" } ,
                                {  "zip code" , 98052 } ,
                                {  "state" , (int) AvailableStateCodes.WA }
                            }
                        },
                        { 
                            "home", new JsonObject()
                            {
                                {  "address line 1", "Pear Ave" },
                                {  "address line 2", "1288" },
                                {  "city" , "Mountain View" } ,
                                {  "zip code" , 94043 } ,
                                {  "state" , (int) AvailableStateCodes.CA }
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Defining as KeyValuePair value
        /// </summary>
        [Fact]
        public static void TestAssignmentDefinition()
        {
            var employee = EmployeesDatabase.GetNextEmployee().Value;
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
        /// Adding KeyValuePair from external library after initialization
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

        /// <summary>
        /// Adding KeyValuePairs collection from external library
        /// </summary>
        [Fact]
        public static void TestAddingKeyValuePairsCollection()
        {
            var employees = new JsonObject(EmployeesDatabase.GetTenBestEmployees());
        }

        /// <summary>
        /// Adding KeyValuePairs collection from external library after initialization
        /// </summary>
        [Fact]
        public static void TestAddingKeyValuePairsCollectionAfterInitialization()
        {
            var employees = new JsonObject();
            employees.AddRange(EmployeesDatabase.GetTenBestEmployees());
        }

        /// <summary>
        /// Creating Json array
        /// </summary>
        [Fact]
        public static void TestCreatingJsonArray()
        {
            var preferences = new JsonObject()
            {
                // TODO: can this be made acceptable: {  "colours", { "red", "green", "purple" } },
                { "colours", new JsonArray{ "red", "green", "purple" } },
                { "numbers", new JsonArray{ 123, 19 } },
                { "varia", new JsonArray{ 17, "green", true } }
            };
        }

        /// <summary>
        /// Creating nested Json array
        /// </summary>
        [Fact]
        public static void TestCreatingNestedJsonArray()
        {
            var vertices = new JsonArray()
            {
                new JsonArray
                {
                    new JsonArray
                    {
                        new JsonArray { 0, 0, 0 },
                        new JsonArray { 0, 0, 1 }
                    },
                    new JsonArray
                    {
                        new JsonArray { 0, 1, 0 },
                        new JsonArray { 0, 1, 1 }
                    }
                },
                new JsonArray
                {
                    new JsonArray
                    {
                        new JsonArray { 1, 0, 0 },
                        new JsonArray { 1, 0, 1 }
                    },
                    new JsonArray
                    {
                        new JsonArray { 1, 1, 0 },
                        new JsonArray { 1, 1, 1 }
                    }
                },
            };
        }

        /// <summary>
        /// Adding values to JsonArray
        /// </summary>
        [Fact]
        public static void TestAddingToJsonArray()
        {
            var employeesIds = new JsonArray();

            foreach (var employee in EmployeesDatabase.GetTenBestEmployees())
            {
                employeesIds.Add(employee.Key);
            }
        }

        /// <summary>
        /// Creating Json array from collection
        /// </summary>
        [Fact]
        public static void TestCreatingJsonArrayFromCollection()
        {
            var employeesIds = new JsonArray(EmployeesDatabase.GetTenBestEmployees().Select(employee => new JsonString(employee.Key)));
        }

        /// <summary>
        /// Creating Json array from collection of strings
        /// </summary>
        [Fact]
        public static void TestCreatingJsonArrayFromCollectionOfString()
        {
            var employeesIds = new JsonArray(EmployeesDatabase.GetTenBestEmployees().Select(employee => employee.Key));
        }
    }
}
