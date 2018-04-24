namespace Falcon.API.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Falcon.API.Models;

    public class SkaController : ApiController
    {
        // GET: api/Ska/CheckAnswer/{name}
        [HttpGet]
        [ActionName("CheckAnswer")]
        public dynamic CheckAnswer([FromUri(Name = "key")]string name)
        {
            var data = SkaCache.GetAnswers();
            var result = (from r in data
                          where r.Name == name
                          select new
                          {
                              Type = r.Type.ToString(),
                              r.URL
                          }).FirstOrDefault();

            return result;
        }

        // GET: api/Ska
        public HttpResponseMessage Get()
        {
            var data = SkaCache.GetAnswers();
            var name = (from r in data
                        orderby Guid.NewGuid()
                        select r.Name).FirstOrDefault();

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(name);

            return response;
        }
    }
}