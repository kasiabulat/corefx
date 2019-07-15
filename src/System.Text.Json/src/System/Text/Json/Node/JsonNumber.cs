// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.System.Text
{
    public partial class JsonNumber : JsonNode
    {
        public JsonNumber() { }
        public JsonNumber(bool value) { }
        public JsonNumber(string value) { }
        public JsonNumber(byte value) { }
        public JsonNumber(short value) { }
        public JsonNumber(int value) { }
        public JsonNumber(long value) { }
        public JsonNumber(float value) { }
        public JsonNumber(double value) { }
        [CLSCompliant(false)]
        public JsonNumber(sbyte value) { }
        [CLSCompliant(false)]
        public JsonNumber(ushort value) { }
        [CLSCompliant(false)]
        public JsonNumber(uint value) { }
        [CLSCompliant(false)]
        public JsonNumber(ulong value) { }

        public byte GetByte() { throw null; }
        public int GetInt32() { throw null; }
        public short GetInt16() { throw null; }
        public long GetInt64() { throw null; }
        public float GetSingle() { throw null; }
        public double GetDouble() { throw null; }
        [CLSCompliant(false)]
        public sbyte GetSByte() { throw null; }
        [CLSCompliant(false)]
        public ushort GetUInt16() { throw null; }
        [CLSCompliant(false)]
        public uint GetUInt32() { throw null; }
        [CLSCompliant(false)]
        public ulong GetUInt64() { throw null; }

        public bool TryGetByte(out byte value) { throw null; }
        public bool TryGetInt32(out int value) { throw null; }
        public bool TryGetInt16(out short value) { throw null; }
        public bool TryGetInt64(out long value) { throw null; }
        public bool TryGetSingle(out float value) { throw null; }
        public bool TryGetDouble(out double value) { throw null; }
        [CLSCompliant(false)]
        public bool TryGetSByte(out sbyte value) { throw null; }
        [CLSCompliant(false)]
        public bool TryGetUInt16(out ushort value) { throw null; }
        [CLSCompliant(false)]
        public bool TryGetUInt32(out uint value) { throw null; }
        [CLSCompliant(false)]
        public bool TryGetUInt64(out ulong value) { throw null; }

        public static implicit operator JsonNumber(int value) { return new JsonNumber(value); }
    }
}
