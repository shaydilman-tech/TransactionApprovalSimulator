using TransactionApprovalSimulator.Server.Models;

namespace TransactionApprovalSimulator.Server.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetApprovedAsync(CancellationToken cancellation = default);
        Task<Transaction> CreateAsync(TransactionRequest request, CancellationToken cancellation = default);
    }
}