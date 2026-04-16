using Microsoft.AspNetCore.Mvc;
using TransactionApprovalSimulator.Server.Models;
using TransactionApprovalSimulator.Server.Services;

namespace TransactionApprovalSimulator.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet("approved", Name = "GetApprovedTransactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetApprovedTransactions()
        {
            var approved = await _service.GetApprovedAsync();
            return Ok(approved);
        }

        [HttpPost(Name = "PostTrancaction")]
        public async Task<ActionResult<Transaction>> PostTransaction([FromBody] TransactionRequest request)
        {
            if (request == null) return BadRequest("Request body is required.");

            try
            {
                var tx = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetApprovedTransactions), null, tx);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
