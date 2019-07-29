// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// for now disabling error caused by not adding documentation to methods 
#pragma warning disable CS1591

namespace System.Text.Json
{
    public partial class JsonNumber : JsonNode, IEquatable<JsonNumber>
    {
        private string _value;

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

        public string GetString()
        { 
            return _value; 
        }
        public byte GetByte()
        {
            return byte.Parse(_value);
        }
        public short GetInt16()
        {
            return short.Parse(_value);
        }
        public int GetInt32()
        {
            return int.Parse(_value);
        }
        public long GetInt64()
        {
            return long.Parse(_value);
        }
        public float GetSingle()
        {
            return float.Parse(_value);
        }
        public double GetDouble()
        {
            return double.Parse(_value);
        }
        [CLSCompliant(false)]
        public sbyte GetSByte()
        {
            return sbyte.Parse(_value);
        }
        [CLSCompliant(false)]
        public ushort GetUInt16()
        {
            return ushort.Parse(_value);
        }
        [CLSCompliant(false)]
        public uint GetUInt32()
        {
            return uint.Parse(_value);
        }
        [CLSCompliant(false)]
        public ulong GetUInt64()
        {
            return ulong.Parse(_value);
        }

        public bool TryGetByte(out byte value)
        {
            try
            {
                value = GetByte();
                return true;
            } 
           catch(Exception)
            {
                value = 0;
                return false;
            }
        }
        public bool TryGetInt16(out short value)
        {
            try
            {
                value = GetInt16();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        public bool TryGetInt32(out int value)
        {
            try
            {
                value = GetInt32();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        public bool TryGetInt64(out long value)
        {
            try
            {
                value = GetInt64();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        public bool TryGetSingle(out float value)
        {
            try
            {
                value = GetSingle();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        public bool TryGetDouble(out double value)
        {
            try
            {
                value = GetDouble();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        [CLSCompliant(false)]
        public bool TryGetSByte(out sbyte value)
        {
            try
            {
                value = GetSByte();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        [CLSCompliant(false)]
        public bool TryGetUInt16(out ushort value)
        {
            try
            {
                value = GetUInt16();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        [CLSCompliant(false)]
        public bool TryGetUInt32(out uint value)
        {
            try
            {
                value = GetUInt32();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
        [CLSCompliant(false)]
        public bool TryGetUInt64(out ulong value)
        {
            try
            {
                value = GetUInt64();
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }

        public void SetString(string value)
        {
            _value = value;
        }
        public void SetByte(byte value)
        {
            _value = value.ToString();
        }
        public void SetInt16(short value)
        {
            _value = value.ToString();
        }
        public void SetInt32(int value)
        {
            _value = value.ToString();
        }
        public void SetInt64(long value)
        {
            _value = value.ToString();
        }
        public void SetSingle(float value)
        {
            _value = value.ToString();
        }
        public void SetDouble(double value)
        {
            _value = value.ToString();
        }
        [CLSCompliant(false)]
        public void SetSByte(sbyte value)
        {
            _value = value.ToString();
        }
        [CLSCompliant(false)]
        public void SetUInt16(ushort value)
        {
            _value = value.ToString();
        }
        [CLSCompliant(false)]
        public void SetUInt32(uint value)
        {
            _value = value.ToString();
        }
        [CLSCompliant(false)]
        public void SetUInt64(ulong value)
        {
            _value = value.ToString();
        }

        public static implicit operator JsonNumber(byte value)
        {
            return new JsonNumber(value);
        }
        public static implicit operator JsonNumber(int value)
        {
            return new JsonNumber(value);
        }
        public static implicit operator JsonNumber(short value)
        {
            return new JsonNumber(value);
        }
        public static implicit operator JsonNumber(long value)
        {
            return new JsonNumber(value);
        }
        public static implicit operator JsonNumber(float value)
        {
            return new JsonNumber(value);
        }
        public static implicit operator JsonNumber(double value)
        {
            return new JsonNumber(value);
        }
        [CLSCompliant(false)]
        public static implicit operator JsonNumber(sbyte value)
        {
            return new JsonNumber(value);
        }
        [CLSCompliant(false)]
        public static implicit operator JsonNumber(ushort value)
        {
            return new JsonNumber(value);
        }
        [CLSCompliant(false)]
        public static implicit operator JsonNumber(uint value)
        {
            return new JsonNumber(value);
        }
        [CLSCompliant(false)]
        public static implicit operator JsonNumber(ulong value)
        {
            return new JsonNumber(value);
        }

        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }

        public bool Equals(JsonNumber other) { throw null; }

        public static bool operator ==(JsonNumber left, JsonNumber right) => throw null;
        public static bool operator !=(JsonNumber left, JsonNumber right) => throw null;
    }
}

#pragma warning restore CS1591
