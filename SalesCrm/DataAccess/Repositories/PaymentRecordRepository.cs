using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;

namespace SalesCrm.DataAccess.Repositories;

public class PaymentRecordRepository : IPaymentRecordRepository
{
    private EmployeeDbContext _context;

    public PaymentRecordRepository(EmployeeDbContext context) => _context = context;

    public async Task CreatePaymentRecordAsync(PaymentRecord paymentRecord)
    {
        await _context.PaymentRecords.AddAsync(paymentRecord);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PaymentRecord>> GetPaymentRecordList()
    {
        return await _context.PaymentRecords
            .Include(n => n.Employee)
            .Include(m => m.TaxYear)
            .ToListAsync();
    }

    public async Task<PaymentRecord> GetEmployeePaymentRecordAsync(Guid paymentRecordId)
    {
        return await _context.PaymentRecords
            .Include(pr => pr.Employee)
            .Include(pr => pr.TaxYear)
            .Where(pr => pr.Id == paymentRecordId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }
}
