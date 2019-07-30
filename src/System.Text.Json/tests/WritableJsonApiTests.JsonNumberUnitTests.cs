// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json
{
    public static partial class WritableJsonApiTests
    {
        [Theory]
        [InlineData(-456)]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(long.MaxValue)]
        [InlineData(2.3)]
        [InlineData(-17.009)]
        [InlineData(3.14f)]
        [InlineData(0x2A)]
        [InlineData(0b_0110_1010)]
        [InlineData("1e400")]
        [InlineData("1e+100000002")]
        [InlineData("184467440737095516150.184467440737095516150")]
        public static void TestJsonNumberConstructor(object value)
        {
            // should not throw any exceptions:
            var jsonNumber = new JsonNumber(value as dynamic);
        }

        [Fact]
        public static void TestDefaultCtor()
        {
            var jsonNumber = new JsonNumber();
            Assert.Equal(0, jsonNumber.GetInt32());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(255)]
        public static void TestByte(byte value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetByte(value);
            byte result = jsonNumber.GetByte();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetByte(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-17)]
        [InlineData(17)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        public static void TestShort(short value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetInt16(value);
            short result = jsonNumber.GetInt16();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetInt16(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-17)]
        [InlineData(17)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public static void TestInt(int value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetInt32(value);
            int result = jsonNumber.GetInt32();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetInt32(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-17)]
        [InlineData(17)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        public static void TestLong(long value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetInt64(value);
            long result = jsonNumber.GetInt64();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetInt64(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(-17)]
        [InlineData(3.14)]
        [InlineData(-15.5)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        public static void TestFloat(float value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetSingle(value);
            float result = jsonNumber.GetSingle();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetSingle(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(-17)]
        [InlineData(3.14)]
        [InlineData(-15.5)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        public static void TestDouble(double value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetDouble(value);
            double result = jsonNumber.GetDouble();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetDouble(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(-17)]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        public static void TestSbyte(sbyte value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetSByte(value);
            sbyte result = jsonNumber.GetSByte();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetSByte(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(ushort.MaxValue)]
        public static void TestUInt16(ushort value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetUInt16(value);
            ushort result = jsonNumber.GetUInt16();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetUInt16(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(uint.MaxValue)]
        public static void TestUInt32(uint value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetUInt32(value);
            uint result = jsonNumber.GetUInt32();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetUInt32(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(ulong.MaxValue)]
        public static void TestUInt64(ulong value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetUInt64(value);
            ulong result = jsonNumber.GetUInt64();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetUInt64(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("3.14")]
        [InlineData("-17")]
        [InlineData("0")]
        [InlineData("189")]
        [InlineData("1e400")]
        [InlineData("1e+100000002")]
        [InlineData("184467440737095516150.184467440737095516150")]
        public static void TestString(string value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetFormattedValue(value);
            string result = jsonNumber.ToString();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("3,14")]
        [InlineData("this is not a number")]
        [InlineData("NAN")]
        [InlineData("0.")]
        [InlineData("008")]
        [InlineData("0e")]
        [InlineData("5e")]
        [InlineData("5a")]
        [InlineData("0.1e")]
        [InlineData("-01")]
        [InlineData("10.5e")]
        [InlineData("10.5e-")]
        [InlineData("10.5e+")]
        [InlineData("10.5e-0.2")]
        [InlineData(" 6")]
        [InlineData("6 ")]
        [InlineData(" 6 ")]
        [InlineData("+0")]
        [InlineData("+1")]
        public static void TestInvalidString(string value)
        {
            Assert.Throws<ArgumentException>(() => new JsonNumber(value));
        }

        [Fact]
        public static void TestNullString()
        {
            Assert.Throws<NullReferenceException>(() => new JsonNumber(null));
        }

        [Fact]
        public static void TestIntFromString()
        {
            var jsonNumber = new JsonNumber("145");
            int result = jsonNumber.GetInt32();
            Assert.Equal(145, result);
        }

        [Fact]
        public static void TestUpcasts()
        {
            byte value = 17;
            var jsonNumber = new JsonNumber(value);

            // Geeting other types should also succeed:
            short shortResult = jsonNumber.GetInt16();
            Assert.Equal(value, shortResult);
            bool success = jsonNumber.TryGetInt16(out shortResult);
            Assert.True(success);
            Assert.Equal(value, shortResult);

            int intResult = jsonNumber.GetInt32();
            Assert.Equal(value, intResult);
            success = jsonNumber.TryGetInt32(out intResult);
            Assert.True(success);
            Assert.Equal(value, intResult);

            long longResult = jsonNumber.GetInt64();
            Assert.Equal(value, longResult);
            success = jsonNumber.TryGetInt64(out longResult);
            Assert.True(success);
            Assert.Equal(value, longResult);

            float floatResult = jsonNumber.GetSingle();
            Assert.Equal(value, floatResult);
            success = jsonNumber.TryGetSingle(out floatResult);
            Assert.True(success);
            Assert.Equal(value, floatResult);

            double doubleResult = jsonNumber.GetDouble();
            Assert.Equal(value, doubleResult);
            success = jsonNumber.TryGetDouble(out doubleResult);
            Assert.True(success);
            Assert.Equal(value, doubleResult);

            sbyte sbyteResult = jsonNumber.GetSByte();
            Assert.Equal(value, (byte)sbyteResult);
            success = jsonNumber.TryGetSByte(out sbyteResult);
            Assert.True(success);
            Assert.Equal(value, (byte)sbyteResult);

            ushort ushortResult = jsonNumber.GetUInt16();
            Assert.Equal(value, ushortResult);
            success = jsonNumber.TryGetUInt16(out ushortResult);
            Assert.True(success);
            Assert.Equal(value, ushortResult);

            uint uintResult = jsonNumber.GetUInt32();
            Assert.Equal(value, uintResult);
            success = jsonNumber.TryGetUInt32(out uintResult);
            Assert.True(success);
            Assert.Equal(value, uintResult);

            ulong ulongResult = jsonNumber.GetUInt64();
            Assert.Equal(value, ulongResult);
            success = jsonNumber.TryGetUInt64(out ulongResult);
            Assert.True(success);
            Assert.Equal(value, ulongResult);
        }

        [Fact]
        public static void TestIntegerGetMismatches()
        {
            var jsonNumber = new JsonNumber(long.MaxValue);

            // Geeting smaller types should fail:
            bool success = jsonNumber.TryGetByte(out byte byteResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetByte());

            success = jsonNumber.TryGetInt16(out short shortResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetInt16());

            success = jsonNumber.TryGetInt32(out int intResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetInt32());

            success = jsonNumber.TryGetSByte(out sbyte sbyteResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetSByte());

            success = jsonNumber.TryGetUInt16(out ushort ushortResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetUInt16());

            success = jsonNumber.TryGetUInt32(out uint uintResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetUInt32());
        }

        [Fact]
        public static void TestUnsignedGetMismatches()
        {
            var jsonNumber = new JsonNumber("-1");

            // Geeting unsigned types should fail:
            bool success = jsonNumber.TryGetByte(out byte byteResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetByte());

            success = jsonNumber.TryGetUInt16(out ushort ushortResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetUInt16());

            success = jsonNumber.TryGetUInt32(out uint uintResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetUInt32());

            success = jsonNumber.TryGetUInt64(out ulong ulongResult);
            Assert.False(success);
            Assert.Throws<OverflowException>(() => jsonNumber.GetUInt64());
        }

        [Fact]
        public static void TestRationalGetMismatches()
        {
            var jsonNumber = new JsonNumber("3.14");

            // Geeting integer types should fail:
            bool success = jsonNumber.TryGetByte(out byte byteResult);
            Assert.False(success);
            Assert.Throws<FormatException>(() => jsonNumber.GetByte());

            success = jsonNumber.TryGetInt16(out short shortResult);
            Assert.False(success);
            Assert.Throws<FormatException>(() => jsonNumber.GetInt16());

            success = jsonNumber.TryGetInt32(out int intResult);
            Assert.False(success);
            Assert.Throws<FormatException>(() => jsonNumber.GetInt32());

            success = jsonNumber.TryGetInt64(out long longResult);
            Assert.False(success);
            Assert.Throws<FormatException>(() => jsonNumber.GetInt64());

            success = jsonNumber.TryGetSByte(out sbyte sbyteResult);
            Assert.False(success);
            Assert.Throws<FormatException>(() => jsonNumber.GetSByte());

            success = jsonNumber.TryGetUInt16(out ushort ushortResult);
            Assert.False(success);
            Assert.Throws<FormatException>(() => jsonNumber.GetUInt16());

            success = jsonNumber.TryGetUInt32(out uint uintResult);
            Assert.False(success);
            Assert.Throws<FormatException>(() => jsonNumber.GetUInt32());

            success = jsonNumber.TryGetUInt64(out ulong ulongResult);
            Assert.False(success);
            Assert.Throws<FormatException>(() => jsonNumber.GetUInt64());
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-17")]
        [InlineData("17")]
        [InlineData("3.14")]
        [InlineData("-3.1415")]
        [InlineData("1234567890")]
        [InlineData("1e400")]
        [InlineData("1e+100000002")]
        [InlineData("184467440737095516150.184467440737095516150")]
        public static void TestToString(string value)
        {
            var jsonNumber = new JsonNumber();
            jsonNumber.SetFormattedValue(value);
            Assert.Equal(value, jsonNumber.ToString());
        }

        [Fact]
        public static void TestChangingTypes()
        {
            var jsonNumber = new JsonNumber(5);
            Assert.Equal(5, jsonNumber.GetInt32());

            jsonNumber.SetDouble(3.14);
            Assert.Equal(3.14, jsonNumber.GetDouble());

            jsonNumber.SetByte(17);
            Assert.Equal(17, jsonNumber.GetByte());

            jsonNumber.SetInt64(long.MaxValue);
            Assert.Equal(long.MaxValue, jsonNumber.GetInt64());

            jsonNumber.SetUInt16(ushort.MaxValue);
            Assert.Equal(ushort.MaxValue, jsonNumber.GetUInt16());

            jsonNumber.SetSingle(-1.1f);
            Assert.Equal(-1.1f, jsonNumber.GetSingle());

            jsonNumber.SetSByte(4);
            Assert.Equal(4, jsonNumber.GetSByte());

            jsonNumber.SetUInt32(127);
            Assert.Equal((uint)127, jsonNumber.GetUInt32());

            jsonNumber.SetFormattedValue("1e400");
            Assert.Equal("1e400", jsonNumber.ToString());

            jsonNumber.SetUInt64(ulong.MaxValue);
            Assert.Equal(ulong.MaxValue, jsonNumber.GetUInt64());
        }
    }
}
