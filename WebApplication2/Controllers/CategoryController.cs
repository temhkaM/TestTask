using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.DtoModels;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;

        public CategoryController(ApplicationContext db)
        {
            _dbContext = db;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _dbContext.Categories.Include(p => p.CategoryFields).ToListAsync();
            return new ObjectResult(categories);
        }
        
        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById([FromQuery] int id)
        {
            var category = await _dbContext.Categories.Include(c => c.CategoryFields).FirstOrDefaultAsync(c1 => c1.Id == id);
            if (category == null)
                return BadRequest("Category not found");

            return new ObjectResult(category);
        }

        [HttpDelete("DeleteCategoryById")]
        public async Task<IActionResult> DeleteCategoryById([FromQuery] int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return BadRequest("category not found");

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return new ObjectResult(await _dbContext.Categories.Include(p => p.CategoryFields).ToListAsync());
        }
        
        [HttpPost("PostCategory")]
        public async Task<IActionResult> PostCategory([FromBody] CategoryEditDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            if (await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == category.Name) != null)
                return BadRequest("This Category Name already exists");

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return new ObjectResult(await _dbContext.Categories.Include(p => p.CategoryFields).ToListAsync());
        }
        
        [HttpPut("PutCategory")]
        public async Task<IActionResult> PutCategory([FromQuery] int id, [FromBody] CategoryEditDto CategoryData)
        {
            var category = await _dbContext.Categories.Include(c => c.CategoryFields).FirstOrDefaultAsync(c1 => c1.Id == id);

            if (category == null)
                return BadRequest(Results.BadRequest("Сategory not exist"));

            if (category.Name == null)
                return BadRequest(Results.BadRequest("Category Name not exist"));
            category.Name = CategoryData.Name;
            category.Description = CategoryData.Description;

            await _dbContext.SaveChangesAsync();
            return new ObjectResult(category);
        }

        [HttpPut("AddCategoryField")]
        public async Task<IActionResult> AddField([FromQuery] int categoryId, [FromQuery] int categoryFieldId)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            var categoryField = await _dbContext.CategoryFields.FirstOrDefaultAsync(x => x.Id == categoryFieldId);

            if (category == null)
                return BadRequest(Results.BadRequest("Category not exist"));

            if (categoryField == null)
                return BadRequest(Results.BadRequest("CategoryField not exist"));

            if (category.CategoryFields.FirstOrDefault(x => x.Id == categoryFieldId) != null)
                return BadRequest(Results.BadRequest("CategoryField is already added"));

            category.CategoryFields.Add(categoryField);
            
            await _dbContext.SaveChangesAsync();

            return new ObjectResult(category);
        }

        [HttpDelete("RemoveCategoryField")]
        public async Task<IActionResult> RemoveField([FromQuery] int categoryId, [FromQuery] int categoryFieldId)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            var categoryField = await _dbContext.CategoryFields.FirstOrDefaultAsync(x => x.Id == categoryFieldId);

            if (category == null)
                return BadRequest(Results.BadRequest("Category not exist"));

            if (categoryField == null || category.CategoryFields.FirstOrDefault(x => x.Id == categoryFieldId) == null)
                return BadRequest(Results.BadRequest("CategoryField not exist"));

            category.CategoryFields.Remove(categoryField);

            await _dbContext.SaveChangesAsync();

            return new ObjectResult(category);
        }
    }
}