using BankAccountApp;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BankAccountTests
{
    public class BankAccountCsvTests
    {
        public static IEnumerable<object[]> GetTestData()
        {
            using (var reader = new StreamReader("BankAccountTestData.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                foreach (var r in csv.GetRecords<TestData>().ToList())
                {
                    yield return new object[] { r.CustomerName, r.InitialBalance, r.DebitAmount, r.ExpectedBalance };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Debit_CsvData_UpdatesBalance(string customerName, decimal initialBalance, decimal debitAmount, string expectedBalance)
        {
            var account = new BankAccount(customerName, initialBalance);

            if (expectedBalance == "Insufficient funds")
            {
                Assert.Throws<InvalidOperationException>(() => account.Debit(debitAmount));
            }
            else
            {
                account.Debit(debitAmount);
                Assert.Equal(decimal.Parse(expectedBalance, CultureInfo.InvariantCulture), account.Balance);
            }
        }

        public class TestData
        {
            public string CustomerName { get; set; }
            public decimal InitialBalance { get; set; }
            public decimal DebitAmount { get; set; }
            public string ExpectedBalance { get; set; }
        }
    }
}