using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly UsersDbContext _context;

    public ProductsController(UsersDbContext context)
    {
        _context = context;
    }

    // api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        string? search = null,
        string? sortBy = null,
        bool descending = false,
        int page = 1,
        int pageSize = 100)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 100;

        var query = _context.Products
            .Include(p => p.Company)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();

            Console.WriteLine(search);

            query = query.Where(p =>
                p.Name.ToLower().Contains(search) ||
                p.Price.ToString().Contains(search) ||
                p.Stock.ToString().Contains(search) ||
                p.Company.Name.ToLower().Contains(search)
            );
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "id" => descending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                "name" => descending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                "price" => descending ? query.OrderByDescending(u => u.Price) : query.OrderBy(u => u.Price),
                "stock" => descending ? query.OrderByDescending(u => u.Stock) : query.OrderBy(u => u.Stock),
                _ => query
            };
        }

        return Ok(new
        {
            TotalCount = await query.CountAsync(),
            Page = page,
            PageSize = pageSize,
            Data = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync()
        });
    }

    // api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }


    // api/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name) || product.Price < 0 || product.Stock < 0)
        {
            return BadRequest("Некорректные данные.");
        }

        product.Id = null;
        var company = await _context.Companies.FindAsync(product.CompanyId);
        if (company == null)
        {
            return NotFound("Компания не найдена.");
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }

    // api/products/id
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound("Продукт не найден.");
        }

        if (!string.IsNullOrWhiteSpace(updatedProduct.Name))
        {
            product.Name = updatedProduct.Name;
        }

        if (updatedProduct.Price > 0)
        {
            product.Price = updatedProduct.Price;
        }

        if (updatedProduct.Stock >= 0)
        {
            product.Stock = updatedProduct.Stock;
        }

        if (updatedProduct.CompanyId > 0)
        {
            var company = await _context.Companies.FindAsync(updatedProduct.CompanyId);
            if (company == null)
            {
                return NotFound("Компания не найдена.");
            }
            product.CompanyId = updatedProduct.CompanyId;
        }

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return Ok(product);
    }

    // api/products/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound("Продукт не найден.");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
