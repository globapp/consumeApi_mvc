using consumeApi1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace consumeApi1.Controllers
{
    public class AccountController : Controller
    {
        public class tokenjwt
        {
            public string? token { get; set; }
            public string? expiration { get; set; }
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult ErrorRegister()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            RegisterModel Userx = new RegisterModel();
            Userx.Username  = user.Username;
            Userx.Email     = user.Username;
            Userx.Password  = user.Password;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Userx), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7109/api/account/register", content))
                {
                    //IEnumerable<tokenjwt> logs = null;
                    if (response.IsSuccessStatusCode)
                    {

                        var readLog = response.Content.ReadAsStringAsync();
                        readLog.Wait();
                        //var token_jwt = readLog.Result;
                        //var root = JsonConvert.DeserializeObject<tokenjwt>(token_jwt);
                        //var tk = root.token;
                        //var exp = root.expiration;
                        //HttpContext.Session.SetString("JWToken", tk);

                        //List<tokenjwt> ModeloJWT = JsonConvert.DeserializeObject<List<tokenjwt>>(token_jwt);

                        ViewBag.Message = "Welcome";

                        return Redirect("~/");
                    }
                    else
                    {

                        ViewBag.Message = "Error";
                        return Redirect("~/Account/ErrorRegister");

                    }


                }
            }
        }

        public async Task<IActionResult> LoginUser(LoginModel user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7109/api/account/login", content))
                {
                    //IEnumerable<tokenjwt> logs = null;
                    if (response.IsSuccessStatusCode)
                    {

                        var readLog = response.Content.ReadAsStringAsync();
                        readLog.Wait();
                        var token_jwt = readLog.Result;
                        var root = JsonConvert.DeserializeObject<tokenjwt>(token_jwt);
                        var tk = root.token;
                        var exp = root.expiration;
                        HttpContext.Session.SetString("JWToken", tk);


                        ViewBag.Message = "Welcome";

                        return Redirect("~/Dashboard");
                    }
                    else
                    {

                        ViewBag.Message = "Incorrect user or password";
                        return Redirect("~/");

                    }


                }
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
