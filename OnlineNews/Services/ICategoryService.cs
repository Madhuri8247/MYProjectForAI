using OnlineNews.Models.Database;

namespace OnlineNews.Service
{
    public interface ICategoryService
    {
        void CreateCategory(Category category);
        List<Category> GetAllCategory();
        void DeleteCategory(int id);
        Category GetCategory(int id);
        bool Edit(int id, Category updatedCategory);
        
    }
}
