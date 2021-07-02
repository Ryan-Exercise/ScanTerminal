using System;
using System.Text.RegularExpressions;

namespace ScanTerminal
{
    public class Product
    {
        public decimal Price { get; set; }
        public string Code { get; private set;  }
        // define more properties here, unit, description and color e.g.

        protected Product(string code, decimal price)
        {
            Price = price;
            Code = code.Trim();
        }

        //public override bool Equals(object obj)
        //{
        //    var right = obj as Product;
            
        //    return right != null && Code.Equals(right.Code);
        //}

        //public override int GetHashCode()
        //{
        //    return Code.GetHashCode();
        //}

        public static Product Create(string code, decimal price)
        {
            // validate code
            if (string.IsNullOrEmpty(code) || string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code cannot be empty or null");
            }

            if (code.Trim().Length != 1)
            {
                throw new ArgumentException("Code can only contain 1 character");
            }

            string pattern = @"[a-zA-Z0-9]{1}";
            if(!Regex.Match(code, pattern).Success)
            {
                throw new ArgumentException("Code can only be single number or letter");
            }

            // validate & reset price
            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            return new Product(code, Math.Round(price, 2));
        }
    }
}
