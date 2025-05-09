using FinancialTracker.Domain.Entities;

namespace FinancialTracker.Domain.Interfaces
{
    public interface ICategoryService
    {
        Category CreateUserCategory(string name);
        void DeleteCategory(int categoryId);
        Category GetCategory(int categoryId);
        List<Category> GetAllCategories();
        List<Category> GetSystemCategories();
        List<Category> GetUserCategories();
    }
}
