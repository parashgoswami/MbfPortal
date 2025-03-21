namespace Domain.Entities;

public class MemberLedger : BaseEntity
{
    public string EmpNo { get; set; } = string.Empty;    
    public int YearMonth { get; set; }

    public decimal Deposit { get; set; }
    public decimal Withdrawal { get; private set; }
    public decimal DepositBal { get; private set; }
    public decimal DepositInt { get; private set; }

    public decimal Loan { get; private set; }
    public decimal LoanRepay { get; set; }
    public decimal LoanBal { get; private set; }
    public decimal LoanInt { get; private set; }

    public void SetWithdrawal(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Withdrawal amount cannot be negative", nameof(amount));
        }
        Withdrawal = amount;
    }

    public void SetDepositBal(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Deposit balance cannot be negative", nameof(amount));
        }
        DepositBal = amount;
    }

    public void SetDepositInt(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Deposit interest cannot be negative", nameof(amount));
        }
        DepositInt = amount;
    }

    public void SetLoan(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Loan amount cannot be negative", nameof(amount));
        }
        Loan = amount;
    }

    public void SetLoanBal(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Loan balance cannot be negative", nameof(amount));
        }
        LoanBal = amount;
    }

    public void SetLoanInt(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Loan interest cannot be negative", nameof(amount));
        }
        LoanInt = amount;
    }
}
