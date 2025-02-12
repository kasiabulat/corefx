# Writable JSON Document Object Model (DOM) for `System.Text.Json`

## Introduction

`JsonNode` is a modifiable, dictionary-backed API to complement the readonly `JsonDocument`.

It is the base class for the following concrete types representing all possible kinds of JSON nodes:
* `JsonString` - representing JSON text value
* `JsonBoolean` - representing JSON boolean value (`true` or `false`)
* `JsonNumber` - representing JSON numeric value, can be created from and converted to all possible built-in numeric types
* `JsonArray` - representing the array of JSON nodes
* `JsonObject` - representing the set of properties - named JSON nodes

It is a summer internship project being developed by @kasiabulat.

## Goals

The user should be able to:
* Build up a structured in-memory representation of the JSON payload. 
* Query the document object model.
* Modify it. That includes, remove, add, and update. This means we want to build a modifiable JsonDocument that is not just readonly.

## TODOs

* Designing API
* Implementation of provided methods
* Tests of provided methods
* Documentation for public API

## Example scenarios
### Collection initialization

One of the aims in designing this API was the take an advantage of C# language features and make it easy and natural for delevopers to create instances of `JsonObject`s without calling too many `new` instructions. Below example shows how to initialize JSON object with different types of properties:

```csharp
var developer = new JsonObject
{
    { "name", "Kasia" },
    { "age", 22 },
    { "is developer", true },
    { "null property", (JsonNode) null }
};
```

JSON object can be nested within other JSON object or include a JSON array: 

```csharp
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

```csharp
string[] dishes = { "sushi", "pasta", "cucumber soup" };
IEnumerable<string> sports = sportsExperienceYears.Where(sport => ((JsonNumber)sport.Value).GetInt32() > 2).Select(sport => sport.Key);

var preferences = new JsonObject()
{
    { "colours", new JsonArray { "red", "green", "purple" } },
    { "numbers", new JsonArray { 4, 123, 88 } },
    { "prime numbers", new JsonNumber[] { 19, 37 } },
    { "dishes", new JsonArray(dishes) },
    { "sports", new JsonArray(sports) },
    { "strange words", strangeWords.Where(word => ((JsonString)word).Value.Length < 10) },
};
```

### Modifying existing instance

The main goal of the new API is to allow users to modify existing instance of `JsonNode` which is not possible with `JsonElement` and `JsonDocument`. 

One may change the existing property to have a different value:
```csharp
 var options = new JsonObject { { "use caching", true } };
 options["use caching"] = (JsonBoolean)false;
```

Add a value to existing JSON array or property to existing JSON object:
```csharp
var bestEmployees = new JsonObject(EmployeesDatabase.GetTenBestEmployees());
bestEmployees.Add(EmployeesDatabase.GetManager());
```

Or modify the exisitng property name:
```csharp
JsonObject manager = EmployeesDatabase.GetManager();
JsonObject reportingEmployees = manager.GetJsonObjectProperty("reporting employees");
reportingEmployees.ModifyPropertyName("software developers", "software engineers");
```

### Transforming to and from JsonElement

The API allows users to get a writable version of JSON document from a readonly one and vice versa:

Transforming JsonNode to JsonElement:
```csharp
JsonNode employeeDataToSend = EmployeesDatabase.GetNextEmployee().Value;
Mailbox.SendEmployeeData(employeeDataToSend.AsJsonElement());
```

Transforming JsonElement to JsonNode:
```csharp
JsonNode receivedEmployeeData = JsonNode.DeepCopy(Mailbox.RetrieveMutableEmployeeData());
if (receivedEmployeeData is JsonObject employee)
{
    employee["name"] = new JsonString("Bob");
}
```

### Parsing to JsonNode

If a developer knows they will be modifying an instance, there is an API to parse string right to `JsonNode`, without `JsonDocument` being an intermediary.

```csharp
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

## Design choices

* Avoid any significant perf regression to the readonly implementation of `JsonDocument` and `JsonElement`.
* Higher emphasis on usability over allocations/performance.
* No advanced methods for looking up properties like `GetAllValuesByPropertyName` or `GetAllPrimaryTypedValues`, because they would be too specialized.
* Support for LINQ style quering capability.
* `null` reference to node instead of `JsonNull` class.

