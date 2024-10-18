using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Configuration.Permissions;
using System.Collections.Generic;

namespace ServiceHost.Areas.Adminstration.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        public ProductSearchModel SearchModel;
        public List<ProductViewModel> Products;
        public SelectList ProductCategories;

        private readonly IProductApplication _ProductApplication;
        private readonly IProductCategoryApplication _ProductCategoryApplication;

        public IndexModel(IProductApplication ProductApplication, IProductCategoryApplication productCategoryApplication)
        {
            _ProductApplication = ProductApplication;
            _ProductCategoryApplication = productCategoryApplication;
        }

        [NeedsPermission(ShopPermission.ListProducts)]
        public void OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new SelectList(_ProductCategoryApplication.GetProductCategories(), "Id", "Name");
            Products = _ProductApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = _ProductCategoryApplication.GetProductCategories()
            };
            return Partial("./Create", command);
        }

        [NeedsPermission(ShopPermission.CreateProducts)]
        public JsonResult OnPostCreate(CreateProduct command)
        {
            var result = _ProductApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var product = _ProductApplication.GetDetails(id);
            product.Categories = _ProductCategoryApplication.GetProductCategories();
            return Partial("Edit", product);
        }

        [NeedsPermission(ShopPermission.EditProducts)]
        public JsonResult OnPostEdit(EditProduct command)
        {
            var result = _ProductApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
