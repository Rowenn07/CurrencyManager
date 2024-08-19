using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyCRUDController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<CurrencyCRUDController> _logger;

        public CurrencyCRUDController(MyDbContext context, ILogger<CurrencyCRUDController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all conversion histories.
        /// </summary>
        /// <returns>A list of conversion history records.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversionHistory>>> GetConversionHistories()
        {
            try
            {
                return await _context.ConversionHistories.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving conversion histories");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific conversion history by ID.
        /// </summary>
        /// <param name="id">The ID of the conversion history.</param>
        /// <returns>The conversion history record.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ConversionHistory>> GetConversionHistory(int id)
        {
            try
            {
                var conversionHistory = await _context.ConversionHistories.FindAsync(id);

                if (conversionHistory == null)
                {
                    return NotFound($"ConversionHistory with ID {id} not found.");
                }

                return conversionHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving conversion history with ID {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new conversion history record.
        /// </summary>
        /// <param name="conversionHistory">The conversion history record to create.</param>
        /// <returns>The created conversion history record.</returns>
        [HttpPost]
        public async Task<ActionResult<ConversionHistory>> PostConversionHistory(ConversionHistory conversionHistory)
        {
            try
            {
                _context.ConversionHistories.Add(conversionHistory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetConversionHistory), new { id = conversionHistory.Id }, conversionHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new conversion history");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing conversion history record.
        /// </summary>
        /// <param name="id">The ID of the conversion history to update.</param>
        /// <param name="conversionHistory">The updated conversion history record.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConversionHistory(int id, ConversionHistory conversionHistory)
        {
            if (id != conversionHistory.Id)
            {
                return BadRequest("ID mismatch.");
            }

            _context.Entry(conversionHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversionHistoryExists(id))
                {
                    return NotFound($"ConversionHistory with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("A concurrency error occurred while updating conversion history with ID {Id}", id);
                    return StatusCode(500, "A concurrency error occurred.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating conversion history with ID {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific conversion history by ID.
        /// </summary>
        /// <param name="id">The ID of the conversion history to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversionHistory(int id)
        {
            try
            {
                var conversionHistory = await _context.ConversionHistories.FindAsync(id);
                if (conversionHistory == null)
                {
                    return NotFound($"ConversionHistory with ID {id} not found.");
                }

                _context.ConversionHistories.Remove(conversionHistory);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting conversion history with ID {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a conversion history record exists by ID.
        /// </summary>
        /// <param name="id">The ID of the conversion history.</param>
        /// <returns>True if the record exists, otherwise false.</returns>
        private bool ConversionHistoryExists(int id)
        {
            return _context.ConversionHistories.Any(e => e.Id == id);
        }
    }
}
