using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManager.API.Data;


namespace OrderManager.API.Controller;


[ApiController]
[Route("[controller]")]
public class DatabaseController: ControllerBase
{
    private readonly OrderManagerDbContext _context;

    public DatabaseController(OrderManagerDbContext context)
    {
        _context = context;
    }

    [HttpGet("Status")]
    public async Task<IActionResult> GetDatabaseStatus()
    {
        var customerCount = await _context.Customers.CountAsync();

        return Ok(
            new {
                Message = "Database connected successfully",
                Database = "SQlite",
                TotalCustomers = customerCount
            }
        );

    }

}
