using FinancialTracker.Interfaces;
using FinancialTracker.Entities;

namespace FinancialTracker.Services
{
    public class CategoryService : ICategoryService
    {
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
            if (_categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Category already exists");
            }

            var category = new Category(++_categoryId, name);
            _categories.Add(category);
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
