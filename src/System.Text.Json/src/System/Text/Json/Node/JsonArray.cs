// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.System.Text
{
    public partial class JsonArray : JsonNode, IList<JsonNode>
    {
        public JsonArray() { }
        public JsonArray(IEnumerable<JsonNode> jsonValues) { }
        public JsonArray(IEnumerable<string> jsonValues) { }
        public JsonArray(IEnumerable<bool> jsonValues) { }
        public JsonArray(IEnumerable<byte> jsonValues) { }
        public JsonArray(IEnumerable<short> jsonValues) { }
        public JsonArray(IEnumerable<int> jsonValues) { }
        public JsonArray(IEnumerable<long> jsonValues) { }
        public JsonArray(IEnumerable<float> jsonValues) { }
        public JsonArray(IEnumerable<double> jsonValues) { }
        [CLSCompliant(false)]
        public JsonArray(IEnumerable<sbyte> jsonValues) { }
        [CLSCompliant(false)]
        public JsonArray(IEnumerable<ushort> jsonValues) { }
        [CLSCompliant(false)]
        public JsonArray(IEnumerable<uint> jsonValues) { }
        [CLSCompliant(false)]
        public JsonArray(IEnumerable<ulong> jsonValues) { }

        public JsonNode this[int idx] { get => throw null; set => throw null; }
        public IEnumerator<JsonNode> GetEnumerator() { throw null; }
        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public void Add(JsonNode jsonValue) { }
        public void Add(string jsonValue) { }
        public void Add(bool jsonValue) { }
        public void Add(byte jsonValue) { }
        public void Add(short jsonValue) { }
        public void Add(int jsonValue) { }
        public void Add(long jsonValue) { }
        public void Add(float jsonValue) { }
        public void Add(double jsonValue) { }
        [CLSCompliant(false)]
        public void Add(sbyte jsonValue) { }
        [CLSCompliant(false)]
        public void Add(ushort jsonValue) { }
        [CLSCompliant(false)]
        public void Add(uint jsonValue) { }
        [CLSCompliant(false)]
        public void Add(ulong jsonValue) { }

        public void Insert(int index, JsonNode item) { throw null; }
        public void Insert(int index, string item) { throw null; }
        public void Insert(int index, bool item) { throw null; }
        public void Insert(int index, byte item) { throw null; }
        public void Insert(int index, short item) { throw null; }
        public void Insert(int index, int item) { throw null; }
        public void Insert(int index, long item) { throw null; }
        public void Insert(int index, float item) { throw null; }
        public void Insert(int index, double item) { throw null; }
        [CLSCompliant(false)]
        public void Insert(int index, sbyte item) { throw null; }
        [CLSCompliant(false)]
        public void Insert(int index, ushort item) { throw null; }
        [CLSCompliant(false)]
        public void Insert(int index, uint item) { throw null; }
        [CLSCompliant(false)]
        public void Insert(int index, ulong item) { throw null; }

        public bool Contains(JsonNode jsonValue) { throw null; }
        public bool Contains(string jsonValue) { throw null; }
        public bool Contains(bool jsonValue) { throw null; }
        public bool Contains(byte jsonValue) { throw null; }
        public bool Contains(short jsonValue) { throw null; }
        public bool Contains(int jsonValue) { throw null; }
        public bool Contains(long jsonValue) { throw null; }
        public bool Contains(float jsonValue) { throw null; }
        public bool Contains(double jsonValue) { throw null; }
        [CLSCompliant(false)]
        public bool Contains(sbyte jsonValue) { throw null; }
        [CLSCompliant(false)]
        public bool Contains(ushort jsonValue) { throw null; }
        [CLSCompliant(false)]
        public bool Contains(uint jsonValue) { throw null; }
        [CLSCompliant(false)]
        public bool Contains(ulong jsonValue) { throw null; }


        public int Count => throw new NotImplementedException();
        public bool IsReadOnly => throw new NotImplementedException();

        public int IndexOf(JsonNode item) { throw null; }
        public void RemoveAt(int index) { throw null; }
        public void Clear() { throw null; }
        public void CopyTo(JsonNode[] array, int arrayIndex) { throw null; }
        public bool Remove(JsonNode item) { throw null; }
    }
}
