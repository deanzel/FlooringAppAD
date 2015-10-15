using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.Models;

namespace FlooringApp.Data.ProductsandTaxes
{
    public class ProductsRepo
    {
        public List<Product> GetProductsList()
        {
            string filePath = @"DataFiles\ProductData.txt";

            List<Product> products = new List<Product>();

            var reader = File.ReadAllLines(filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var product = new Product();

                product.ProductType = columns[0].ToUpper();
                product.CostPerSquareFoot = decimal.Parse(columns[1]);
                product.LaborCostPerSquareFoot = decimal.Parse(columns[2]);

                products.Add(product);
            }

            return products;
        }

        public Product GetProductInfo(string ProductType)
        {
            var productsList = GetProductsList();

            var product = productsList.FirstOrDefault(p => p.ProductType.ToUpper() == ProductType.ToUpper());

            return product;
        }
    }
}
