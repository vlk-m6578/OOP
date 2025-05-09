using FinancialTracker.Interfaces;
using FinancialTracker.Entities;
using FinancialTracker.Data;

namespace FinancialTracker.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
            InitializeSystemCategories();
        }

        private void InitializeSystemCategories()
        {
            if (!_context.Categories.Any())
            {
                var systemCategories = new List<Category>
                {
                    new Category("Food", true),
                    new Category("Transport", true),
                    new Category("Housing", true),
                    new Category("Utilities", true),
                    new Category("Health", true),
                    new Category("Education", true)
                };

                _context.Categories.AddRange(systemCategories);
                _context.SaveChanges();
            }
        }
        public Category CreateUserCategory(string name)
        {
            if (_context.Categories.Any(c => c.Name == name))
                throw new ArgumentException("Category already exists");

            var category = new Category(name);
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }
        public void DeleteCategory(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null) return;
            if (category.IsSystemCategory)
                throw new InvalidOperationException("You can't delete a system category.");

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
        public Category GetCategory(int categoryId) =>
        _context.Categories.FirstOrDefault(c => c.Id == categoryId);

        public List<Category> GetAllCategories()
        {
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }

        public List<Category> GetSystemCategories()
        {
            return _context.Categories
                .Where(c => c.IsSystemCategory)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public List<Category> GetUserCategories()
        {
            return _context.Categories
                .Where(c => !c.IsSystemCategory)
                .OrderBy(c => c.Name)
                .ToList();
        }

    }
}
