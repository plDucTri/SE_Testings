using CalculatorApp;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using CsvHelper;

namespace BasicMathTests
{
    public class CalculatorCsvTests
    {
        public static IEnumerable<object[]> GetTestData(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                foreach (var record in csv.GetRecords<TestData>().ToList())
                {
                    yield return new object[] { record.A, record.B, record.Expected };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestData), parameters: "add_testdata.csv")]
        public void Add_CsvData_ReturnsCorrectSum(int a, int b, int expected)
        {
            var calculator = new Calculator();
            Assert.Equal(expected, calculator.Add(a, b));
        }

        [Theory]
        [MemberData(nameof(GetTestData), parameters: "subtract_testdata.csv")]
        public void Subtract_CsvData_ReturnsCorrectDifference(int a, int b, int expected)
        {
            var calculator = new Calculator();
            Assert.Equal(expected, calculator.Subtract(a, b));
        }

        [Theory]
        [MemberData(nameof(GetTestData), parameters: "multiply_testdata.csv")]
        public void Multiply_CsvData_ReturnsCorrectProduct(int a, int b, int expected)
        {
            var calculator = new Calculator();
            Assert.Equal(expected, calculator.Multiply(a, b));
        }

        [Theory]
        [MemberData(nameof(GetTestData), parameters: "divide_testdata.csv")]
        public void Divide_CsvData_ReturnsCorrectQuotient(int a, int b, double expected)
        {
            var calculator = new Calculator();
            Assert.Equal(expected, calculator.Divide(a, b));
        }

        public class TestData
        {
            public int A { get; set; }
            public int B { get; set; }
            public int Expected { get; set; }
        }
    }
}