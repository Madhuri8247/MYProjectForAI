using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
//using OnlineNews.Migrations;
using OnlineNews.Models.Database;

using OnlineNews.Interfaces;



namespace OnlineNews.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService (ApplicationDbContext db)
        {
            _db = db;
            
        }
        public void CreateCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }
        public List<Category> GetAllCategory()
        {
            var categories = _db.Categories.ToList();
            return categories;
        }

        public void DeleteCategory(int id )
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
            }

        }
        public Category GetCategory(int id)
        {
            /*var CategoryId =*/
            return  _db.Categories.FirstOrDefault(c => c.Id == id);
            
        }

        public bool Edit (int id,  Category Updatecategory)
        {
            var category = _db.Categories.FirstOrDefault(category => category.Id == id);
            if (category == null)
            {
                return false;
            }
            //category.Articles = Updatecategory.Articles;
            category.Name = Updatecategory.Name;
            _db.SaveChanges ();
            return true;
         

        }
        
    }
}
