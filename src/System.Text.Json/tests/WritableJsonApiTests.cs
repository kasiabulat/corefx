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

        private static class StudentsDatabase
        {
            private static int Id = 0;

            /// <summary>
            /// Returns JsonObject cointaining two inner JsonObjects: "personal data" (first and last name) 
            /// and "grades" (grades from subjects grouped into fields of science).
            /// All students have algebra grades, 1/3 of them have C# grades, 1/3 not passed, 1/3 didn't take the subject.
            /// </summary>
            /// <returns></returns>
            public static KeyValuePair<string, JsonNode> GetNextStudent()
            {
                var student = new JsonObject();

                var personalData = new JsonObject();
                personalData.Add("first namne", "John");
                personalData.Add("last namne", "Smith");

                var grades = new JsonObject();
                var random = new Random();

                var mathsGrades = new JsonObject();
                mathsGrades.Add("algebra", random.Next(2, 5));
                mathsGrades.Add("geometry", random.Next(2, 5));
                mathsGrades.Add("analysis", random.Next(2, 5));

                var computerScienceGrades = new JsonObject();
                computerScienceGrades.Add("databases", random.Next(2, 5));

                switch (Id % 3)
                {
                    case 0:
                        computerScienceGrades.Add("C#", random.Next(2, 5));
                        break;
                    case 1:
                        computerScienceGrades.Add("C#", "not passed");
                        break;
                    case 2:
                        break;
                }

                grades.Add("math", mathsGrades);
                grades.Add("computer science", computerScienceGrades);

                student.Add("personal data", personalData);
                student.Add("grades", grades);

                return new KeyValuePair<string, JsonNode>("id" + Id++, student);
            }

            public static IEnumerable<KeyValuePair<string, JsonNode>> GetTenBestStudents()
            {
                for (int i = 0; i < 10; i++)
                    yield return GetNextStudent();
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
            string[] dishes = { "sushi", "pasta", "cucumber soup" };

            var sportsExperienceYears = new JsonObject()
            {
                { "skiing", 5 },
                { "cycling", 8 },
                { "hiking", 6 },
                { "chess", 2 },
                { "skating", 1 },
            };

            var sports = sportsExperienceYears.Where(sport => ((JsonNumber)sport.Value).GetInt32() > 2).Select(sport => sport.Key);

            var strangeWords = new JsonArray()
            {
                "supercalifragilisticexpialidocious",
                "gladiolus",
                "albumen",
                "smaragdine"
            };

            var preferences = new JsonObject()
            {
                { "colours", new JsonArray { "red", "green", "purple" } },
                { "numbers", new JsonArray { 4, 123, 88 } },
                { "prime numbers", new JsonNumber[] { 19, 37 } },
                { "varia", new JsonArray { 17, "green", true } },
                { "dishes", new JsonArray(dishes) },
                { "sports", new JsonArray(sports) },
                { "strange words", strangeWords.Where(word => ((JsonString)word).GetString().Length < 10) },
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
        public static void TestContainsChecks()
        {
            var person = new JsonObject
            {
                { "name", "John" },
                { "ssn", "123456789" },
            };

            if (person.ContainsProperty("ssn"))
            {
                EmployeesDatabase.CheckSSN(((JsonString)person["ssn"]).GetString());
            }

            var enabledOptions = new JsonArray
            {
                "readonly",
                "no cache",
                "continue on failure"
            };

            if(enabledOptions.Contains("no cache"))
            {
                // do sth without using caching
            }

            var requiredOptions = new JsonArray
            {
                "readonly",
                "continue on failure"
            };

            // if all required options are enabled
            if(!requiredOptions.Select(option => !enabledOptions.Contains(option)).Any())
            {
                // do sth without using caching
            }
        }

        /// <summary>
        /// Modifying Json object's primnary types
        /// </summary>
        [Fact]
        public static void TestModifyingJsonObjectPrimaryTypes()
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
        public static void TestAccesingNestedJsonObjectCastWithAs()
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
        public static void TestAccesingNestedJsonObjectCastWithIs()
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
        public static void TestAccesingNestedJsonObjectExplicitCast()
        {
            var manager = EmployeesDatabase.GetManager();

            ((JsonObject)((JsonObject)manager["reporting employees"])["HR"]).Add(EmployeesDatabase.GetNextEmployee());
        }

        /// <summary>
        /// Accesing nested Json object - GetNestedProperty method
        /// </summary>
        [Fact]
        public static void TestAccesingNestedJsonObjectGetNestedPropertyMethod()
        {
            var manager = EmployeesDatabase.GetManager();
            var internDevelopers = manager.GetNestedProperty("reporting employees")
                                          .GetNestedProperty("software developers")
                                          .GetNestedProperty("intern employees");

            internDevelopers.Add(EmployeesDatabase.GetNextEmployee());
        }

        /// <summary>
        /// Modifying Json object key - remove & add
        /// </summary>
        [Fact]
        public static void TestModifyingJsonObjectKeyRemoveAdd()
        {
            var manager = EmployeesDatabase.GetManager();
            var reportingEmployees = manager.GetNestedProperty("reporting employees");

            var softwareDevelopers = reportingEmployees["software developers"];
            reportingEmployees.Remove("software developers");
            reportingEmployees.Add("software engineers", softwareDevelopers);
        }

        /// <summary>
        /// Modifying Json object key - modify method
        /// </summary>
        [Fact]
        public static void TestModifyingJsonObjectKeyModifyMethod()
        {
            var manager = EmployeesDatabase.GetManager();
            var reportingEmployees = manager.GetNestedProperty("reporting employees");

            reportingEmployees.ModifyPropertyName("software developers", "software engineers");
        }

        /// <summary>
        /// Aquiring all values of properties with the same name
        /// </summary>
        [Fact]
        public static void TestAquiringAllPropertiesValusWithSpecificName()
        {
            var students = new JsonObject(StudentsDatabase.GetTenBestStudents());
            
            // Calculating avarage with foreach loop
            var algebraGrades = students.GetAllValuesByPropertyName("algebra");

            long gradesSum = 0;
            foreach(var grade in algebraGrades)
            {
                if (grade is JsonNumber gradeAsNumber)
                    gradesSum += gradeAsNumber.GetInt32();
            }
            if (algebraGrades.Count() != 0)
            {
                var avarage = gradesSum / algebraGrades.Count();
            }

            // Calculating avarage with aggregate
            var csharpGrades = students.GetAllValuesByPropertyName("C#");
            var csharpGradesAvarage = (csharpGrades.Count() == 0) ? 0 : 
                                        (csharpGrades.Aggregate(0, (sum, grade) => 
                                            grade is JsonNumber gradeAsNumber ? sum + gradeAsNumber.GetInt32() : sum)
                                        / csharpGrades.Count());
        }
    }
}
