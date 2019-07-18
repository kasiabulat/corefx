// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json.Serialization.Tests;
using Xunit;


namespace System.Text.Json
{
#pragma warning disable xUnit1000
    internal static class WritableJsonApiTests
#pragma warning enable xUnit1000
    {
        /// <summary>
        /// Helper class simulating external library
        /// </summary>
        private static class EmployeesDatabase
        {
            private static int s_id = 0;
            public static KeyValuePair<string, JsonNode> GetNextEmployee()
            {
                var employee = new JsonObject()
                {
                    { "name", "John" } ,
                    { "surname", "Smith"},
                    { "age", 45 }
                };

                return new KeyValuePair<string, JsonNode>("employee" + s_id++, employee);
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
                var manager = GetNextEmployee().Value as JsonObject;

                manager.Add
                (
                    "phone numbers",
                    new JsonObject()
                    {
                        { "work", "123-456-7890" }, { "home", "123-456-7890" }
                    }
                );

                manager.Add
                (
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
                );

                return manager;
            }

            public static bool CheckSSN(string ssnNumber) => true;
            public static void PerformHeavyOperations(JsonElement employee) { }
        }

        /// <summary>
        /// Helper class simulating sending Json files via network
        /// </summary>
        private static class Mailbox
        {
            public static void SendEmployeeData(JsonElement employeeData) { }
            public static JsonElement RetrieveEmployeeData() { return EmployeesDatabase.GetNextEmployee().Value.AsJsonElement(); }

            public static void SendAllEmployeesData(JsonElement employeesData) { }
        }

        private static class HealthCare
        {
            public static void CreateMedicalAppointment(string personName) { }
        }

        /// <summary>
        /// Helper class simulating enum
        /// </summary>
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
                { "null property", (JsonNode) null }
            };

            Assert.IsType<JsonString>(developer["name"]);
            var developerNameCasted = developer["name"] as JsonString;
            Assert.Equal("Kasia", developerNameCasted.Value);

            Assert.IsType<JsonNumber>(developer["age"]);
            var developerAgeCasted = developer["age"] as JsonNumber;
            Assert.Equal(22, developerAgeCasted.GetInt32());

            Assert.IsType<JsonBoolean>(developer["is developer"]);
            var isDeveloperCasted = developer["is developer"] as JsonBoolean;
            Assert.Equal(true, isDeveloperCasted.Value);

