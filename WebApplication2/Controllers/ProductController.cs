using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Description;
using WebApplication2.Models;
using WebApplication2.Models.DtoModels;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;

        public ProductController(ApplicationContext db)
        {
            _dbContext = db;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _dbContext.Products.ToListAsync();

            List <ProductFieldDto> productsWithFields = new List<ProductFieldDto> ();

            foreach (var prod in products) {
                var categoryData = await _dbContext.Categories.FirstOrDefaultAsync(c1 => c1.Id == prod.CategoryId);
                var fieldData = await _dbContext.ProductCategoryFields.Where(pcf => pcf.ProductId == prod.Id).Include(pcf => pcf.CategoryField).ToListAsync();

                List<ProductCategoryFieldDto> fields = new List<ProductCategoryFieldDto>();
                foreach (var f in fieldData)
                {
                    fields.Add(new ProductCategoryFieldDto { CategoryFieldId = f.CategoryFieldId, CategoryField = f.CategoryField.FieldName, FieldValue=f.FieldValue });
                }

                productsWithFields.Add(new ProductFieldDto
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Price = prod.Price,
                    Description = prod.Description,
                    CategoryId = prod.CategoryId,
                    Category = new CategoryEditDto { Name = categoryData.Name, Description = categoryData.Description},
                    Fields = fields
                }); 
            }

            return new ObjectResult(productsWithFields);
        }

        [HttpGet("GetAllProductsFilterByCategory")]
        public async Task<IActionResult> GetAllProductsFilterByCategory(int filter)
        {
            var products = await _dbContext.Products.ToListAsync();

            List<ProductFieldDto> productsWithFields = new List<ProductFieldDto>();

            foreach (var prod in products)
            {
                if (prod.CategoryId == filter)
                {
                    var categoryData = await _dbContext.Categories.FirstOrDefaultAsync(c1 => c1.Id == prod.CategoryId);
                    var fieldData = await _dbContext.ProductCategoryFields.Where(pcf => pcf.ProductId == prod.Id).Include(pcf => pcf.CategoryField).ToListAsync();

                    List<ProductCategoryFieldDto> fields = new List<ProductCategoryFieldDto>();
                    foreach (var f in fieldData)
                    {
                        fields.Add(new ProductCategoryFieldDto { CategoryFieldId = f.CategoryFieldId, CategoryField = f.CategoryField.FieldName, FieldValue = f.FieldValue });
                    }

                    productsWithFields.Add(new ProductFieldDto
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        Description = prod.Description,
                        CategoryId = prod.CategoryId,
                        Category = new CategoryEditDto { Name = categoryData.Name, Description = categoryData.Description },
                        Fields = fields
                    });
                }
            }

            return new ObjectResult(productsWithFields);
        }

        [HttpGet("GetAllProductsFilterByField")]
        public async Task<IActionResult> GetAllProductsFilterByField(string filter)
        {
            var products = await _dbContext.Products.ToListAsync();

            List<ProductFieldDto> productsWithFields = new List<ProductFieldDto>();

            foreach (var prod in products)
            {
                var categoryData = await _dbContext.Categories.FirstOrDefaultAsync(c1 => c1.Id == prod.CategoryId);
                var fieldData = await _dbContext.ProductCategoryFields.Where(pcf => pcf.ProductId == prod.Id).Include(pcf => pcf.CategoryField).ToListAsync();

                List<ProductCategoryFieldDto> fields = new List<ProductCategoryFieldDto>();
                foreach (var f in fieldData)
                {
                    fields.Add(new ProductCategoryFieldDto { CategoryFieldId = f.CategoryFieldId, CategoryField = f.CategoryField.FieldName, FieldValue = f.FieldValue });
                }

                if (fields.Exists(f => f.FieldValue == filter))
                {
                    productsWithFields.Add(new ProductFieldDto
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        Description = prod.Description,
                        CategoryId = prod.CategoryId,
                        Category = new CategoryEditDto { Name = categoryData.Name, Description = categoryData.Description },
                        Fields = fields
                    });
                }
            }

            return new ObjectResult(productsWithFields);
        }

        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById([FromQuery] int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return BadRequest("Product not found");

            var categoryData = await _dbContext.Categories.FirstOrDefaultAsync(c1 => c1.Id == product.CategoryId);
            var fieldData = await _dbContext.ProductCategoryFields.Where(pcf => pcf.ProductId == id).Include(pcf => pcf.CategoryField).ToListAsync();

            List<ProductCategoryFieldDto> fields = new List<ProductCategoryFieldDto>();
            foreach (var f in fieldData)
            {
                fields.Add(new ProductCategoryFieldDto { CategoryFieldId = f.CategoryFieldId, CategoryField = f.CategoryField.FieldName, FieldValue = f.FieldValue });
            }

            var productDto = new ProductFieldDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Category = new CategoryEditDto { Name = categoryData.Name, Description = categoryData.Description },
                Fields = fields
            };
            //var product = await _dbContext.Products.Include(p => p.Category).ThenInclude(c => c.CategoryFields).FirstOrDefaultAsync(p1 => p1.Id == id);
            //if (product == null)
            //    return BadRequest("Product was not found");
            
            return new ObjectResult(productDto);
        }

        [HttpDelete("DeleteProductById")]
        public async Task<IActionResult> DeleteProductById([FromQuery] int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return BadRequest("Product was not found");

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("PostProduct")]
        public async Task<IActionResult> PostProduct([FromBody] ProductEditDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId,
                Category = await _dbContext.Categories.Include(c => c.CategoryFields).FirstOrDefaultAsync(c1 => c1.Id == productDto.CategoryId)
            };

            if (await _dbContext.Products.FirstOrDefaultAsync(c => c.Name == product.Name) != null)
                return BadRequest("This Product Name already exists");

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            foreach (var f in product.Category.CategoryFields)
            {
                var fieldValue = new ProductCategoryField
                {
                    ProductId = product.Id,
                    CategoryFieldId = f.Id,
                    CategoryField = f,
                    FieldValue = ""
                };

                await _dbContext.ProductCategoryFields.AddAsync(fieldValue);
                await _dbContext.SaveChangesAsync();
            }

            return new ObjectResult(product);
        }

        [HttpPut("PutProductById")]
        public async Task<IActionResult> PutProductById([FromQuery] int id, [FromBody] ProductEditDto ProductData)
        {
            var product = await _dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p1 => p1.Id == id);

            if (product == null)
                return BadRequest("Product was not found");

            if (ProductData.Name == null)
                return BadRequest("The Product Name is empty");
            product.Name = ProductData.Name;

            if (ProductData.Price == null)
                return BadRequest("The Product Price is empty");
            product.Price = ProductData.Price;

            product.Description = ProductData.Description;

            if (ProductData.CategoryId == null)
                return BadRequest("The Product CategoryId is empty");
            product.CategoryId = ProductData.CategoryId;

            await _dbContext.SaveChangesAsync();
            return await GetProductById(id);
        }

        [HttpPut("PutProductFields")]
        public async Task<IActionResult> PutProductById([FromQuery] int productId, [FromQuery] int categoryFieldId, [FromBody] string fieldValue )
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p1 => p1.Id == productId);

            if (product == null)
                return BadRequest("Product was not found");

            var categoryField = await _dbContext.CategoryFields.FirstOrDefaultAsync(p1 => p1.Id == categoryFieldId);

            if (categoryField == null)
                return BadRequest("CategoryField was not found");

            var productCategoryField = await _dbContext.ProductCategoryFields.FirstOrDefaultAsync(p1 => p1.ProductId == productId && p1.CategoryFieldId == categoryFieldId);

            if (productCategoryField == null)
                return BadRequest("productCategoryField was not found");
            
            productCategoryField.FieldValue = fieldValue;

            await _dbContext.SaveChangesAsync();

            return await GetProductById(productId);
        }



    }
}