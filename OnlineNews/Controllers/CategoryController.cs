using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models.Database;
using OnlineNews.Service;

namespace OnlineNews.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View(_categoryService.GetAllCategory());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)

        {
            if (ModelState.IsValid)
            {
                _categoryService.CreateCategory(category);
                return RedirectToAction("CategorySuccess");
            }
            return View(category);
        }
        public IActionResult CategorySuccess()
        {
            return View();
        }
        public IActionResult RemovedCategory()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetCategory(id); 

            if (category == null)
            {
                return NotFound();
            }

            return View(category); 
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id); 
            return RedirectToAction("Index");
        }

    
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound(); 
            }

            return View(category); 
        }

        [HttpPost]
        public IActionResult EditCategory(int id, Category category)
        {
            if (ModelState.IsValid) 
            {
                var updateSuccessful = _categoryService.Edit(id, category); 

                if (!updateSuccessful)
                {
                    return NotFound(); 
                }

                return RedirectToAction("Index"); 
            }

            return View(category);
        }


        public IActionResult Details(int id)
        {
            var category = _categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        


    }
   
}