            Assert.Null(developer["null property"]);
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
                { "is developer", new JsonBoolean(true) }
            };

            Assert.IsType<JsonString>(developer["name"]);
            var developerNameCasted = developer["name"] as JsonString;
            Assert.Equal("Kasia", developerNameCasted.Value);

            Assert.IsType<JsonNumber>(developer["age"]);
            var developerAgeCasted = developer["age"] as JsonNumber;
            Assert.Equal(22, developerAgeCasted.GetInt32());

            Assert.IsType<JsonBoolean>(developer["is developer"]);
            var isDeveloperCasted = developer["is developer"] as JsonBoolean;
            Assert.Equal(true, isDeveloperCasted.Value);
        }

        /// <summary>
        /// Creating and retriving different numeric values
        /// </summary>
        [Fact]
        public static void TestNumerics()
        {
            double PI = 3.14159265359;
            var circle = new JsonObject
            {
                { "radius", 1 },
                { "length", 2*PI },
                { "area", PI }
            };

            Assert.IsType<JsonNumber>(circle["radius"]);
            var radius = circle["radius"] as JsonNumber;
            Assert.Equal(radius, 1);

            Assert.IsType<JsonNumber>(circle["length"]);
            var length = circle["length"] as JsonNumber;
            Assert.Equal(length, 2 * PI);

            Assert.IsType<JsonNumber>(circle["area"]);
            var area = circle["area"] as JsonNumber;
            Assert.Equal(area, PI);

            JsonNumber bigConstantBoxed = long.MaxValue;
            long bigConstant = bigConstantBoxed.GetInt64();

            Assert.Equal(long.MaxValue, bigConstant);

            var smallValueBoxed = new JsonNumber(17);
            smallValueBoxed.TryGetInt16(out short smallValue);

            Assert.Equal(17, smallValue);
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

            Assert.IsType<JsonObject>(person["phone numbers"]);
            var phoneNumbers = person["phone numbers"] as JsonObject;
            Assert.IsType<JsonString>(phoneNumbers["work"]);
            Assert.IsType<JsonString>(phoneNumbers["home"]);

            Assert.IsType<JsonObject>(person["addresses"]);
            var addresses = person["office"] as JsonObject;
            Assert.IsType<JsonObject>(addresses["office"]);
            Assert.IsType<JsonObject>(addresses["home"]);
        }

        /// <summary>
        /// Defining as KeyValuePair value
        /// </summary>
        [Fact]
        public static void TestAssignmentDefinition()
        {
            JsonNode employee = EmployeesDatabase.GetNextEmployee().Value;
            Assert.IsType<JsonObject>(employee);
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

            string prevId = "";
            foreach((string id, JsonNode employee) in employees)
            {
                Assert.NotEqual(prevId, id);
                prevId = id;

                Assert.IsType<JsonObject>(employee);
            }
        }

        /// <summary>
        /// Adding KeyValuePair from external library after initialization
        /// </summary>
        [Fact]
        public static void TestAddingKeyValuePairAfterInitialization()
        {
            var employees = new JsonObject();
            foreach (KeyValuePair<string, JsonNode> employee in EmployeesDatabase.GetTenBestEmployees())
            {
                employees.Add(employee);
            }

            string prevId = "";
            foreach ((string id, JsonNode employee) in employees)
            {
                Assert.NotEqual(prevId, id);
                prevId = id;

                Assert.IsType<JsonObject>(employee);
            }
        }

        /// <summary>
        /// Adding KeyValuePairs collection from external library
        /// </summary>
        [Fact]
        public static void TestAddingKeyValuePairsCollection()
        {
            var employees = new JsonObject(EmployeesDatabase.GetTenBestEmployees());

            string prevId = "";
            foreach ((string id, JsonNode employee) in employees)
            {
                Assert.NotEqual(prevId, id);
                prevId = id;

                Assert.IsType<JsonObject>(employee);
            }
        }

        /// <summary>
        /// Adding KeyValuePairs collection from external library after initialization
        /// </summary>
        [Fact]
        public static void TestAddingKeyValuePairsCollectionAfterInitialization()
        {
            var employees = new JsonObject();
            employees.AddRange(EmployeesDatabase.GetTenBestEmployees());

            string prevId = "";
            foreach ((string id, JsonNode employee) in employees)
            {
                Assert.NotEqual(prevId, id);
                prevId = id;

                Assert.IsType<JsonObject>(employee);
            }
        }

        /// <summary>
        /// Adding JsonArray to JsonObject by creating it in initializing collection
        /// </summary>
        [Fact]
        public static void TestAddingJsonArray()
        {
            var preferences = new JsonObject()
                {
                    { "colours", new JsonArray { "red", "green", "purple" } }       
                };

            Assert.IsType<JsonArray>(preferences["colours"]);
            var colours = preferences["colours"] as JsonArray;
            Assert.Equal(3, colours.Count);

            string[] expected = { "red", "green", "blue" };

            for (int i = 0; i < colours.Count; i++)
            {
                Assert.IsType<JsonString>(colours[i]);
                Assert.Equal(expected[i], colours[i] as JsonString);
            }
        }

        /// <summary>
        /// Adding JsonArray to JsonObject by creating it from string array
        /// </summary>
        [Fact]
        public static void TestCretingJsonArrayFromStringArray()
        {
            string[] expected = { "sushi", "pasta", "cucumber soup" };
            var preferences = new JsonObject()
                {
                    { "dishes", new JsonArray(expected) }
                };

            Assert.IsType<JsonArray>(preferences["dishes"]);
            var dishesJson = preferences["dishes"] as JsonArray;
            Assert.Equal(3, dishesJson.Count);

            for (int i = 0; i < dishesJson.Count; i++)
            {
                Assert.IsType<JsonString>(dishesJson[i]);
                Assert.Equal(expected[i], dishesJson[i] as JsonString);
            }
        }

        /// <summary>
        /// Adding JsonArray to JsonObject by passing JsonNumber array
        /// </summary>
        [Fact]
        public static void TestAddingJsonArrayFromJsonNumberArray()
        {
            var preferences = new JsonObject()
                {
                     { "prime numbers", new JsonNumber[] { 19, 37 } }
                };

            Assert.IsType<JsonArray>(preferences["prime numbers"]);
            var primeNumbers = preferences["prime numbers"] as JsonArray;
            Assert.Equal(2, primeNumbers.Count);

            int[] expected = { 19, 37 };

            for (int i = 0; i < primeNumbers.Count; i++)
            {
                Assert.IsType<JsonNumber>(primeNumbers[i]);
                Assert.Equal(expected[i], primeNumbers[i] as JsonNumber);
            }
        }

        /// <summary>
        /// Adding JsonArray to JsonObject by passing IEnumerable of strings
        /// </summary>
        [Fact]
        public static void TestAddingJsonArrayFromIEnumerableOfStrings()
        {
            var sportsExperienceYears = new JsonObject()
            {
                { "skiing", 5 },
                { "cycling", 8 },
                { "hiking", 6 },
                { "chess", 2 },
                { "skating", 1 },
            };

            // choose only sports with > 2 experience years
            IEnumerable<string> sports = sportsExperienceYears.Where(sport => ((JsonNumber)sport.Value).GetInt32() > 2).Select(sport => sport.Key);

            var preferences = new JsonObject()
                {
                     { "sports", new JsonArray(sports) }
                };

            Assert.IsType<JsonArray>(preferences["sports"]);
            var sportsJsonArray = preferences["sports"] as JsonArray;
            Assert.Equal(3, sportsJsonArray.Count);

            for (int i = 0; i < sportsJsonArray.Count; i++)
            {
                Assert.IsType<JsonString>(sportsJsonArray[i]);
                Assert.Equal(sports.ElementAt(i), sportsJsonArray[i] as JsonString);
            }
        }

        /// <summary>
        /// Adding JsonArray to JsonObject by passing IEnumerable of JsonNodes
        /// </summary>
        [Fact]
        public static void TestAddingJsonArrayFromIEnumerableOfJsonNodes()
        {
            var strangeWords = new JsonArray()
            {
                "supercalifragilisticexpialidocious",
                "gladiolus",
                "albumen",
                "smaragdine"
            };

            var preferences = new JsonObject()
                {
                     { "strange words", strangeWords.Where(word => ((JsonString)word).Value.Length < 10) }
                };

            Assert.IsType<JsonArray>(preferences["strange words"]);
            var strangeWordsJsonArray = preferences["strange words"] as JsonArray;
            Assert.Equal(2, strangeWordsJsonArray.Count);

            string [] expected = { "gladiolus", "albumen" };

            for (int i = 0; i < strangeWordsJsonArray.Count; i++)
            {
                Assert.IsType<JsonString>(strangeWordsJsonArray[i]);
                Assert.Equal(expected[i], strangeWordsJsonArray[i] as JsonString);
            }
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

            Assert.IsType<JsonArray>(vertices[0]);
            var innerJsonArray = vertices[0] as JsonArray;
            Assert.IsType<JsonArray>(innerJsonArray[0]);
            innerJsonArray = innerJsonArray[0] as JsonArray;
            Assert.IsType<JsonArray>(innerJsonArray[0]);
        }

        /// <summary>
        /// Adding values to JsonArray
        /// </summary>
        [Fact]
        public static void TestAddingToJsonArray()
        {
            var employeesIds = new JsonArray();

            foreach (KeyValuePair<string, JsonNode> employee in EmployeesDatabase.GetTenBestEmployees())
            {
                employeesIds.Add(employee.Key);
            }

            JsonString prevId = new JsonString();
            foreach(JsonNode employeeId in employeesIds)
            {
                Assert.IsType<JsonString>(employeeId);
                var employeeIdString = employeeId as JsonString;
                Assert.NotEqual(prevId, employeeIdString);
                prevId = employeeIdString;
            }
        }

        /// <summary>
        /// Creating Json array from collection
        /// </summary>
        [Fact]
        public static void TestCreatingJsonArrayFromCollection()
        {
            var employeesIds = new JsonArray(EmployeesDatabase.GetTenBestEmployees().Select(employee => new JsonString(employee.Key)));

            JsonString prevId = new JsonString();
            foreach (JsonNode employeeId in employeesIds)
            {
                Assert.IsType<JsonString>(employeeId);
                var employeeIdString = employeeId as JsonString;
                Assert.NotEqual(prevId, employeeIdString);
                prevId = employeeIdString;
            }
        }

        /// <summary>
        /// Creating Json array from collection of strings
        /// </summary>
        [Fact]
        public static void TestCreatingJsonArrayFromCollectionOfString()
        {
            var employeesIds = new JsonArray(EmployeesDatabase.GetTenBestEmployees().Select(employee => employee.Key));

            JsonString prevId = new JsonString();
            foreach (JsonNode employeeId in employeesIds)
            {
                Assert.IsType<JsonString>(employeeId);
                var employeeIdString = employeeId as JsonString;
                Assert.NotEqual(prevId, employeeIdString);
                prevId = employeeIdString;
            }
        }

        /// <summary>
        /// Contains checks
        /// </summary>
        [Fact]
        public static void TestContains()
        {
            var person = new JsonObject
            {
                { "name", "John" },
                { "ssn", "123456789" },
            };

            if (person.ContainsProperty("ssn"))
            {
                EmployeesDatabase.CheckSSN(((JsonString)person["ssn"]).Value);
            }

            Assert.True(person.ContainsProperty("ssn"));
            Assert.False(person.ContainsProperty("surname"));

            // Different scenario:

            var enabledOptions = new JsonArray
            {
                "readonly",
                "no cache",
                "continue on failure"
            };

            if (enabledOptions.Contains("no cache"))
            {
                // do sth without using caching
            }

            var requiredOptions = new JsonArray
            {
                "readonly",
                "continue on failure"
            };

            // if all required options are enabled
            if (!requiredOptions.Select(option => !enabledOptions.Contains(option)).Any())
            {
                // do sth without using caching
            }
        }

        /// <summary>
        /// Replacing Json object's primnary types
        /// </summary>
        [Fact]
        public static void TestReplacingsonObjectPrimaryTypes()
        {
            var person1 = new JsonObject
            {
                { "name", "John" },
                { "age", 45 },
                { "is_married", true }
            };

            // Assign by creating a new instance of primary Json type
            person1["name"] = new JsonString("Bob");

            Assert.IsType<JsonString>(person1["name"]);
            Assert.Equal(person1["name"] as JsonString, "Bob");

            // Assign by using an implicit operator on primary Json type
            JsonNumber newAge = 55;
            person1["age"] = newAge;

            Assert.IsType<JsonNumber>(person1["age"]);
            Assert.Equal(person1["age"] as JsonNumber, 55);

            // Assign by explicit cast from Json primary type
            person1["is_married"] = (JsonBoolean)false;

            Assert.IsType<JsonBoolean>(person1["is_married"]);
            Assert.Equal(person1["is_married"] as JsonBoolean, false);

            // Not possible scenario (wold require implicit cast operators in JsonNode):
            // person["name"] = "Bob";

            var person2 = new JsonObject
            {
                { "name", new JsonString[]{ "Emily", "Rosalie" } },
                { "age", 33 },
                { "is_married", true }
            };

            // Copy property from another JsonObject
            person1["age"] = person2["age"];

            Assert.IsType<JsonNumber>(person1["age"]);
            Assert.Equal(person1["age"] as JsonNumber, 33);

            // Copy property of different typoe
            person1["name"] = person2["name"];

            Assert.IsType<JsonArray>(person1["name"]);
        }

        /// <summary>
        /// Modifying Json object's primnary types
        /// </summary>
        [Fact]
        public static void TestModifyingJsonObjectPrimaryTypes()
        {
            JsonString name = "previous name";
            name.Value = "new name";

            Assert.Equal("new name", name.Value);

            bool shouldBeEnabled = true;
            var isEnabled = new JsonBoolean(false);
            isEnabled.Value = shouldBeEnabled;

            Assert.True(isEnabled.Value);

            JsonNumber veryBigConstant = new JsonNumber();
            veryBigConstant.SetString("1e1000");
            string bigNumber = veryBigConstant.GetString();

            Assert.Equal("1e1000", bigNumber);

            veryBigConstant.SetInt16(123);
            short smallNumber = veryBigConstant.GetInt16();

            Assert.Equal(123, smallNumber);

            // Incrementing age:
            JsonObject employee = EmployeesDatabase.GetManager();
            int age = ((JsonNumber)employee["age"]).GetInt32();
            ((JsonNumber)employee["age"]).SetInt32(age + 1);

            Assert.Equal(46, ((JsonNumber)employee["age"]).GetInt32());
        }

        /// <summary>
        /// Accesing nested Json object - casting with as operator
        /// </summary>
        [Fact]
        public static void TestAccesingNestedJsonObjectCastWithAs()
        {
            // Casting with as operator
            JsonObject manager = EmployeesDatabase.GetManager();

            var reportingEmployees = manager["reporting employees"] as JsonObject;
            if (reportingEmployees == null)
                throw new InvalidCastException();

            var softwareDevelopers = reportingEmployees["software developers"] as JsonObject;
            if (softwareDevelopers == null)
                throw new InvalidCastException();

            var internDevelopers = softwareDevelopers["intern employees"] as JsonObject;
            if (internDevelopers == null)
                throw new InvalidCastException();

            internDevelopers.Add(EmployeesDatabase.GetNextEmployee());
        }

        /// <summary>
        /// Accesing nested Json object - casting with is operator
        /// </summary>
        [Fact]
        public static void TestAccesingNestedJsonObjectCastWithIs()
        {
            JsonObject manager = EmployeesDatabase.GetManager();

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
        public static void TestAccesingNestedJsonObjectExplicitCast()
        {
            JsonObject manager = EmployeesDatabase.GetManager();

            ((JsonObject)((JsonObject)manager["reporting employees"])["HR"]).Add(EmployeesDatabase.GetNextEmployee());
        }

        /// <summary>
        /// Accesing nested Json object - GetObjectProperty method
        /// </summary>
        [Fact]
        public static void TestAccesingNestedJsonObjectGetPropertyMethod()
        {
            JsonObject manager = EmployeesDatabase.GetManager();
            JsonObject internDevelopers = manager.GetObjectProperty("reporting employees")
                                          .GetObjectProperty("software developers")
                                          .GetObjectProperty("intern employees");
            internDevelopers.Add(EmployeesDatabase.GetNextEmployee());
        }

        /// <summary>
        /// Accesing nested Json object - TryGetObjectProperty method
        /// </summary>
        [Fact]
        public static void TestAccesingNestedJsonObjectTryGetPropertyMethod()
        {
            JsonObject manager = EmployeesDatabase.GetManager();
            if (manager.TryGetObjectProperty("reporting employees", out JsonObject reportingEmployees))
            {
                if (reportingEmployees.TryGetObjectProperty("software developers", out JsonObject softwareDevelopers))
                {
                    if (softwareDevelopers.TryGetObjectProperty("full time employees", out JsonObject fullTimeEmployees))
                    {
                        fullTimeEmployees.Add(EmployeesDatabase.GetNextEmployee());
                    }
                }
            }
        }

        /// <summary>
        /// Accesing nested Json array - GetArrayProperty method
        /// </summary>
        [Fact]
        public static void TestAccesingNestedJsonArrayGetPropertyMethod()
        {
            var issues = new JsonObject()
            {
                { "features", new JsonArray{ "new functionality 1", "new functionality 2" } },
                { "bugs", new JsonArray{ "bug 123", "bug 4566", "bug 821" } },
                { "tests", new JsonArray{ "code coverage" } },
            };

            issues.GetArrayProperty("bugs").Add("bug 12356");
            ((JsonString)issues.GetArrayProperty("features")[0]).Value = "feature 1569";
            ((JsonString)issues.GetArrayProperty("features")[1]).Value = "feature 56134";
        }

        /// <summary>
        /// Modifying Json object key - remove and add
        /// </summary>
        [Fact]
        public static void TestModifyingJsonObjectKeyRemoveAdd()
        {
            JsonObject manager = EmployeesDatabase.GetManager();
            JsonObject reportingEmployees = manager.GetObjectProperty("reporting employees");

            JsonNode softwareDevelopers = reportingEmployees["software developers"];
            reportingEmployees.Remove("software developers");
            reportingEmployees.Add("software engineers", softwareDevelopers);
        }

        /// <summary>
        /// Modifying Json object key - modify method
        /// </summary>
        [Fact]
        public static void TestModifyingJsonObjectKeyModifyMethod()
        {
            JsonObject manager = EmployeesDatabase.GetManager();
            JsonObject reportingEmployees = manager.GetObjectProperty("reporting employees");

            reportingEmployees.ModifyPropertyName("software developers", "software engineers");
        }

        /// <summary>
        /// Aquiring all values
        /// </summary>
        [Fact]
        public static void TestAquiringAllValues()
        {
            var employees = new JsonObject(EmployeesDatabase.GetTenBestEmployees());
            ICollection<JsonNode> employeesWithoutId = employees.Values;
        }

        /// <summary>
        /// Aquiring all properties
        /// </summary>
        [Fact]
        public static void TestAquiringAllProperties()
        {
            var employees = new JsonObject()
            {
                { "FTE", "John Smith" },
                { "FTE", "Ann Predictable" },
                { "Intern", "Zoe Coder" },
                { "FTE", "Byron Shadow" },
            };

            IEnumerable<JsonNode> fullTimeEmployees = employees.GetAllProperties("FTE");
        }

        /// <summary>
        /// Transforming JsoneNode to JsonElement
        /// </summary>
        [Fact]
        public static void TestTransformingJsonNodeToJsonElement()
        {
            // Send Json through network
            var employeeDataToSend = EmployeesDatabase.GetNextEmployee().Value;
            Mailbox.SendEmployeeData(employeeDataToSend.AsJsonElement());
        }

        /// <summary>
        /// Transforming JsonElement to JsonNode
        /// </summary>
        [Fact]
        public static void TestTransformingJsonElementToJsonNode()
        {
            var receivedEmployeeData = JsonElement.DeepCopy(Mailbox.RetrieveEmployeeData());
            if (receivedEmployeeData is JsonObject employee)
            {
                employee["name"] = new JsonString("Bob");
                Mailbox.SendEmployeeData(employee.AsJsonElement());
            }
        }

        /// <summary>
        /// Transforming JsonDocument to JsonNode and vice versa
        /// </summary>
        [Fact]
        public static void TestTransformingToFromJsonDocument()
        {
            string jsonString = @"
            {
                ""employee1"" : 
                {
                    ""name"" : ""Ann"",
                    ""surname"" : ""Predictable"",
                    ""age"" : 30,                
                },
                ""employee2"" : 
                {
                    ""name"" : ""Zoe"",
                    ""surname"" : ""Coder"",
                    ""age"" : 24,                
                }
            }";

            using (JsonDocument employeesToSend = JsonDocument.Parse(jsonString))
            {
                // regular send:
                Mailbox.SendAllEmployeesData(employeesToSend.RootElement);

                // modified elements send:
                JsonNode modifiableDocument = JsonElement.DeepCopy(employeesToSend);
                var employees = modifiableDocument as JsonObject;
                employees.Add(EmployeesDatabase.GetNextEmployee());

                Mailbox.SendAllEmployeesData(employees.AsJsonElement());
            }
        }

        /// <summary>
        /// Parsing right to JsonNode if user knows data will be modified
        /// </summary>
        [Fact]
        public static void TestParsingToJsonNode()
        {
            string jsonString = @"
            {
                ""employee1"" : 
                {
                    ""name"" : ""Ann"",
                    ""surname"" : ""Predictable"",
                    ""age"" : 30,                
                },
                ""employee2"" : 
                {
                    ""name"" : ""Zoe"",
                    ""surname"" : ""Coder"",
                    ""age"" : 24,                
                }
            }";

            JsonObject employees = JsonNode.Parse(jsonString) as JsonObject;
            employees.Add(EmployeesDatabase.GetNextEmployee());
            Mailbox.SendAllEmployeesData(employees.AsJsonElement());
        }

        /// <summary>
        /// Copying JsoneNode
        /// </summary>
        [Fact]
        public static void TestCopyingJsonNode()
        {
            JsonObject employee = EmployeesDatabase.GetManager();
            JsonNode employeeCopy = JsonNode.DeepCopy(employee);
        }

        /// <summary>
        /// Checking IsImmutable property
        /// </summary>
        [Fact]
        public static void TestIsImmutable()
        {
            JsonElement employee = Mailbox.RetrieveEmployeeData();
            if(employee.IsImmutable)
            {
                JsonObject employeeNode = JsonNode.GetNode(employee) as JsonObject;
                employeeNode["received as node"] = (JsonBoolean) true;
            }
        }

        /// <summary>
        /// Retrieving JsonNode from JsonElement
        /// </summary>
        [Fact]
        public static void TestTryGetNode()
        {
            JsonElement employee = Mailbox.RetrieveEmployeeData();
            if (JsonNode.TryGetNode(employee, out JsonNode employeeNode))
            {
                ((JsonObject)employeeNode)["received as node"] = (JsonBoolean) true;
            }
        }

        /// <summary>
        /// Sorting string JsonArray
        /// </summary>
        [Fact]
        public static void TestSortingJsonArray()
        {
            var employees = new JsonArray
            {
                "Smith John",
                "Predictable Ann",
                "Coder Zoe",
                "Shadow Byron"
            };

            employees.Sort();
            
            // Create medical appointments for employees in alphabetical order
            foreach (JsonNode employee in employees)
            {
                if (employee is JsonString person)
                {
                    HealthCare.CreateMedicalAppointment(person.Value);
                }
            }
        }

        /// <summary>
        /// Sorting multi-typed JsonArray
        /// </summary>
        [Fact]
        public static void TestSortingMultiTypedJsonArray()
        {
            var studentGrades = new JsonArray
            {
                3,
                5,
                4,
                "not passed",
                5,
                3.5,
                "passed with no grade",
                "passed with no grade"
            };

            studentGrades.Sort();

            double ComputeMedian(JsonArray grades)
            {
                if ((grades?.Count ?? 0) == 0)
                {
                    return double.NaN;
                }

                grades.Sort();

                int lastIdx = 0;
                while (grades[lastIdx] is JsonNumber)
                {
                    lastIdx++;
                }

                if (lastIdx == 0)
                {
                    return double.NaN;
                }

                double median;

                if (lastIdx % 2 == 0)
                {
                    median = ((JsonNumber)grades[lastIdx / 2]).GetDouble();
                }
                else
                {
                    double left = ((JsonNumber)grades[lastIdx / 2]).GetDouble();
                    double right = ((JsonNumber)grades[lastIdx / 2 + 1]).GetDouble();
                    median = (left + right) / 2.0;
                }

                return median;
            }

            Assert.Equal(4, ComputeMedian(studentGrades));
            // ComputeMedian sorted the array, the side effect has leaked out:
            Assert.Equal(3.5, ((JsonNumber)studentGrades[1]).GetDouble());
        }
    }
}
