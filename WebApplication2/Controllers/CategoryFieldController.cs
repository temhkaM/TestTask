using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.DtoModels;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryFieldController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;

        public CategoryFieldController(ApplicationContext db)
        {
            _dbContext = db;
        }

        [HttpGet("GetAllFields")]
        public async Task<IActionResult> GetAllFields()
        {
            var fields = await _dbContext.CategoryFields.ToListAsync();
            return new ObjectResult(fields);
        }

        [HttpGet("GetAllFieldsByCategoryId")]
        public async Task<IActionResult> GetAllFieldsByCategoryId([FromQuery] int categoryId)
        {
            var fields = await _dbContext.CategoryFields.Where(cf => cf.CategoryId == categoryId).ToListAsync();
            return new ObjectResult(fields);
        }

        [HttpGet("GetFieldById")]
        public async Task<IActionResult> GetFieldById([FromQuery] int id)
        {
            var field = await _dbContext.CategoryFields.FirstOrDefaultAsync(c1 => c1.Id == id);
            if (field == null)
                return BadRequest("Field not found");

            return new ObjectResult(field);
        }

        [HttpDelete("DeleteFieldById")]
        public async Task<IActionResult> DeleteFieldById([FromQuery] int id)
        {
            var field = await _dbContext.CategoryFields.FirstOrDefaultAsync(c => c.Id == id);
            if (field == null)
                return BadRequest("Field not found");

            _dbContext.CategoryFields.Remove(field);
            await _dbContext.SaveChangesAsync();

            return new ObjectResult(await _dbContext.CategoryFields.ToListAsync());
        }

        [HttpPost("PostField")]
        public async Task<IActionResult> PostField([FromBody] CategoryFieldEditDto FieldDto)
        {
            var field = new CategoryField
            {
                FieldName = FieldDto.FieldName
            };

            if (await _dbContext.CategoryFields.FirstOrDefaultAsync(c => c.FieldName == field.FieldName) != null)
                return BadRequest("This FieldName already exists");

            await _dbContext.CategoryFields.AddAsync(field);
            await _dbContext.SaveChangesAsync();

            return new ObjectResult(await _dbContext.CategoryFields.ToListAsync());
        }

        [HttpPut("PutField")]
        public async Task<IActionResult> PutField([FromQuery] int id, [FromBody] CategoryFieldEditDto FieldDto)
        {
            var field = await _dbContext.CategoryFields.FirstOrDefaultAsync(f => f.Id == id);

            if (field == null)
                return BadRequest(Results.BadRequest("Сategory not exist"));

            field.FieldName = FieldDto.FieldName;

            await _dbContext.SaveChangesAsync();
            return new ObjectResult(field);
        }
    }
}