* Initializing JsonArray with additional constructors accepting `IEnumerable`s of all primary types (bool, string, int, double,  long...).

    Considered solutions:

    1. One additional constructor in JsonArray
    ```csharp
    public JsonArray(IEnumerable<object> jsonValues) { }
    ``` 
    2. Implicit operator from Array in JsonArray

    3. More additional constructors in JsonArray (chosen)
    ```csharp
    public JsonArray(IEnumerable<string> jsonValues) { }
    public JsonArray(IEnumerable<bool> jsonValues) { }
    public JsonArray(IEnumerable<sbyte> jsonValues) { }
    ...
    public JsonArray(IEnumerable<double> jsonValues) { }
    ``` 

    | Solution | Pros | Cons | Comment |
    |----------|:-------------|:------|--------:|
    | 1 | - only one additional method <br> - accepts collection of different types <br> - accepts `IEnumerable` <br> - IntelliSense (autocompletion and showing suggestions) | - accepts collection of types not deriving from `JsonNode` <br> - needs to check it in runtime  | accepts too much, <br> array of different primary types wouldn't be returned from method |
    | 2 | - only one additional method <br> - accepts collection of different types <br > | - works  only in C# <br> - no IntelliSense <br> - users may not be aware of it <br> - accepts only `Array` <br> - accepts collection of types not deriving from `JsonNode` <br> - needs to check it in runtime | from {1,2}, <br>2 seems worse |
    | 3 | - accepts IEnumerable <br> - does not accept collection of  types not deriving from `JsonNode` <br> - no checks in runtime <br> - IntelliSense | - a lot of additional methods <br> - does not accept a collection of different types | gives less possibilities than {1,2}, but requiers no additional checks |

* Implicit operators for `JsonString`, `JsonBoolean` and `JsonNumber` as an additional feature.
* `Sort` not implemented for `JsonArray`, beacuse there is no right way to compare `JsonObject`s. If user wants to sort `JsonArray` of `JsonNumber`s, `JsonBooleans`s or `JsonStrings` he/she now needs to do the following: convert `JsonArray` to regular array (by iterating through all elements), calling sort (and converting back to `JsonArray` if needed).
* No support for duplicates of property names. Possibly, adding an option for user to choose from: "first value", "last value", or throw-on-duplicate.
* Transformation API:
    * `DeepCopy` method in JsonElement allowing to change JsonElement and JsonDocument into JsonNode recursively transforming all of the elements
    * `AsJsonElement` method in JsonNode allowing to change JsonNode into JsonElement with IsImmutable property set to false
    * `IsImmutable` property informing if JsonElement is keeping JsonDocument or JsonNode underneath
    * `Parse(string)` in JsonNode to be able to parse Json string right into JsonNode if user knows he/she wants mutable version
    * `DeepCopy` in JsonNode to make a copy of the whole tree
    * `GetNode` and TryGetNode in JsonNode allowing to retrieve it from JsonElement

## Implementation details
* `JsonNumber` value is stored as a `string`.

## Open questions
API:
* Do we want to add recursive equals on `JsonArray` and `JsonObject`?
* Do we want to make `JsonNode`s derived types (and which) implement `IComparable`?
* Would escaped characters be supported for creating `JsonNumber` from string? 
* Is the API for `JsonNode` and `JsonElement` interactions sufficient? 
* Do we want to support duplicate and order preservation/control when adding/removing values in `JsonArray`/`JsonObject`?
* Do we want to have accelerator APIs for writing a JsonNode to a Utf8JsonWriter without having to go through JsonElement? (currently added as a `Parse` method in JsonNode)
* Should JsonNode have a JsonValueKind property that a caller can inspect and cast to the right concrete type?
* Let's say someone else passes a JsonNode to me, what can I do with it?
* Should nodes track their own position in the JSON graph? Do we want to allow properties like Parent, Next and Previous?

    | Solution | Pros | Cons |
    |----------|:-------------|--------|
    |current API| - no additional checks need to be made | - creating recursive loop by the user may be problematic |
    |tracking nodes | - handles recursive loop problem | - when node is added to a parent, it needs to be checked <br>  if it already has a parent  and make a copy if it has |

Implementation:
* Do we want to add a copy of `JsonWriterHelper.ValidateNumber` with additional checks?
* Do we want to change `JsonNumber`'s backing field to something different than `string`?     
    Suggestions: `Span<byte>` or array of `Utf8String`/`Char8` (once they come online in the future) / `byte`  

## Useful links

### JSON
* grammar: https://www.json.org/
* specification: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-404.pdf
* RFC: https://tools.ietf.org/html/rfc8259

### Similar APIs
`JsonElement` and `JsonDocument` from `System.Json.Text` API:
* video: https://channel9.msdn.com/Shows/On-NET/Try-the-new-SystemTextJson-APIs
* blogpost: https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/
* spans: https://msdn.microsoft.com/en-us/magazine/mt814808.aspx

`Json.NET` and its advantages:
* XPath: https://goessner.net/articles/JsonPath/
* LINQ: https://www.newtonsoft.com/json/help/html/LINQtoJSON.htm
* XML: https://www.newtonsoft.com/json/help/html/ConvertJsonToXml.htm
* JToken: https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JToken.htm
