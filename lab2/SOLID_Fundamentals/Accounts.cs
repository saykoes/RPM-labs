using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Fundamentals
{
    public abstract class Account
    {
        public decimal Balance { get; protected set; }

        public virtual void Deposit(decimal amount)
        {
            // Change: added checks
            if (amount <= 0) throw new ArgumentException("Amount must be positive");
            Balance += amount;
        }
        // Change: removed hardcoded CalculateInterest method in base class
        public abstract decimal CalculateInterest();
    }

    // Change: Defining base withdrawable class and here is moved Withdraw method
    public abstract class WithdrawableAccount : Account
    {
        public abstract void Withdraw(decimal amount);
    }

    public class SavingsAccount : WithdrawableAccount
    {
        public decimal MinimumBalance { get; } = 100m;

        public override void Withdraw(decimal amount)
        {
            if (Balance - amount < MinimumBalance)
                throw new InvalidOperationException("Cannot go below minimum balance");

            Balance -= amount;
        }

        public override decimal CalculateInterest() => Balance * 0.01m;
    }

    public class CheckingAccount : WithdrawableAccount
    {
        public decimal OverdraftLimit { get; } = 500m;

        public override void Withdraw(decimal amount)
        {
            if (Balance - amount < -OverdraftLimit)
                throw new InvalidOperationException("Overdraft limit exceeded");

            Balance -= amount;
        }

        public override decimal CalculateInterest() => Balance * 0.005m;
    }

    // Change: now it's seperate from other withdrawable accounts
    public class FixedDepositAccount : Account
    {
        public DateTime MaturityDate { get; }

        public FixedDepositAccount(DateTime maturityDate) => MaturityDate = maturityDate;

        public bool IsMatured => DateTime.Now >= MaturityDate;

        public void CloseAndWithdrawAll(WithdrawableAccount targetAccount)
        {
            if (!IsMatured)
                throw new InvalidOperationException("Cannot close before maturity date");

            targetAccount.Deposit(Balance);
            Balance = 0;
        }

        public override decimal CalculateInterest() => Balance * 0.05m;
    }
}
