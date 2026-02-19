namespace SOLID_Fundamentals
{

    // Change: now accepts only withdrawable accounts
    public class Bank
    {
        public void ProcessWithdrawal(WithdrawableAccount account, decimal amount)
        {
            try
            {
                account.Withdraw(amount);
                Console.WriteLine($"Successfully withdrew {amount}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Withdrawal failed: {ex.Message}");
            }
        }

        public void Transfer(WithdrawableAccount from, Account to, decimal amount)
        {
            from.Withdraw(amount);
            to.Deposit(amount);
        }
    }
}