using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessModel;

namespace DataAccessLayer
{
    // Brand Data Access Object
    public class BrandDAO
    {
        private readonly Team13QlbhContext _context;

        public BrandDAO(Team13QlbhContext context)
        {
            _context = context;
        }

        // Retrieve all brands
        public List<Brand> GetAllBrands()
        {
            return _context.Set<Brand>()
                .Include(b => b.Products)
                .ToList();
        }

        // Get a specific brand by its ID
        public Brand GetBrandById(int brandId)
        {
            return _context.Set<Brand>()
                .Include(b => b.Products)
                .FirstOrDefault(b => b.BrandId == brandId);
        }

        // Add a new brand
        public void AddBrand(Brand brand)
        {
            _context.Set<Brand>().Add(brand);
            _context.SaveChanges();
        }

        // Update an existing brand
        public void UpdateBrand(Brand brand)
        {
            _context.Set<Brand>().Update(brand);
            _context.SaveChanges();
        }

        // Delete a brand by its ID
        public void DeleteBrand(int brandId)
        {
            var brand = GetBrandById(brandId);
            if (brand != null)
            {
                _context.Set<Brand>().Remove(brand);
                _context.SaveChanges();
            }
        }

        // Find brands by name (partial match)
        public List<Brand> GetBrandsByName(string brandName)
        {
            return _context.Set<Brand>()
                .Where(b => b.BrandName.Contains(brandName))
                .Include(b => b.Products)
                .ToList();
        }
    }

    // Category Data Access Object
    public class CategoryDAO
    {
        private readonly DbContext _context;

        public CategoryDAO(DbContext context)
        {
            _context = context;
        }

        // Retrieve all categories
        public List<Category> GetAllCategories()
        {
            return _context.Set<Category>()
                .Include(c => c.Products)
                .ToList();
        }

        // Get a specific category by its ID
        public Category GetCategoryById(int categoryId)
        {
            return _context.Set<Category>()
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CategoryId == categoryId);
        }

        // Add a new category
        public void AddCategory(Category category)
        {
            _context.Set<Category>().Add(category);
            _context.SaveChanges();
        }

        // Update an existing category
        public void UpdateCategory(Category category)
        {
            _context.Set<Category>().Update(category);
            _context.SaveChanges();
        }

        // Delete a category by its ID
        public void DeleteCategory(int categoryId)
        {
            var category = GetCategoryById(categoryId);
            if (category != null)
            {
                _context.Set<Category>().Remove(category);
                _context.SaveChanges();
            }
        }

        // Find categories by name (partial match)
        public List<Category> GetCategoriesByName(string categoryName)
        {
            return _context.Set<Category>()
                .Where(c => c.CategoryName.Contains(categoryName))
                .Include(c => c.Products)
                .ToList();
        }
    }

    // Product Data Access Object
    public class ProductDAO
    {
        private readonly DbContext _context;

        public ProductDAO(DbContext context)
        {
            _context = context;
        }

        // Retrieve all products with related entities
        public List<Product> GetAllProducts()
        {
            return _context.Set<Product>()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.OrderDetails)
                .ToList();
        }

        // Get a specific product by its ID
        public Product GetProductById(int productId)
        {
            return _context.Set<Product>()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.OrderDetails)
                .FirstOrDefault(p => p.ProductId == productId);
        }

        // Add a new product
        public void AddProduct(Product product)
        {
            _context.Set<Product>().Add(product);
            _context.SaveChanges();
        }

        // Update an existing product
        public void UpdateProduct(Product product)
        {
            _context.Set<Product>().Update(product);
            _context.SaveChanges();
        }

        // Delete a product by its ID
        public void DeleteProduct(int productId)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                _context.Set<Product>().Remove(product);
                _context.SaveChanges();
            }
        }

        // Advanced search method for products
        public List<Product> SearchProducts(
            string productName = null,
            int? brandId = null,
            int? categoryId = null,
            int? minPrice = null,
            int? maxPrice = null)
        {
            // Start with a base query including related entities
            IQueryable<Product> query = _context.Set<Product>()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.OrderDetails);

            // Apply filters based on provided parameters
            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(p => p.ProductName.Contains(productName));
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            return query.ToList();
        }

        // Find products by brand
        public List<Product> GetProductsByBrand(int brandId)
        {
            return _context.Set<Product>()
                .Where(p => p.BrandId == brandId)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToList();
        }

        // Find products by category
        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Set<Product>()
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToList();
        }
    }
}