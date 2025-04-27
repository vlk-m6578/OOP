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
                new Category("Services", true),
                new Category("Cafe", true),
                new Category("Entertainment", true)
            };

                _context.Categories.AddRange(systemCategories);
                _context.SaveChanges();
            }
        }

        private static readonly List<Category> _categories = new List<Category>
        {
            new Category(1, "Food", true),
            new Category(2, "Transport", true),
            new Category(3, "Housing", true),
            new Category(4, "Servicees", true),
            new Category(5, "Cafe", true),
            new Category(6, "Entertainment", true)
        };
        private static int _categoryId = 6;
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
            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null) return;

            if (category.IsSystemCategory)
            {
                throw new InvalidOperationException("Cannot delete system categories");
            }

            _categories.Remove(category);
        }
        public Category GetCategory(int categoryId) =>
        _categories.FirstOrDefault(c => c.Id == categoryId);

        public List<Category> GetAllCategories() => _categories;

        public List<Category> GetSystemCategories() =>
            _categories.Where(c => c.IsSystemCategory).ToList();

        public List<Category> GetUserCategories() =>
            _categories.Where(c => !c.IsSystemCategory).ToList();

    }
}
