using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Uploads;

public class UploadMonthlyDataCommand : IRequest
{
    public byte[] Data { get; set; } = new byte[0];
    public string YearMonth { get; set; } = string.Empty;
}

public class UploadMonthlyDataCommandHandler : IRequestHandler<UploadMonthlyDataCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IExcelService _excelService;
    private readonly IValidator<MonthlyDataDto> _validator;
    private readonly ILogger<UploadMonthlyDataCommandHandler> _logger;

    public UploadMonthlyDataCommandHandler(IExcelService excelService, ILogger<UploadMonthlyDataCommandHandler> logger, IAppDbContext dbContext, IValidator<MonthlyDataDto> validator)
    {
        _excelService = excelService;
        _logger = logger;
        _dbContext = dbContext;
        _validator = validator;
    }
    public async Task Handle(UploadMonthlyDataCommand request, CancellationToken cancellationToken)
    {
        List<MonthlyDataDto> monthlyData;
        try
        {
            monthlyData = _excelService.Import<MonthlyDataDto>(new MemoryStream(request.Data)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading monthly data");
            throw new BadRequestException("Please upload data in the correct format");
        }

        foreach (var data in monthlyData)
        {
            var validationResult = await _validator.ValidateAsync(data, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Calculate previous year-month
            var previousYearMonth = GetPreviousYearMonth(request.YearMonth);

            // Look for previous MemberLedger entry
            var prevLedger = await _dbContext.MemberLedgers
                .Where(m => m.EmpNo == data.EmpNo && m.YearMonth == previousYearMonth)
                .FirstOrDefaultAsync(cancellationToken);

            var prevDepositBal = prevLedger?.DepositBal ?? 0;

            var memberLedger = new MemberLedger
            {
                EmpNo = data.EmpNo,
                YearMonth = request.YearMonth,
                Deposit = data.Deposit,
                LoanRepay = data.LoanRepay
            };

            memberLedger.SetDepositBal(prevDepositBal + data.Deposit);
           
            var prevLoanBal = prevLedger?.LoanBal ?? 0;
            memberLedger.SetLoanBal(prevLoanBal - data.LoanRepay);

            _dbContext.MemberLedgers.Add(memberLedger);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private string GetPreviousYearMonth(string yearMonth)
    {
        if (!DateTime.TryParseExact(yearMonth + "01", "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var date))
        {
            throw new ArgumentException("Invalid YearMonth format", nameof(yearMonth));
        }

        var prevMonth = date.AddMonths(-1);
        return prevMonth.ToString("yyyyMM");
    }
    
}
