using Azure;
using bossabox_backend_challange.Data;
using bossabox_backend_challange.DTOs;
using bossabox_backend_challange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace bossabox_backend_challange.Controllers;

[ApiController]
[Route("[Controller]")]
public class ToolsController : Controller
{
    private readonly VUTTRDbContext _context;

    public ToolsController(VUTTRDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateToolAsync([FromBody] CreateToolDto dto) 
    {
        var tags = new List<Tag>();
        foreach (var tag in dto.Tags)
        {
            var tagExists = await _context.Tags.FirstOrDefaultAsync(x => x.Name.Equals(tag));
            if (tagExists == null)
            {
                var newTag = new Tag { Name = tag };
                await _context.Tags.AddAsync(newTag);
                tags.Add(newTag);
            }
            else
            {
                tags.Add(tagExists);
            }
        }

        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Name.Equals(userName));
        if (user == null)
        {
            return NotFound(new ResultDto<object> { Error = "Usuário não encontrado" });
        }

        var tool = new Tool
        {
            Title = dto.Title,
            Description = dto.Description,
            Link = dto.Link,
            Tags = tags,
            UserId = user.Id
        };

        await _context.Tools.AddAsync(tool);
        await _context.SaveChangesAsync();

        return Created($"/tools/{tool.Id}", new ResultDto<object> { Data = new { tool.Id } });
    }
}
