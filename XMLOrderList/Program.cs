using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace XMLOrderList
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                // Пробую открыть xml документ, и в случае успеха передаю полученный экземпляр XmlDocument в метод для добавления заказа в существующий файл
                doc.Load("Orders.xml");
                WriteOrderToXmlFile(doc);
            }
            catch (Exception ex)
            {
                // В случае ошибок, создаю новый путем вызова метода с передачей null вместо экземпляра XmlDocument
                WriteOrderToXmlFile(null);
                Console.WriteLine(ex.Message);
            }

            ShowNode(doc.DocumentElement);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }


        /// <summary>
        /// Метод используемый для добавления нового заказа в существующий xml документ, или создание нового xml документа.
        /// </summary>
        /// <param name="doc">Передается ссылка на экземпляр XmlDocument, если нужно добавить заказ в существующий файл, либо null в случае если формируется новый xml файл.</param>
        static void WriteOrderToXmlFile(XmlDocument doc)
        {
            Console.Write("Хотите добавить новый заказ ? [Y/N]: ");
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key.ToString() != "Y" && key.ToString() != "y")
            {
                ShowNode(doc.DocumentElement);
                return;
            }
            Console.WriteLine();

            XmlNode rootElem;
            if (doc == null)
            {
                doc = new XmlDocument();
                rootElem = doc.CreateElement("orders");
                doc.AppendChild(rootElem);
            }
            else
            {
                rootElem = doc.DocumentElement;
            }

            // Создаю экземпляр нового заказа
            Order orderObj = new Order();
            // Получаю новый заказ в виде XmlNode
            XmlNode order = orderObj.Make(doc);

            // Если заказ сформирован корректно (не null) добавляю его в файл xml и записываю документ на диск.
            if (order != null)
            {
                // Вычисляю номер для нового заказа
                XmlNodeList orders = rootElem.ChildNodes;
                uint orderID = 0; // ID нового заказа
                // ID нового заказа = (максимальный номер среди уже созданных заказов) +1
                foreach (XmlNode node in orders)
                {
                    XmlAttributeCollection attributes = node.Attributes;
                    foreach (XmlAttribute attr in attributes)
                    {
                        if ((attr.Name == "id") && (orderID <= Convert.ToUInt32(attr.Value))) { orderID = Convert.ToUInt32(attr.Value) + 1; }
                    }
                }

                // Добавляю к заказу атрибут ID
                XmlAttribute orderIDAttr = doc.CreateAttribute("id");
                orderIDAttr.Value = orderID.ToString();
                order.Attributes.Append(orderIDAttr);

                // Добавляю новый заказ к xml документу
                rootElem.AppendChild(order);

                doc.Save("Orders.xml");
                doc.Load("Orders.xml");
            }
            else
            {
                Console.WriteLine("Error, new order hasn't been added !!! Press ane key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Вывод списка заказов на консоль
        /// </summary>
        /// <param name="node"></param>
        static void ShowNode(XmlNode orders)
        {
            foreach (XmlNode order in orders)
            {
                Console.WriteLine();
                if (order.Attributes != null)
                {
                    foreach (XmlAttribute attr in order.Attributes)
                    {
                        Console.WriteLine("Order ID = {0}", attr.Value);
                        Console.WriteLine("================");
                    }
                }
                if (order.HasChildNodes)
                {
                    XmlNodeList products = order.ChildNodes;
                    foreach (XmlNode product in products)
                    {
                        if (product.HasChildNodes)
                        {
                            XmlNodeList productFields = product.ChildNodes;
                            foreach (XmlNode productField in productFields)
                            {
                                if (productField.NodeType == XmlNodeType.Element)
                                {
                                    Console.Write(" {0} = {1},", productField.Name, productField.InnerText);
                                }
                                else
                                {
                                    Console.Write(" {0} = {1}", product.Name, product.InnerText);
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
    }
}