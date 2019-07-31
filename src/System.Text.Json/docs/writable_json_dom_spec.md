# Writable JSON DOM for `System.Text.Json`

## Introduction

`JsonNode` is modifiable, dictionary-backed API to complement the readonly `JsonDocumet`.

It is the base class for the following overloads representing all possible kinds of JSON nodes:
* `JsonString` - representing JSON text value
* `JsonBoolean` - representing JSON boolean value (`true` or `false`)
* `JsonNumber` - representing JSON numeric value, can be created from and converted to all possible numeric primary types
* `JsonArray` - representing the array of JSON nodes
* `JsonObject` - representing the set of properties - named JSON nodes

It is a summer internship project being developed by @kasiabulat.

## Goals

* Designing API
* Implementation of provided methods
* Tests of provided methods
* Documentation for public API

## Example scenarios
### Collection initialization

One of the aims in designing this API was the take an advantage of C# language features and make it easy and natural for delevopers to create instances of `JsonObject`s without calling too many `new` instructions. Below example shows how to initialize JSON object with different types of properties:

```
var developer = new JsonObject
{
    { "name", "Kasia" },
    { "age", 22 },
    { "is developer", true },
    { "null property", (JsonNode) null }
};
```

JSON object can be nested within other JSON object or include a JSON array: 

```
var person = new JsonObject
{
    { "name", "John" },
    { "surname", "Smith" },
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
    },
    {
        "phone numbers", new JsonArray()
        {
            "123-456-7890",
            "123-456-7890" 
        }
    }
};
```

JSON array can be also initialized easily in various ways which might be useful in different secnarios:

```
string[] dishes = { "sushi", "pasta", "cucumber soup" };
IEnumerable<string> sports = sportsExperienceYears.Where(sport => ((JsonNumber)sport.Value).GetInt32() > 2).Select(sport => sport.Key);

var preferences = new JsonObject()
{
    { "colours", new JsonArray { "red", "green", "purple" } },
    { "numbers", new JsonArray { 4, 123, 88 } },
    { "prime numbers", new JsonNumber[] { 19, 37 } },
    { "varia", new JsonArray { 17, "green", true } },
    { "dishes", new JsonArray(dishes) },
    { "sports", new JsonArray(sports) },
    { "strange words", strangeWords.Where(word => ((JsonString)word).Value.Length < 10) },
};
```

### Modifying existing instance

The main goal of the new API is to allow users to modify existing instance of `JsonNode` which is not possible with `JsonElement` and `JsonDocument`. 

One may change the existing property to have a different value:
```
 var options = new JsonObject { { "use caching", true } };
 options["use caching"] = (JsonBoolean)false;
```

Add a value to existing JSON array or property to existing JSON object:
```
var bestEmployees = new JsonObject(EmployeesDatabase.GetTenBestEmployees());
bestEmployees.Add(EmployeesDatabase.GetManager());
```

Or modify the exisitng property name:
```
JsonObject manager = EmployeesDatabase.GetManager();
JsonObject reportingEmployees = manager.GetJsonObjectProperty("reporting employees");
reportingEmployees.ModifyPropertyName("software developers", "software engineers");
```

### Transforming to and from JsonElement

The API allows users to get a writable version of JSON document from readonly one and vice versa:

Transforming JsoneNode to JsonElement:
```
JsonNode employeeDataToSend = EmployeesDatabase.GetNextEmployee().Value;
Mailbox.SendEmployeeData(employeeDataToSend.AsJsonElement());
```

Transforming JsonElement to JsonNode:
```
JsonNode receivedEmployeeData = JsonNode.DeepCopy(Mailbox.RetrieveMutableEmployeeData());
if (receivedEmployeeData is JsonObject employee)
{
    employee["name"] = new JsonString("Bob");
}
```

### Parsing to JsonNode

If developer knows he/she wil be modifying and instance, API provides the possibility to parse string rigth to `JsonNode`, without `JsonDocument` being an intermediary.

```
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
```

