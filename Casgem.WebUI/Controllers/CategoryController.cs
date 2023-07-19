using Casgem.WebUI.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Casgem.WebUI.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public CategoryController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client=_httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:44319/api/Category/getCategoryList");
			if (responseMessage .IsSuccessStatusCode)
			{
				var jsonData=await responseMessage.Content.ReadAsStringAsync();
				var values=JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpGet]
		public IActionResult AddCategory()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddCategory(CreateCategoryDto category)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData=JsonConvert.SerializeObject(category);
			StringContent content = new StringContent(jsonData,Encoding.UTF8,"application/json");
			var responseMessage=await client.PostAsync("https://localhost:44319/api/Category/addCategory",content);
			if (responseMessage .IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}


            return View();
		}

		
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var client = _httpClientFactory.CreateClient();
			//var responseMessage = await client.DeleteAsync($"https://localhost:44319/api/Category/deleteCategory/{id}");
			var responseMessage = await client.GetAsync($"https://localhost:44319/api/Category/deleteCategory/{id}");
			if (responseMessage .IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateCategory(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:44319/api/Category/getCategory/{id}");
			if (responseMessage .IsSuccessStatusCode)
			{
				var jsonData=await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateCategoryDto>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCategory(UpdateCategoryDto category)
		{
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:44319/api/Category/updateCategory", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

	}
}
