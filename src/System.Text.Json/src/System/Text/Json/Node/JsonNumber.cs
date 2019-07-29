// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// for now disabling error caused by not adding documentation to methods 
#pragma warning disable CS1591

namespace System.Text.Json
{
    public partial class JsonNumber : JsonNode, IEquatable<JsonNumber>
    {
        // number representation stored:
        // * in big-endian if created from number
        // * in Utf8 if created from string
        private byte[] _bytes;

        public JsonNumber() { }
        public JsonNumber(string value)
        {
            SetString(value);
        }
        public JsonNumber(byte value)
        {
            SetByte(value);
        }
        public JsonNumber(short value)
        {
            SetInt16(value);
        }
        public JsonNumber(int value)
        {
            SetInt32(value);
        }

        public JsonNumber(long value)
        {
            SetInt64(value);
        }
        public JsonNumber(float value)
        {
            SetSingle(value);
        }
        public JsonNumber(double value)
        {
            SetDouble(value);
        }
        [CLSCompliant(false)]
        public JsonNumber(sbyte value)
        {
            SetSByte(value);
        }
        [CLSCompliant(false)]
        public JsonNumber(ushort value)
        {
            SetUInt16(value);
        }
        [CLSCompliant(false)]
        public JsonNumber(uint value)
        {
            SetUInt32(value);
        }
        [CLSCompliant(false)]
        public JsonNumber(ulong value)
        {
            SetUInt64(value);
        }

        private void AdjustBitOrder()
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(_bytes);
        }

        public string GetString() { throw null; }
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

        public void SetString(string value)
        {
            _bytes = Encoding.UTF8.GetBytes(value);
        }
        public void SetByte(byte value)
        {
            _bytes = new byte[1];
            _bytes[0] = value;
            AdjustBitOrder();
        }
        public void SetInt32(int value)
        {
            _bytes = BitConverter.GetBytes(value);
            AdjustBitOrder();
        }
        public void SetInt16(short value)
        {
            _bytes = BitConverter.GetBytes(value);
            AdjustBitOrder();
        }
        public void SetInt64(long value)
        {
            _bytes = BitConverter.GetBytes(value);
            AdjustBitOrder();
        }
        public void SetSingle(float value)
        {
            _bytes = BitConverter.GetBytes(value);
            AdjustBitOrder();
        }
        public void SetDouble(double value)
        {
            _bytes = BitConverter.GetBytes(value);
            AdjustBitOrder();
        }
        [CLSCompliant(false)]
        public void SetSByte(sbyte value)
        {
            _bytes = new byte[1];
            _bytes[0] = (byte)value;
            AdjustBitOrder();
        }
        [CLSCompliant(false)]
        public void SetUInt16(ushort value)
        {
            _bytes = BitConverter.GetBytes(value);
            AdjustBitOrder();
        }
        [CLSCompliant(false)]
        public void SetUInt32(uint value)
        {
            _bytes = BitConverter.GetBytes(value);
            AdjustBitOrder();
        }
        [CLSCompliant(false)]
        public void SetUInt64(ulong value)
        {
            _bytes = BitConverter.GetBytes(value);
            AdjustBitOrder();
        }

        public static implicit operator JsonNumber(byte value) { throw null; }
        public static implicit operator JsonNumber(int value) { throw null; }
        public static implicit operator JsonNumber(short value) { throw null; }
        public static implicit operator JsonNumber(long value) { throw null; }
        public static implicit operator JsonNumber(float value) { throw null; }
        public static implicit operator JsonNumber(double value) { throw null; }
        [CLSCompliant(false)]
        public static implicit operator JsonNumber(sbyte value) { throw null; }
        [CLSCompliant(false)]
        public static implicit operator JsonNumber(ushort value) { throw null; }
        [CLSCompliant(false)]
        public static implicit operator JsonNumber(uint value) { throw null; }
        [CLSCompliant(false)]
        public static implicit operator JsonNumber(ulong value) { throw null; }

        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }

        public bool Equals(JsonNumber other) { throw null; }

        public static bool operator ==(JsonNumber left, JsonNumber right) => throw null;
        public static bool operator !=(JsonNumber left, JsonNumber right) => throw null;
    }
}

#pragma warning restore CS1591
