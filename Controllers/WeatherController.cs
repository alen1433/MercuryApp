using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace MercuryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        //async is better because if it takes a long time to get the data, we can still run other programs, but not in this case, since there are no other programs
        public async Task<IActionResult> Meteo()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://www.meteo.si");
                    var response = await client.GetAsync($"/uploads/probase/www/observ/surface/text/sl/observation_si_text.html");
                    response.EnsureSuccessStatusCode();
                    //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

                    var result = await response.Content.ReadAsStringAsync();
                    //Regex to remove the HTML tags that we get when we get the data as string
                    return Ok(Regex.Replace(result, "<[^>]*(>|$)[\n\r]+", String.Empty));
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting data from Meteo.si");
                }
            }
        }
            //public HttpResponseMessage Get()
            //{
            //    var response = new HttpResponseMessage();
            //    System.Net.WebRequest req = System.Net.WebRequest.Create("http://www.meteo.si/uploads/probase/www/observ/surface/text/sl/observation_si_text.html");
            //    using (System.Net.WebResponse resp = req.GetResponse())
            //    using (System.IO.StreamReader sr =
            //                new System.IO.StreamReader(resp.GetResponseStream()))
            //        response.Content = new StringContent(sr.ReadToEnd().Trim());
            //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            //    return response;
            //}
        }
}
