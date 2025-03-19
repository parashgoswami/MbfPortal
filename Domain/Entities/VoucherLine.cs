using Domain.Exceptions;

namespace Domain.Entities;
public class VoucherLine : BaseEntity
{
    public int VoucherId { get; private set; }
    public int AccountHeadId { get; private set; }
    public decimal DebitAmt { get; private set; }
    public decimal CreditAmt { get; private set; }
    public string Narration { get; private set; } = string.Empty;
    public Voucher? Voucher { get; private set; }

    public VoucherLine(int accountHeadId, decimal debitAmt, decimal creditAmt, string narration)
    {
        if (debitAmt < 0)
        {
            throw new InvalidAmountException(nameof(debitAmt));
        }

        if (creditAmt < 0)
        {
            throw new InvalidAmountException(nameof(creditAmt));
        }

        AccountHeadId = accountHeadId;
        DebitAmt = debitAmt;
        CreditAmt = creditAmt;
        Narration = narration;
    }

    public void SetAccountHeadId(int accountHeadId)
    {
        AccountHeadId = accountHeadId;
    }

    public void SetDebitAmt(decimal debitAmt)
    {
        if (debitAmt < 0)
        {
            throw new InvalidAmountException(nameof(debitAmt));
        }
        DebitAmt = debitAmt;
    }

    public void SetCreditAmt(decimal creditAmt)
    {
        if (creditAmt < 0)
        {
            throw new InvalidAmountException(nameof(creditAmt));
        }
        CreditAmt = creditAmt;
    }

    public void SetNarration(string narration)
    {
        Narration = narration;
    }
}