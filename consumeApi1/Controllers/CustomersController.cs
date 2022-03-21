using consumeApi1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace consumeApi1.Controllers
{
    public class CustomersController : Controller
    {

        public IActionResult Index()
        {
            var ntoken = HttpContext.Session.GetString("JWToken");
            IEnumerable<Customers> logs = null;
            using (var log_ = new HttpClient())
            {
                log_.BaseAddress = new Uri("https://localhost:7109/api/");
                //log_.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ntoken);
                var responseTask = log_.GetAsync("Customers");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readLog = result.Content.ReadAsAsync<IList<Customers>>();
                    readLog.Wait();
                    logs = readLog.Result;
                }
                else
                {
                    logs = Enumerable.Empty<Customers>();
                    ModelState.AddModelError(string.Empty, "Server Error");
                }

            }

            return View(logs);
        }


        public async Task<IActionResult> GetCustomers(string id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:7109/api/Customers/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {

                        var readLog = response.Content.ReadAsStringAsync();
                        readLog.Wait();
                        var token_jwt = readLog.Result;
                        var root = JsonConvert.DeserializeObject<Customers>(token_jwt);

                        using (var response2 = await httpClient.GetAsync($"https://localhost:7109/api/CustomerAddresses?id={id}"))
                        {
                            if (response2.IsSuccessStatusCode)
                            {
                                var readLog2 = response2.Content.ReadAsAsync<IList<AddressCustomer>>();
                                //var readLog2 = response2.Content.ReadAsStringAsync();
                                readLog2.Wait();
                                var token_jwt2 = readLog2.Result;
                                //var root2 = JsonConvert.DeserializeObject<AddressCustomer>(token_jwt2);
                                var customer = token_jwt2;
                                ViewBag.customeraddress = customer;
                            }
                        }


                        return View(root);
                    }
                    else
                    {

                        ViewBag.Message = "Error";
                        return Redirect("~/Account/ErrorRegister");

                    }


                }
            }
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Customers user)
        {
            Customers Userx = new Customers();
            Userx.Name = user.Name;
            Userx.Email = user.Email;
            Userx.Phone = user.Phone;
            Userx.Addby = user.Addby;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Userx), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7109/api/Customers", content))
                {
                    //IEnumerable<tokenjwt> logs = null;
                    if (response.IsSuccessStatusCode)
                    {


                        var readLog = response.Content.ReadAsStringAsync();
                        readLog.Wait();
                        var token_jwt = readLog.Result;
                        var root = JsonConvert.DeserializeObject<Customers>(token_jwt);
                        var id = root.Id;

                        return Redirect($"~/Customers/GetCustomers/{id}");
                    }
                    else
                    {

                        ViewBag.Message = "Error";
                        return Redirect("~/Account/ErrorRegister");

                    }


                }
            }
        }


        [HttpGet]
        public IActionResult RegisterAddress(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAddress(AddressCustomer user)
        {

            AddressCustomer Userx = new AddressCustomer();
            Userx.Idcustomer = user.Idcustomer;
            Userx.AddressType = 1;
            Userx.Country = user.Country;
            Userx.State = user.State;
            Userx.City = user.City;
            Userx.Address = user.Address;
            Userx.Adby = "test";

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Userx), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7109/api/CustomerAddresses", content))
                {
                    //IEnumerable<tokenjwt> logs = null;
                    if (response.IsSuccessStatusCode)
                    {

                        var readLog = response.Content.ReadAsStringAsync();
                        readLog.Wait();
                        var token_jwt = readLog.Result;
                        var root = JsonConvert.DeserializeObject<AddressCustomer>(token_jwt);
                        var id = root.Idcustomer;

                        return Redirect($"~/Customers/GetCustomerAddress?id={id}");
                    }
                    else
                    {

                        ViewBag.Message = "Error";
                        return Redirect("~/Account/ErrorRegister");

                    }


                }
            }
        }

        public async Task<IActionResult> GetCustomerAddress(string id)
        {
            IEnumerable<AddressCustomer> ca = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:7109/api/CustomerAddresses?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var readLog = response.Content.ReadAsAsync<IList<AddressCustomer>>();
                        readLog.Wait();
                        ca = readLog.Result;
                                //Get Customer info
                                using (var response2 = await httpClient.GetAsync($"https://localhost:7109/api/Customers/{id}"))
                                {
                                    if (response2.IsSuccessStatusCode)
                                    {
                                        ///var readLog2 = response2.Content.ReadAsAsync<IList<Customers>>();
                                        var readLog2 = response2.Content.ReadAsStringAsync();
                                        readLog2.Wait();
                                        var token_jwt2 = readLog2.Result;
                                        var root2 = JsonConvert.DeserializeObject<Customers>(token_jwt2);
                                        var customer = root2.Name;
                                        ViewBag.customer = customer;
                                    }
                                }

                        return View(ca); 
                    }
                    else
                    {

                        ViewBag.Message = "Error";
                        return Redirect("~/Account/ErrorRegister");

                    }


                }
            }
        }


        //SEARCH CUSTOMERS
        public IActionResult Search(string id)
        {
            var ntoken = HttpContext.Session.GetString("JWToken");
            IEnumerable<Customers> logs = null;
            using (var log_ = new HttpClient())
            {
                log_.BaseAddress = new Uri("https://localhost:7109/api/");
                //log_.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ntoken);
                var responseTask = log_.GetAsync($"Account?id={id}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readLog = result.Content.ReadAsAsync<IList<Customers>>();
                    readLog.Wait();
                    logs = readLog.Result;
                }
                else
                {
                    logs = Enumerable.Empty<Customers>();
                    ModelState.AddModelError(string.Empty, "Server Error");
                }

            }

            return View(logs);
        }

    }
}
