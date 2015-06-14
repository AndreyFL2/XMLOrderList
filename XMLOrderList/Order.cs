using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;

namespace XMLOrderList
{
    class Order
    {
        class Product
        {
            string name;
            public uint amount;
            public uint price;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public uint Amount
            {
                get
                { return amount; }
                set
                { amount = value; }
            }
            public uint Price
            {
                get
                { return price; }
                set
                { price = value; }
            }

        }

        List<Product> Products;
        string Phone { get; set; }

        public Order()
        {
            Products = new List<Product>();
        }

        public XmlNode Make(XmlDocument doc)
        {
            Console.Write("Введите номер телефона заказчика: ");
            Phone = Console.ReadLine();
            if (Phone.Length <= 0)
            {
                return null;
            }

            ConsoleKey key;
            do
            {
                key = ConsoleKey.N;
                Product prod = new Product();

                Console.Write("Введите наименование продукта: ");
                prod.Name = Console.ReadLine();
                if (prod.Name.Length <= 0)
                {
                    return null;
                }
                try
                {
                    Console.Write("Введите количество продукта: ");
                    prod.Amount = Convert.ToUInt32(Console.ReadLine());
                    Console.Write("Введите стоимость продукта: ");
                    prod.Price = Convert.ToUInt32(Console.ReadLine());

                    Products.Add(prod);
                }
                catch (Exception ex)
                {
                    return null;
                }
                Console.WriteLine("Добавить еще одину позицию заказа ? [Y/N]: ");
                key = Console.ReadKey(true).Key;
            } while (key.ToString() == "y" || key.ToString() == "Y");

            if (Products.Count > 0)
            {
                XmlNode order = doc.CreateElement("order");
                foreach (Product pr in Products)
                {
                    XmlNode product = doc.CreateElement("product");

                    XmlNode productName = doc.CreateElement("name");
                    XmlNode productNameValue = doc.CreateTextNode(pr.Name);
                    productName.AppendChild(productNameValue);
                    product.AppendChild(productName);

                    XmlNode productAmount = doc.CreateElement("amount");
                    XmlNode productAmountValue = doc.CreateTextNode(pr.Amount.ToString());
                    productAmount.AppendChild(productAmountValue);
                    product.AppendChild(productAmount);

                    XmlNode productPrice = doc.CreateElement("price");
                    XmlNode productPriceValue = doc.CreateTextNode(pr.Price.ToString());
                    productPrice.AppendChild(productPriceValue);
                    product.AppendChild(productPrice);

                    order.AppendChild(product);

                }
                XmlNode clientsPhone = doc.CreateElement("clientsPhone");
                XmlNode clientsPhoneValue = doc.CreateTextNode(this.Phone);
                clientsPhone.AppendChild(clientsPhoneValue);
                order.AppendChild(clientsPhone);

                return order;
            }
            else
            { return null; }
        }
    }
}
