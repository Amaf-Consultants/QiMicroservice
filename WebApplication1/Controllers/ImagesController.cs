using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public string GetImage()
        {
            var imgCollection = GetImages().Result; // Get Images detail from collection
            return JsonConvert.SerializeObject(imgCollection);
           // return "value";
        }

        //
        private async Task<List<Image>> GetImages(int pageNumber=1, int recount =20)
        {
            string URL = "https://5ad8d1c9dc1baa0014c60c51.mockapi.io/api/br/v1/magic/";
            Task<List<Image>> task= Task<List<Image>>.Factory.StartNew(() =>
                {
                    List<Image> imgList = new List<Image>();
                    int startCounter = pageNumber == 1 ? 0 : ((pageNumber -1) * recount) + 1; 
                    int endCounter = pageNumber == 1 ? recount : pageNumber * recount;
                    using (var client = new WebClient()) //WebClient  
                    {
                        for(var i = startCounter; i < endCounter; i++)
                        {
                            client.Headers.Add("Content-Type:application/json"); //Content-Type  
                            client.Headers.Add("Accept:application/json");
                            //string finalURL = URL + i.ToString()
                            try
                            {
                                var result = client.DownloadString(URL + i.ToString());
                                Console.WriteLine(Environment.NewLine + result);
                                imgList.Add(JsonConvert.DeserializeObject<Image>(result));
                            }
                           catch(WebException e)
                            {
                                Console.WriteLine(e.Message);
                                imgList.Add(null);
                            };

                        }

                        return imgList; 
                    }
                });
            
            var t = await task;
            return t;
        }
    }

    class Image
    {
        public string id { get; set; }
        public string createdAt { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }

        //public int Id { get; set; }
        //public string Exception { get; set; }
        //public int Status { get; set; }
        //public Boolean IsCanceled { get; set; }
        //public Boolean IsCompleted { get; set; }
        //public Boolean IsCompletedSuccessfully { get; set; }
        //public int CreationOptions { get; set; }
        //public string AsyncState { get; set; }
        //public Boolean IsFaulted { get; set; }

    }
}
//{"Result":{"id":"12","createdAt":"1524158544","name":"name 12","imageUrl":"https://unsplash.it/500?image=12"},"Id":62,"Exception":null,"Status":5
//,"IsCanceled":false,"IsCompleted":true,"IsCompletedSuccessfully":true,"CreationOptions":0,"AsyncState":null,"IsFaulted":false}