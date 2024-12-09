using Microsoft.EntityFrameworkCore;
using ProductInventorySystem.Models;
using ProductInventorySystem.ViewModel;

namespace ProductInventorySystem.Repository
{
    public class ProductRepositoryImpl : IProductRepository
    {
        // Virtualbase Private Variable
        private readonly ProductInventoryDbContext _dbContext;

        // Dependency Injection
        public ProductRepositoryImpl(ProductInventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get All Products - ViewModel
        public async Task<IEnumerable<ProductVarientViewModel>> GetAllProductsViewModel()
        {
            try
            {
                if (_dbContext != null)
                {
                    // Use Linq for Join products, varients & Options
                    var products = await (from p in _dbContext.Products
                                          join pv in _dbContext.ProductVarients on p.Id equals pv.ProductId
                                          join v in _dbContext.VariantOptions on pv.Id equals v.VarientId
                                          where p.Active == true
                                          select new ProductVarientViewModel
                                          {
                                              ProductCode = p.ProductCode,
                                              ProductName = p.ProductName,
                                              VarientName = pv.VarientName,
                                              OptionValue = v.OptionValue,
                                              Stock = v.Stock
                                          }).ToListAsync();

                    return products;
                }
                // Return empty if null
                return new List<ProductVarientViewModel>();
            }
            catch (Exception)
            {
                // Exception handling
                throw;
            }
        }


        // Create New Product - ViewModel
        public async Task<Product> CreateProduct(CreateProductViewModel request, Guid userId)
        {
            // Create new product entity
            var product = new Product
            {
                Id = Guid.NewGuid(), // Explicitly set the Id
                ProductName = request.Name,
                // Generate unique product code using name and timestamp
                ProductCode = $"{request.Name.Substring(0, 3).ToUpper()}{DateTime.Now.Ticks % 1000:D3}",
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow,
                CreatedUserId = userId,
                Active = true
            };

            // Add product to context
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(); // Save to get the Product Id

            // Create variants for the product
            foreach (var variant in request.Variants)
            {
                var productVariant = new ProductVarient
                {
                    Id = Guid.NewGuid(), // Explicitly set the Id
                    ProductId = product.Id,
                    VarientName = variant.Name
                };

                _dbContext.ProductVarients.Add(productVariant);
                await _dbContext.SaveChangesAsync(); // Save to get the Variant Id

                // Create options for each variant
                foreach (var option in variant.Options)
                {
                    _dbContext.VariantOptions.Add(new VariantOption
                    {
                        Id = Guid.NewGuid(), // Explicitly set the Id
                        VarientId = productVariant.Id,
                        OptionValue = option,
                        Stock = 0 // Initialize stock to zero
                    });
                }
                // Save all changes to database
                await _dbContext.SaveChangesAsync();
            }
            return product;
        }


        // Add Stock +(Purchase)
        public async Task<bool> AddStock(StockManagementViewModel model)
        {
            var variantOption = await (from p in _dbContext.Products
                                       join pv in _dbContext.ProductVarients on p.Id equals pv.ProductId
                                       join vo in _dbContext.VariantOptions on pv.Id equals vo.VarientId
                                       where p.ProductCode == model.ProductCode
                                       && pv.VarientName == model.VarientName
                                       && vo.OptionValue == model.OptionValue
                                       select vo).FirstOrDefaultAsync();

            if (variantOption == null) return false;

            variantOption.Stock += model.Quantity;

            // Update total stock in product table
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == model.ProductCode);
            product.TotalStock += model.Quantity;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Remove Stock -(Sale)
        public async Task<bool> RemoveStock(StockManagementViewModel model)
        {
            var variantOption = await (from p in _dbContext.Products
                                       join pv in _dbContext.ProductVarients on p.Id equals pv.ProductId
                                       join vo in _dbContext.VariantOptions on pv.Id equals vo.VarientId
                                       where p.ProductCode == model.ProductCode
                                       && pv.VarientName == model.VarientName
                                       && vo.OptionValue == model.OptionValue
                                       select vo).FirstOrDefaultAsync();

            if (variantOption == null || variantOption.Stock < model.Quantity) return false;

            variantOption.Stock -= model.Quantity;

            // Update total stock in product table
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == model.ProductCode);
            product.TotalStock -= model.Quantity;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
    
}
