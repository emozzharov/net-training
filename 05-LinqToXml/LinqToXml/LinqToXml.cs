using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace LinqToXml
{
    public static class LinqToXml
    {
        /// <summary>
        /// Creates hierarchical data grouped by category
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation (refer to CreateHierarchySourceFile.xml in Resources)</param>
        /// <returns>Xml representation (refer to CreateHierarchyResultFile.xml in Resources)</returns>
        public static string CreateHierarchy(string xmlRepresentation)
        {
            XElement doc = XElement.Parse(xmlRepresentation);
            var newData =
                new XElement("Root", from data in doc.Elements("Data") group data by (string)data.Element("Category") into groupedData
                    select new XElement("Group", new XAttribute("ID", groupedData.Key), from g in groupedData
                        select new XElement("Data",
                            g.Element("Quantity"),
                            g.Element("Price")
                        )
                    )
                );
            return newData.ToString();
        }

        /// <summary>
        /// Get list of orders numbers (where shipping state is NY) from xml representation
        /// </summary>
        /// <param name="xmlRepresentation">Orders xml representation (refer to PurchaseOrdersSourceFile.xml in Resources)</param>
        /// <returns>Concatenated orders numbers</returns>
        /// <example>
        /// 99301,99189,99110
        /// </example>
        public static string GetPurchaseOrders(string xmlRepresentation)
        {
            XDocument document = XDocument.Parse(xmlRepresentation);
            XNamespace namesp = "http://www.adventure-works.com";
            var result = document.Descendants(namesp+"PurchaseOrder")
                .Where(order=>(string)order.Element(namesp + "Address").Attribute(namesp+"Type") == "Shipping" && (string)order.Element(namesp+ "Address").Element(namesp+ "State") == "NY")
                .Select(or=>(string)or.Attribute(namesp+ "PurchaseOrderNumber")).ToList();
            return String.Join(",",result);
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            XDocument document = XDocument.Parse(xmlRepresentation);
            var result = document.Descendants().Select(x=>x.Value.ToString()).ToList().FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            XDocument document = XDocument.Parse(xmlRepresentation);
            foreach(XElement item in document.Descendants())
            {
                if (item.Name.LocalName.Equals("customer")) item.Name = "contact";
            }
            return document.ToString();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            XDocument document = XDocument.Parse(xmlRepresentation);
            var result = document.DescendantNodes().Where(x=>x.GetType() == typeof(XComment) && x.Parent.Elements("subscriber").Count()>=2)
                .Select(x=>(int)x.Parent.Attribute("id")).ToList();
            return result;
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            XDocument document = XDocument.Parse(xmlRepresentation);
            var sortedCustomers = document.Descendants("Customers")
                .OrderBy(item => (string)item.Element("FullAddress").Element("Country"))
                .ThenBy(item => (string)item.Element("FullAddress").Element("City")).ToList();

            XDocument sortedDocument = new XDocument();
            XElement root = new XElement("Root");
            sortedDocument.Add(root);
            foreach (var item in sortedCustomers)
            {
                root.Add(item);
            }
            return sortedDocument.ToString();
        }

        /// <summary>
        /// Gets XElement flatten string representation to save memory
        /// </summary>
        /// <param name="xmlRepresentation">XElement object</param>
        /// <returns>Flatten string representation</returns>
        /// <example>
        ///     <root><element>something</element></root>
        /// </example>
        public static string GetFlattenString(XElement xmlRepresentation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            XDocument document = XDocument.Parse(xmlRepresentation);
            int totalSumm = 0;
            var productInfo = document.Descendants("products")
                .Select(x=>x.Elements().Select(r=>new { id= (int)r.FirstAttribute , value= (int) r.FirstAttribute.NextAttribute}).ToDictionary(k=>k.id))
                .FirstOrDefault();
            var orders = document.Descendants("Order").Select(s=>int.Parse(s.Element("product").Value)).ToList();
            foreach (var prodId in orders) totalSumm += productInfo[prodId].value;
            return totalSumm;
        }
    }
}
