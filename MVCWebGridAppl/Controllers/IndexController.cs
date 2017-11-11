using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using MVCWebGridAppl.Models;

namespace MVCWebGridAppl.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            IEnumerable<ResponseDataModel> Data = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:20342/api/");
                    var responseTask = client.GetAsync("DBDataApi");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readResponse = result.Content.ReadAsAsync<IList<ResponseDataModel>>();
                        readResponse.Wait();
                        Data = readResponse.Result;
                    }
                    else
                    {
                        Data = Enumerable.Empty<ResponseDataModel>();
                        ModelState.AddModelError("error", "Service Request Failed, Contact Helpdesk");
                    }
                }
                return View(Data);
            }
            catch (Exception) { return null;}
            finally
            {
                    
            }
        }
    }
}