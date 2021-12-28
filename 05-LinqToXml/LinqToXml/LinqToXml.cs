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
                new XElement("Root",
                    from data in doc.Elements("Data")
                    group data by (string)data.Element("Category") into groupedData
                    select new XElement("Group",
                        new XAttribute("ID", groupedData.Key),
                        from g in groupedData
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
            var doc = XDocument.Parse(xmlRepresentation);

            XNamespace ns = "http://www.adventure-works.com";

            var listNumber = doc.Descendants(ns + "PurchaseOrder")
                .Elements(ns + "Address")
                .Where(x => x.Element(ns + "State").Value == "NY")
                .Where(x => x.Attribute(ns + "Type").Value == "Shipping")
                .Select(x => x.Parent.FirstAttribute.Value).ToList();

            var result = string.Empty;

            foreach (var item in listNumber)
            {
                result += item + ",";
            }

            return result.Trim(',');
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            File.WriteAllText("cust.csv", customers);

            string[] source = File.ReadAllLines("cust.csv");
            XElement cust = new XElement("Root",
                from str in source
                let fields = str.Split(',')
                select new XElement("Customer",
                    new XAttribute("CustomerID", fields[0]),
                    new XElement("CompanyName", fields[1]),
                    new XElement("ContactName", fields[2]),
                    new XElement("ContactTitle", fields[3]),
                    new XElement("Phone", fields[4]),
                    new XElement("FullAddress",
                        new XElement("Address", fields[5]),
                        new XElement("City", fields[6]),
                        new XElement("Region", fields[7]),
                        new XElement("PostalCode", fields[8]),
                        new XElement("Country", fields[9])
                    )
                )
            );

            return cust.ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            var doc = XDocument.Parse(xmlRepresentation);

            var result = doc.Descendants().Select(x => x.Value).ToList();

            string strResult = result[0];

            return strResult;
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            var doc = XDocument.Parse(xmlRepresentation);

            foreach (var element in doc.Descendants())
            {
                if (element.Name.LocalName.StartsWith("customer"))
                {
                    element.Name = "contact";
                }
            }

            return doc.ToString();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            var doc = XDocument.Parse(xmlRepresentation);

            var res = doc.DescendantNodes()
                .Where(x => x.GetType() == typeof(XComment))
                .Where(y => y.Parent.Elements("subscriber").Count() > 1);

            var result = res.Select(x => int.Parse(x.Parent.FirstAttribute.Value)).ToList();

            return result;
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            var doc = XDocument.Parse(xmlRepresentation);

            //var sortedElements = doc.Descendants().OrderBy(x => x.Attribute("City"));

            var sort = doc.Descendants("FullAddress").OrderBy(x => (string)x.Element("Country")).ThenBy(x => (string)x.Element("City"));

            return null;
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
            var doc = XDocument.Parse(xmlRepresentation);

            var result = doc.Descendants("product").Select(x => x.Value).ToList();

            int sum = 0;

            foreach (var item in result)
            {
                if (item.Equals("1"))
                {
                    sum += 300;
                }

                if (item.Equals("2"))
                {
                    sum += 910;
                }
            }

            return sum;
        }
    }
}
