using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context db;
        public HomeController(Context context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            XmlDocument doc = new();
            doc.Load("http://partner.market.yandex.ru/pages/help/YML.xml");
            string targetNodeName = "offer";

            XmlNodeList nodes = doc.GetElementsByTagName(targetNodeName);
            if (nodes.Count == 0)
            {
                ViewBag.Message = $"No nodes \"{targetNodeName}\" found in the XML document.";
                return View();
            }
            else
            {
                List<Offer> offers = new();

                foreach (XmlNode offerNode in nodes)
                {
                    Offer offer = new();

                    offer.VendorCode = offerNode.Attributes?["id"]?.Value;
                    offer.Description = offerNode.SelectSingleNode("description")?.InnerText;

                    var priceNode = offerNode.SelectSingleNode("price");
                    if (priceNode != null && int.TryParse(priceNode.InnerText, out int price))
                    {
                        offer.Price = price;
                    }
                    else
                    {
                        offer.Price = null;
                    }

                    offers.Add(offer);
                }

                Offer? targetOffer = offers.FirstOrDefault(o => o.VendorCode == "12344");

                Offer? existingOffer = null;
                if (targetOffer != null)
                {
                    existingOffer = db.Offers.FirstOrDefault(o => o.VendorCode == targetOffer.VendorCode);

                    if (existingOffer == null)
                    {
                        db.Offers.Add(targetOffer);
                        db.SaveChanges();
                    }
                    else
                    {
                        Offer viewModel = new()
                        {
                            VendorCode = targetOffer.VendorCode,
                            Description = targetOffer.Description,
                            Price = targetOffer.Price,
                            IsAlreadyInDB = true
                        };
                        return View(viewModel);
                    }
                }
                return View(targetOffer);
            }
        }
    }
}