using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_bll.Models.Transaction;
using PickNPlay.picknplay_bll.Services;
using Stripe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickNPlay.Controllers
{
    /// <summary>
    /// Controller for handling transaction operations.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// </summary>
        /// <param name="service">The transaction service.</param>
        public TransactionController(TransactionService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets a transaction by its ID.
        /// </summary>
        /// <param name="id">The ID of the transaction.</param>
        /// <returns>A transaction with the specified ID.</returns>
        /// <response code="200">Returns the transaction.</response>
        /// <response code="404">If the transaction is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TransactionGet))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TransactionGet>> GetTransactionByIdAsync(int id)
        {
            var transaction = await service.GetByIdAsync(id);
            if (transaction != null)
            {
                return Ok(transaction);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all transactions.
        /// </summary>
        /// <returns>All transactions.</returns>
        /// <response code="200">Returns the list of transactions.</response>
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionGet>))]
        public async Task<ActionResult<IEnumerable<TransactionGet>>> GetAllTransactionsAsync()
        {
            var transactions = await service.GetAllAsync();
            return Ok(transactions);
        }

        /// <summary>
        /// Filters transactions based on the provided criteria.
        /// </summary>
        /// <param name="listingId">The ID of the listing.</param>
        /// <param name="amountMoreThan">Filters transactions with amount greater than this value.</param>
        /// <param name="amountLessThan">Filters transactions with amount less than this value.</param>
        /// <param name="buyerId">The ID of the buyer.</param>
        /// <param name="sellerId">The ID of the seller.</param>
        /// <param name="status">The status of the transaction.</param>
        /// <param name="after">Filters transactions occurring after this date.</param>
        /// <param name="before">Filters transactions occurring before this date.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <returns>A list of filtered transactions.</returns>
        [Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> Filter(
        int? listingId, int? amountMoreThan, int? amountLessThan,
        int? buyerId, int? sellerId, string? status,
        DateTime? after, DateTime? before,
        int pageNumber = 1, int pageSize = 10)
        {
            var transactions = await service.FilterTransactions(
                listingId, amountMoreThan, amountLessThan,
                buyerId, sellerId, status, after, before,
                pageNumber, pageSize);

            if (transactions == null || !transactions.Any())
            {
                return NotFound();
            }

            return Ok(transactions);
        }
        /// <summary>
        /// Adds a new transaction.
        /// </summary>
        /// <param name="model">The transaction to add.</param>
        /// <returns>The added transaction.</returns>
        /// <response code="201">If the transaction is created successfully.</response>
        /// <response code="400">If the transaction creation fails.</response>
        [HttpPost("add")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PostTransactionAsync([FromBody] TransactionPost model)
        {
            await service.AddAsync(model);
            return Ok();
        }


        [HttpGet("completed/month")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public object StatsByMonth()
        {
            var stats = service.GetTransactionsCountByMonth();
            if (stats == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(stats);
            }
        }


        [HttpGet("completed/year")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public object StatsByYear()
        {
            var stats = service.GetTransactionsCountByYear();
            if (stats == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(stats);
            }
        }

        [HttpGet("average")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public  object AvgTransactions()
        {
            var obj = service.AvgTransactions();
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(obj);
            }
        }

        [HttpGet("commission/month")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public object StatsCommissionsMonth()
        {
            var stats = service.StatsCommissionsMonth();
            if (stats == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(stats);
            }
        }

        [HttpGet("commission/year")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public object StatsCommissionsYear()
        {
            var stats = service.StatsCommissionsYear();
            if (stats == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(stats);
            }
        }

        [HttpPut("success")]
        [ProducesResponseType(204, Type = typeof(TransactionGet))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TransactionGet>> SetStatusSuccess(int transactionId)
        {
            try
            {
                var transaction = await service.SetSuccessStatus(transactionId);
                return StatusCode(204, "Updated");
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPut("cancelled")]
        [ProducesResponseType(204, Type = typeof(TransactionGet))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TransactionGet>> SetStatusCancelled(int transactionId)
        {
            try
            {
                var transaction = await service.SetCancelledStatus(transactionId);
                return StatusCode(204, "Updated");
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Updates an existing transaction.
        /// </summary>
        /// <param name="id">The ID of the transaction to update.</param>
        /// <param name="model">The updated transaction details.</param>
        /// <returns>An updated transaction.</returns>
        /// <response code="200">If the transaction is updated successfully.</response>
        /// <response code="400">If the update fails.</response>
        /// <response code="404">If the transaction is not found.</response>
        [HttpPut("{id}/edit")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> PutTransactionAsync(int id, [FromBody] TransactionUpdate model)
        {
            await service.UpdateAsync(id, model);
            return Ok();
        }

        /// <summary>
        /// Deletes a transaction.
        /// </summary>
        /// <param name="id">The ID of the transaction to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the transaction is deleted successfully.</response>
        /// <response code="404">If the transaction is not found.</response>
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> RemoveTransactionAsync(int id)
        {
            await service.DeleteAsync(id);
            return Ok();
        }
    }
}
