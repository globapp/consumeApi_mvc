using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using consumeApi1.Models;
using Microsoft.AspNetCore.Mvc;

namespace consumeApi1.Controllers
{
    public class LogsController : Controller
    {
        public IActionResult Index()
        {
            var ntoken = HttpContext.Session.GetString("JWToken");
            IEnumerable<UserLog> logs = null;
            using (var log_ = new HttpClient())
            {
                log_.BaseAddress = new Uri("https://localhost:7109/api/");
                log_.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ntoken);
                var responseTask = log_.GetAsync("userlogs");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readLog = result.Content.ReadAsAsync<IList<UserLog>>();
                    readLog.Wait();
                    logs = readLog.Result;
                }
                else
                {
                    logs = Enumerable.Empty<UserLog>();
                    ModelState.AddModelError(string.Empty, "Server Error");
                }

            }

            return View(logs);
        }
    }
}
