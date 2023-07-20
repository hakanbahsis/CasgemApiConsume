using Casgem.WebUI.Dtos.CategoryDtos;
using Casgem.WebUI.Dtos.CustomerDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Casgem.WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:44319/api/Customer/getCustomerList");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCustomerDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(CreateCustomerDto customer)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(customer);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:44319/api/Customer/addCustomer", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();
           // var responseMessage = await client.GetAsync($"https://localhost:44319/api/Customer/deleteCustomer/{id}");
            var responseMessage = await client.DeleteAsync($"https://localhost:44319/api/Customer/deleteCustomer/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var client= _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:44319/api/Customer/getCustomer/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData=await responseMessage.Content.ReadAsStringAsync();
                var values=JsonConvert.DeserializeObject<UpdateCustomerDto>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto customer)
        {
            var client= _httpClientFactory.CreateClient();
            var jsonData=JsonConvert.SerializeObject(customer);
            var content=new StringContent(jsonData,Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:44319/api/Customer/updateCustomer", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
