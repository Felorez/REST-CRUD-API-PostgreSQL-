using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using RestApi.Function;
using System;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UsersDbContext _context;

    public UserController(UsersDbContext context)
    {
        _context = context;
    }

    [HttpOptions]
    public ActionResult AllowRequest()
    {
        return Ok("Allow: GET, POST, PUT, DELETE, HEAD");
    }

    [HttpHead]
    public ActionResult IsAlive()
    {
        return Ok("Content - Type: text / plain; charset = UTF - 8");
    }

    // api/user/IIN
    [HttpGet("IIN")]
    public async Task<ActionResult<IEnumerable<UserIIN>>> GetUserIINs()
    {
        var query = _context.UserIINs.AsQueryable();

        return Ok(await query.ToListAsync());
    }

    // api/user/IIN/{id}
    [HttpGet("IIN/{id}")]
    public async Task<ActionResult<UserIIN>> GetUserIIN(int id)
    {
        var user = await _context.UserIINs.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // GET: api/user
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(
        string? name = null,
        string? city = null,
        int? minAge = null,
        int? maxAge = null,
        string? sortBy = null,
        bool descending = false,
        int page = 1,
        int pageSize = 100
        )
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u => u.Name.Contains(name));
        }

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(u => u.City == city);
        }

        if (minAge.HasValue)
        {
            query = query.Where(u => u.Age >= minAge);
        }

        if (maxAge.HasValue)
        {
            query = query.Where(u => u.Age <= maxAge);
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "id" => descending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                "name" => descending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                "city" => descending ? query.OrderByDescending(u => u.City) : query.OrderBy(u => u.City),
                "age" => descending ? query.OrderByDescending(u => u.Age) : query.OrderBy(u => u.Age),
                _ => query
            };
        }

        return Ok(new
        {
            TotalCount = await _context.Users.CountAsync(),
            Page = page,
            PageSize = pageSize,
            Data = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync()
        });
    }

    // GET: api/user/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/user
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        var userIIN = new UserIIN { IIN = GenerateIIN.GenerateNumberUser(user), User = user };
        user.Id = null;
        user.UserIIN = userIIN;

        _context.Users.Add(user);
        _context.UserIINs.Add(userIIN);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/user/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        user.Id = id;

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/user/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
