using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OmarSupportClient.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImagesController : ApiController
    {
        // GET: api/Images
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Images/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Images

        [Route("api/images/postimages")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                int index = 0;
                string primaryFilePath = "";
                int primaryFileIndex = Convert.ToInt32(HttpContext.Current.Request.Params["PrimaryFileIndex"]);
                foreach (string file in HttpContext.Current.Request.Files)
                {


                    var postedFile = HttpContext.Current.Request.Files[file];
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
                    var filePath = HttpContext.Current.Server.MapPath("~/Images" + fileName);
                    postedFile.SaveAs(filePath);
                    if (index == primaryFileIndex)
                    {
                        primaryFilePath = filePath;
                    }
                    index++;
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // PUT: api/Images/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Images/5
        public void Delete(int id)
        {
        }
    }
}
