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
        [InlineData(Int64.MaxValue)]
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
        public static void TestByte()
        {
            var jsonNumber = new JsonNumber();
            byte value = 17;
            
            jsonNumber.SetByte(value);
            byte result = jsonNumber.GetByte();
            Assert.Equal(value, result);
            
            bool success = jsonNumber.TryGetByte(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestShort()
        {
            var jsonNumber = new JsonNumber();
            short value = 17;

            jsonNumber.SetInt16(value);
            short result = jsonNumber.GetInt16();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetInt16(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestInt()
        {
            var jsonNumber = new JsonNumber();
            int value = 17;

            jsonNumber.SetInt32(value);
            int result = jsonNumber.GetInt32();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetInt32(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestLong()
        {
            var jsonNumber = new JsonNumber();
            long value = 17;

            jsonNumber.SetInt64(value);
            long result = jsonNumber.GetInt64();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetInt64(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestFloat()
        {
            var jsonNumber = new JsonNumber();
            float value = 3.14f;

            jsonNumber.SetSingle(value);
            float result = jsonNumber.GetSingle();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetSingle(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestDouble()
        {
            var jsonNumber = new JsonNumber();
            double value = 3.14;

            jsonNumber.SetDouble(value);
            double result = jsonNumber.GetDouble();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetDouble(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestSbyte()
        {
            var jsonNumber = new JsonNumber();
            sbyte value = 5;

            jsonNumber.SetSByte(value);
            sbyte result = jsonNumber.GetSByte();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetSByte(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestUInt16()
        {
            var jsonNumber = new JsonNumber();
            ushort value = 5;

            jsonNumber.SetUInt16(value);
            ushort result = jsonNumber.GetUInt16();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetUInt16(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestUInt32()
        {
            var jsonNumber = new JsonNumber();
            uint value = 5;

            jsonNumber.SetUInt32(value);
            uint result = jsonNumber.GetUInt32();
            Assert.Equal(value, result);

            bool success = jsonNumber.TryGetUInt32(out result);
            Assert.True(success);
            Assert.Equal(value, result);
        }

        [Fact]
        public static void TestUInt64()
        {
            var jsonNumber = new JsonNumber();
            ulong value = 5;

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
            jsonNumber.SetString(value);
            string result = jsonNumber.GetString();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("3,14")]
        [InlineData("this is not a number")]
        [InlineData("NAN")]
        [InlineData("0.")]
        [InlineData("008")]
        [InlineData("5e")]
        public static void TestInvalidString(string value)
        {
            Assert.Throws<ArgumentException>(() => new JsonNumber(value));
        }

        [Fact]
        public static void TestIntFromString()
        {
            var jsonNumber = new JsonNumber("145");
            int result = jsonNumber.GetInt32();
            Assert.Equal(145, result);
        }
    }
}
