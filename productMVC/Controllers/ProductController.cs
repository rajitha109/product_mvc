using productMVC.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;

namespace productMVC.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source=DESKTOP-6U7CQKC;Initial Catalog=MvcDemoDb;Integrated Security=True";
        // GET: Product
        public ActionResult Index()
        {
            
            return View();


        }
        //Get Product Details
        public ActionResult GetProducts()
        {
            var productsData = new List<ProductModel>();
            using (SqlConnection sqlConnect = new SqlConnection(connectionString))
            {
                sqlConnect.Open();
                StringBuilder returnsValue = new StringBuilder();
                SqlCommand sqlCmd = new SqlCommand("getAllProduct", sqlConnect);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlData = new SqlDataAdapter(sqlCmd);

                using (var xmlReader = sqlCmd.ExecuteXmlReader())
                {
                    while (xmlReader.Read())
                    {
                        string xmlString = xmlReader.ReadOuterXml();
                        returnsValue.Append(xmlString);
                    }
                }
                var data = returnsValue.ToString();
                var xmlDocument = XDocument.Parse(data);
                var productsXml = xmlDocument.Elements("Products").Elements("Product");

                productsData = productsXml.Select((s) =>
                new ProductModel()
                {
                    ProductID = (s.Attribute("ProductID") != null) ? int.Parse(s.Attribute("ProductID").Value) : 0,
                    ProductName = s.Attribute("ProductName").Value.ToString(),
                    Price = decimal.Parse(s.Attribute("Price").Value),
                    Count = int.Parse(s.Attribute("Counter").Value)
                }).ToList();


                sqlConnect.Close();
            }
            return Json(productsData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllDetails()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }


        public ActionResult Test2()
        {
            return View();
        }


        //Get Product and Seller Data
        public ActionResult GetData()
        {
            var productsData = new List<ProductModel>();
            var sellersData = new List<SellerModel>();
            using (SqlConnection sqlConnect = new SqlConnection(connectionString))
            {
                sqlConnect.Open();
                StringBuilder returnsValue = new StringBuilder();
                SqlCommand sqlCmd = new SqlCommand("getAllDetails", sqlConnect);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlData = new SqlDataAdapter(sqlCmd);

                using (var xmlReader = sqlCmd.ExecuteXmlReader())
                {
                    while (xmlReader.Read())
                    {
                        string xmlString = xmlReader.ReadOuterXml();
                        returnsValue.Append(xmlString);
                    }
                }
                var data = returnsValue.ToString();
                var xmlDocument = XDocument.Parse(data);
                var productsXml = xmlDocument.Elements("ExportList").Elements("byProduct").Elements("Product");

                productsData = productsXml.Select((s) =>
                    new ProductModel()
                    {
                        ProductID = (s.Attribute("ProductID") != null) ? int.Parse(s.Attribute("ProductID").Value) : 0,
                        ProductName = s.Attribute("ProductName").Value.ToString(),
                        Price = decimal.Parse(s.Attribute("Price").Value),
                        Count = int.Parse(s.Attribute("Counter").Value)
                    }).ToList();

                var sellersXml = xmlDocument.Elements("ExportList").Elements("bySeller").Elements("Seller");

                sellersData = sellersXml.Select((s) =>
                    new SellerModel()
                    {
                        Id = (s.Attribute("Id") != null) ? int.Parse(s.Attribute("Id").Value) : 0,
                        Name = s.Attribute("Name").Value.ToString(),

                    }).ToList();
                sqlConnect.Close();

                var productViewList = new ProductViewModel
                {
                    ProductList = productsData,
                    SellerList = sellersData

                };


                return Json(productViewList, JsonRequestBehavior.AllowGet);


            }
        }

        


    }
}