using System;
using ScanTerminal;

namespace StoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PricingList list = PricingList.Create()
                                .AddProduct(BulkPriceProduct.Create("A", 1.25m, 3, 3m))
                                .AddProduct(Product.Create("B", 4.25m))
                                .AddProduct(BulkPriceProduct.Create("C", 1m, 6, 5m))
                                .AddProduct(Product.Create("D", 0.75m));
            PointOfSaleTerminal terminal = new PointOfSaleTerminal(list);
            //const string cart = "ABCDABA";
            //const string cart = "CCCCCCC";
            const string cart = "ABCD";
            foreach (var code in cart)
            {
                terminal.ScanProduct(code.ToString());
            }

            Console.WriteLine(terminal.Total);
        }
    }
}
