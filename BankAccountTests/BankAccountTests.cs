using BankAccountApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccountTests
{
    public class BankAccountTests
    {
        [Theory]
        [InlineData("John Doe", 1000, 200, 800)]    // EP: amount < balance
        [InlineData("Jane Smith", 500, 100, 400)]
        [InlineData("Alice Johnson", 300, 50, 250)]
        [InlineData("Boundary", 100, 100, 0)]       // BVA: amount == balance
        [InlineData("Zero", 100, 0, 100)]           // BVA: zero amount
        public void Debit_ValidAmount_UpdatesBalance(string name, decimal initial, decimal debit, decimal expected)
        {
            var account = new BankAccount(name, initial);
            account.Debit(debit);
            Assert.Equal(expected, account.Balance);
        }

        [Theory]
        [InlineData("Bob Brown", 100, 150)]         // EP: amount > balance
        [InlineData("Boundary", 100, 100.01)]       // BVA: just above balance
        public void Debit_InsufficientFunds_ThrowsException(string name, decimal initial, decimal debit)
        {
            var account = new BankAccount(name, initial);
            var ex = Assert.Throws<InvalidOperationException>(() => account.Debit(debit));
            Assert.Equal("Insufficient funds", ex.Message);
        }

        [Theory]
        [InlineData("John Doe", 1000, 200, 1200)]
        [InlineData("Jane Smith", 500, 100, 600)]
        [InlineData("Alice Johnson", 300, 50, 350)]
        [InlineData("Zero", 100, 0, 100)]           // BVA: zero amount
        public void Credit_ValidAmount_UpdatesBalance(string name, decimal initial, decimal credit, decimal expected)
        {
            var account = new BankAccount(name, initial);
            account.Credit(credit);
            Assert.Equal(expected, account.Balance);
        }

        [Theory]
        [InlineData("John Doe", 1000, 200, 800)]
        [InlineData("Jane Smith", 500, 100, 400)]
        [InlineData("Boundary", 100, 100, 0)]       // BVA: withdraw everything
        public void Withdraw_ValidAmount_UpdatesBalance(string name, decimal initial, decimal withdraw, decimal expected)
        {
            var account = new BankAccount(name, initial);
            account.Withdraw(withdraw);
            Assert.Equal(expected, account.Balance);
        }

        [Fact]
        public void Withdraw_InsufficientFunds_ThrowsException()
        {
            var account = new BankAccount("Bob Brown", 100);
            Assert.Throws<InvalidOperationException>(() => account.Withdraw(150));
        }
    }
}