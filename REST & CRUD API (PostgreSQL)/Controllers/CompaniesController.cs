using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly UsersDbContext _context;

    public CompaniesController(UsersDbContext context)
    {
        _context = context;
    }

    // api/companies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanies(
        string? name = null,
        int page = 1,
        int pageSize = 100)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 100;

        var query = _context.Companies.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(c => c.Name.Contains(name));
        }

        int totalCount = await query.CountAsync();

        var companies = await query
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            totalCount,
            data = companies
        });
    }

    // api/companies/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        return company;
    }

    // api/companies
    [HttpPost]
    public async Task<ActionResult<Company>> CreateCompany(Company company)
    {
        if (string.IsNullOrWhiteSpace(company.Name))
        {
            return BadRequest("Название компании не может быть пустым.");
        }

        company.Id = null;
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCompanies), new { id = company.Id }, company);
    }

   // api/companies/id
   [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] Company updatedCompany)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null)
        {
            return NotFound("Компания не найдена.");
        }

        if (!string.IsNullOrWhiteSpace(updatedCompany.Name))
        {
            company.Name = updatedCompany.Name;
        }

        _context.Companies.Update(company);
        await _context.SaveChangesAsync();

        return Ok(company);
    }
}
