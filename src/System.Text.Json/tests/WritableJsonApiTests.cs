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
using System.Dynamic;

namespace System.Text.Json.Tests
{
    public static class WritableJsonApiTests
    {
        /// <summary>
        /// Helper class simulating external library
        /// </summary>
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

            /// <summary>
            /// Returns following JsonObject:
            /// {
            ///     { "name" : "John" }
            ///     { "phone numbers" : { "work" :  "123-456-7890", "home": "123-456-7890"  } }
            ///     { 
            ///         "reporting employees" : 
            ///         {
            ///             "software developers" :
            ///             {
            ///                 "full time employees" : /JsonObject of 3 employees fromk database/ 
            ///                 "intern employees" : /JsonObject of 2 employees fromk database/ 
            ///             },
            ///             "HR" : /JsonObject of 10 employees fromk database/ 
            ///         }
            /// </summary>
            /// <returns></returns>
            public static JsonObject GetManager()
            {
                return new JsonObject
                {
                    { "name", "John" },
                    {
                        "phone numbers", new JsonObject()
                        {
                            { "work", "123-456-7890" }, { "home", "123-456-7890" }
                        }
                    },
                    {
                        "reporting employees", new JsonObject()
                        {
                            {
                                "software developers", new JsonObject()
                                {
                                    {
                                        "full time employees", new JsonObject()
                                        {
                                            EmployeesDatabase.GetNextEmployee(),
                                            EmployeesDatabase.GetNextEmployee(),
                                            EmployeesDatabase.GetNextEmployee(),
                                        }
                                    },
                                    {
                                        "intern employees", new JsonObject()
                                        {
                                            EmployeesDatabase.GetNextEmployee(),
                                            EmployeesDatabase.GetNextEmployee(),
                                        }
                                    }
                                }
                            },
                            {
                                "HR", new JsonObject()
                                {
                                    {
                                        "full time employees", new JsonObject(EmployeesDatabase.GetTenBestEmployees())
                                    }
                                }
                            }
                        }
                    }
                };
            }

            public static bool CheckSSN(string ssnNumber) => true;
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
                { "null property", (JsonNull) null }
            };
        }

        /// <summary>
        /// Creating simple Json object by new methods on primary types
        /// </summary>
        [Fact]
        public static void TestCreatingJsonObjectNewMethods()
        {
            var developer = new JsonObject
            {
                { "name", new JsonString("Kasia") },
                { "age", new JsonNumber(22) },
                { "is developer", new JsonBool(true) },
                { "null property", new JsonNull() }
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
                { "colours", new JsonArray { "red", "green", "purple" } },
                { "numbers", new JsonArray { 4, 123, 88 } },
                { "primeNumbers", new JsonNumber[] { 19, 37 } },
                { "varia", new JsonArray { 17, "green", true } },
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

        /// <summary>
        /// Contains Checks
        /// </summary>
        [Fact]
        public static void ContainsChecks()
        {
            var person = new JsonObject
            {
                { "name", "John" },
                { "ssn", "123456789" },
            };

            if (person.ContainsProperty("ssn"))
            {
                EmployeesDatabase.CheckSSN(person["ssn"].ToString());
            }

            var enabledOptions = new JsonArray
            {
                "readonly",
                "no cache",
                "continue on failure"
            };

            if(enabledOptions.Contains((JsonString)"no cache"))
            {
                // do sth without using caching
            }
        }

        /// <summary>
        /// Modifying Json object's primnary types
        /// </summary>
        [Fact]
        public static void ModifyingJsonObjectPrimaryTypes()
        {
            var person = new JsonObject
            {
                { "name", "John" },
                { "age", 45 },
                { "is_married", true }
            };

            // Assign by creating a new instance of primary Json type
            person["name"] = new JsonString("Bob");

            // Assign by using an implicit operator on primary Json type
            JsonNumber newAge = 55;
            person["age"] = newAge;

            // Assign by explicit cast from Json primary type
            person["is_married"] = (JsonBool) true;

            // Not possible scenario (wold require implicit cast operators in JsonNode):
            // person["name"] = "Bob";
        }

        /// <summary>
        /// Accesing nested Json object - casting with as operator
        /// </summary>
        [Fact]
        public static void AccesingNestedJsonObjectCastWithAs()
        {
            // Casting with as operator
            var manager = EmployeesDatabase.GetManager();

            var reportingEmployees = manager["reporting employees"] as JsonObject;
            if (reportingEmployees == null) throw new InvalidCastException();

            var softwareDevelopers = reportingEmployees["software developers"] as JsonObject;
            if (softwareDevelopers == null)  throw new InvalidCastException();

            var internDevelopers = softwareDevelopers["intern employees"] as JsonObject;
            if (internDevelopers == null)  throw new InvalidCastException();

            internDevelopers.Add(EmployeesDatabase.GetNextEmployee());
        }

        /// <summary>
        /// Accesing nested Json object - casting with is operator
        /// </summary>
        [Fact]
        public static void AccesingNestedJsonObjectCastWithIs()
        {
            var manager = EmployeesDatabase.GetManager();

            if (manager["reporting employees"] is JsonObject reportingEmployees)
            {
                if (reportingEmployees["software developers"] is JsonObject softwareDevelopers)
                {
                    if (softwareDevelopers["full time employees"] is JsonObject fullTimeEmployees)
                    {
                        fullTimeEmployees.Add(EmployeesDatabase.GetNextEmployee());
                    }
                }
            }
        }

        /// <summary>
        /// Accesing nested Json object - explicit casting
        /// </summary>
        [Fact]
        public static void AccesingNestedJsonObjectExplicitCast()
        {
            // Casting with as operator
            var manager = EmployeesDatabase.GetManager();

            // Casting with explicit cast
            ((JsonObject)((JsonObject)manager["reporting employees"])["HR"]).Add(EmployeesDatabase.GetNextEmployee());
        }

        /// <summary>
        /// Modifying Json object key - remove & add
        /// </summary>
        [Fact]
        public static void ModifyingJsonObjectKeyRemoveAdd()
        {
            var manager = EmployeesDatabase.GetManager();
            var reportingEmployees = manager["reporting employees"] as JsonObject;

            var softwareDevelopers = reportingEmployees["software developers"];
            reportingEmployees.Remove("software developers");
            reportingEmployees.Add("software engineers", softwareDevelopers);
        }

        /// <summary>
        /// Modifying Json object key - modify method
        /// </summary>
        [Fact]
        public static void ModifyingJsonObjectKeyModifyMethod()
        {
            var manager = EmployeesDatabase.GetManager();
            var reportingEmployees = manager["reporting employees"] as JsonObject;

            reportingEmployees.ModifyPropertyName("software developers", "software engineers");
        }
    }
}
