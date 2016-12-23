using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using AutosDataApi.Models;
using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace AutosDataApi.Controllers
{
    public class AutoController : ApiController
    {
        [Route("auto/getbrands")]
        public List<Brand> GetBrands()
        {
            List<Brand> brands = new List<Brand>();
            string urlAddress = "http://www.auto-data.net/tr/";


            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(GetSiteHtml(urlAddress));
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//a[@class='marki_blok']"))
            {
                if (node.ChildNodes.Count != 3) continue;
                Brand brand = new Brand();
                brand.Name = node.InnerText;
                //brand.Url = "http://www.auto-data.net/tr/" + node.Attributes["href"].Value.Replace("&amp;", "&");
                brand.Id = node.Attributes["href"].Value.Substring(node.Attributes["href"].Value.IndexOf("id=") + 3, node.Attributes["href"].Value.Length - node.Attributes["href"].Value.IndexOf("id") - 3);
                brands.Add(brand);
            }



            return brands;

        }
        [Route("auto/getmodels")]
        public List<Model> GetModels(int id)
        {
            List<Model> models = new List<Model>();
            string urlAddress = "http://www.auto-data.net/tr/?f=showModel&marki_id=" + id;


            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(GetSiteHtml(urlAddress));
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//a[@class='modeli']"))
            {
                if (node.ChildNodes.Count != 5) continue;
                Model model = new Model();
                model.Name = node.ChildNodes[1].InnerText;
                model.Image = "http://www.auto-data.net/" + node.ChildNodes[3].Attributes["src"].Value;
                model.Id = node.Attributes["href"].Value.Substring(node.Attributes["href"].Value.IndexOf("id=") + 3, node.Attributes["href"].Value.Length - node.Attributes["href"].Value.IndexOf("id") - 3);
                models.Add(model);
            }



            return models;

        }

        [Route("auto/getsubmodels")]
        public List<SubModel> GetSubModels(int id)
        {
            List<SubModel> subModels = new List<SubModel>();
            string urlAddress = "http://www.auto-data.net/tr/?f=showSubModel&modeli_id=" + id;


            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(GetSiteHtml(urlAddress));
            HtmlNodeCollection table = htmlDoc.DocumentNode.SelectNodes("//table/tr/td/a");
            string headerName = "";
            foreach (HtmlNode node in table)
            {
                SubModel model = new SubModel();
                if (String.IsNullOrWhiteSpace(node.InnerText)) continue;
                if(headerName != node.ParentNode.ParentNode.ParentNode.PreviousSibling.InnerText)
                {
                    headerName = node.ParentNode.ParentNode.ParentNode.PreviousSibling.InnerText;
                    subModels.Add(new SubModel() { IsHeader = true, Name = headerName });
                }


                model.Name = node.InnerText;
                model.Id = node.Attributes["href"].Value.Substring(node.Attributes["href"].Value.IndexOf("id=") + 3, node.Attributes["href"].Value.Length - node.Attributes["href"].Value.IndexOf("id") - 3);
                subModels.Add(model);
            }



            return subModels;

        }
        public string GetSiteHtml(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
                return data;
            }
            return "";
        }
        [Route("auto/getcar")]
        public CarModel GetCar(int id)
        {
            CarModel car = new CarModel();
            string urlAddress = "http://www.auto-data.net/tr/?f=showCar&car_id=" + id;


            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(GetSiteHtml(urlAddress));
            HtmlNodeCollection table = htmlDoc.DocumentNode.SelectNodes("//table/tr");
            Dictionary<string, string> carInfo = new Dictionary<string, string>();
            foreach (HtmlNode node in table)
            {
                carInfo.Add(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText);
            }
            car.Info = carInfo;
            HtmlNodeCollection images = htmlDoc.DocumentNode.SelectNodes("//div[@class='imagescar']/a/img");
            List<string> imageList = new List<string>();
            if (images != null)
                if (images[0].Attributes["src"] != null)
                {
                    string imgDetailUrl = "http://www.auto-data.net/tr/" + images[0].ParentNode.Attributes["href"].Value.Replace("&amp;", "&");
                    HtmlDocument htmlDoc2 = new HtmlDocument();
                    htmlDoc2.LoadHtml(GetSiteHtml(imgDetailUrl));
                    HtmlNodeCollection images2 = htmlDoc2.DocumentNode.SelectNodes("//div[@id='center']/a");
                    List<HtmlNode> images3 = images2.Where(h => h.Attributes["href"] != null && h.Attributes["title"] != null).Take(5).ToList();
                    foreach (var item in images3)
                    {
                        string imgDetailUrl2 = "http://www.auto-data.net/tr/" + item.Attributes["href"].Value.Replace("&amp;", "&");
                        HtmlDocument htmlDoc3 = new HtmlDocument();
                        htmlDoc3.LoadHtml(GetSiteHtml(imgDetailUrl2));
                        HtmlNode imagee = htmlDoc3.DocumentNode.SelectSingleNode("//div[@class='image-wrapper']/img");
                        imageList.Add("http://www.auto-data.net" + imagee.Attributes["src"].Value.Replace("&amp;", "&"));
                    }
                }
            //imageList.Add("http://www.auto-data.net" + item.Attributes["src"].Value.Replace("small", "big"));

            car.Images = imageList;



            return car;
        }
    }
}
