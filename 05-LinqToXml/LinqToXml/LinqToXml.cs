using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Globalization;

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
            var xml = XDocument.Parse(xmlRepresentation).Descendants("Root");
            var elements = xml.Elements("Data");

            XDocument result = new XDocument();
            result.Add(new XElement("Root", null));
            var root = result.Element("Root");

            var groupsValues = elements.Select(e => e.Element("Category").Value).Distinct(); // selects unique categories from source.

            // Adds group to the root of a document in accordance with "Category".
            foreach (var val in groupsValues)
            {
                var group = new XElement("Group");
                group.Add(new XAttribute("ID", val));
                root.Add(group);
            }

            foreach (var el in elements)
            {
                var category = el.Element("Category").Value; // gets category of the current element.

                // Selects group of result file by the category in accordance with the current element's category.
                var selectedGroup = root.Elements("Group").SingleOrDefault(e => e.Attributes().Any(a => a.Value == category));

                var data = new XElement("Data");
                var qty = new XElement("Quantity", el.Element("Quantity").Value);
                var price = new XElement("Price", el.Element("Price").Value);

                data.Add(new XElement[] { qty, price });
                selectedGroup.Add(data);
            }

            return result.ToString();
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
            XNamespace aw = "http://www.adventure-works.com";
            var xml = XDocument.Parse(xmlRepresentation).Descendants(aw + "PurchaseOrders");

            var numbers = xml.Elements(aw + "PurchaseOrder").Where(order =>
            {
                var shippingState = order.Elements(aw + "Address")
                    .SingleOrDefault(typeOfDestination => typeOfDestination.Attribute(aw + "Type").Value == "Shipping")
                    .Element(aw + "State").Value;

                if (shippingState == "NY")
                {
                    return true;
                }

                return false;
            }).Select(order => order.Attribute(aw + "PurchaseOrderNumber").Value);

            return string.Join(",", numbers);
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            var customersAsString = customers.Split('\n').Select(c => c.Split(','));

            XDocument xml = new XDocument();
            XElement root = new XElement("Root");
            xml.Add(root);

            foreach (var cust in customersAsString)
            {
                XElement customer = new XElement("Customer");
                XAttribute custID = new XAttribute("CustomerID", cust[0]);

                customer.Add(custID);

                XElement companyName = new XElement("CompanyName", cust[1]);
                XElement contactName = new XElement("ContactName", cust[2]);
                XElement contactTitle = new XElement("ContactTitle", cust[3]);
                XElement phone = new XElement("Phone", cust[4]);

                XElement fullAddress = new XElement("FullAddress");
                XElement address = new XElement("Address", cust[5]);
                XElement city = new XElement("City", cust[6]);
                XElement region = new XElement("Region", cust[7]);
                XElement postalCode = new XElement("PostalCode", cust[8]);
                XElement country = new XElement("Country", cust[9].Trim(new char[] { '\n', '\r' }));
                fullAddress.Add(new XElement[] { address, city, region, postalCode, country });

                customer.Add(new XElement[] { companyName, contactName, contactTitle, phone, fullAddress });

                root.Add(customer);
            }

            return xml.ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            var parsed = XDocument.Parse(xmlRepresentation).Descendants("Document").SelectMany(d => d.Descendants("Sentence"))
                .Select(innerDesc => innerDesc.Value);

            return string.Concat(parsed);
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            var xml = XDocument.Parse(xmlRepresentation);
            var docs = xml.Descendants("Document").Elements("customer");
            foreach (var doc in docs)
            {
                doc.Name = "contact";
            }

            return xml.ToString();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            var res = XDocument.Parse(xmlRepresentation).Descendants("service").Elements("channel").Where(el =>
            {
                if (el.Elements("subscriber").Count() >= 2 
                && el.Nodes().Any(inner => inner.NodeType == System.Xml.XmlNodeType.Comment && ((XComment)inner).Value == "DELETE")
                )
                {
                    return true;
                }

                return false;
            }).Select(el => Convert.ToInt32(el.Attribute("id").Value));

            return res;
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            var xml = XDocument.Parse(xmlRepresentation);
            var elements = xml.Root.Elements("Customers").OrderBy(e => e.Element("FullAddress").Element("Country").Value)
                .ThenBy(e => e.Element("FullAddress").Element("City").Value).ToList();

            xml.Root.RemoveAll();
            xml.Root.Add(elements);

            return xml.ToString();
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
            // What exactly to do?
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            var xml = XDocument.Parse(xmlRepresentation);

            var dict = xml.Root.Elements("Orders").SelectMany(el => el.Elements("Order")).Select(e => e.Element("product").Value).GroupBy(v => v)
                .ToDictionary(val => val.Key, val => val.Count() * Convert.ToInt32(xml.Root.Element("products").Elements()
                .Where(e => e.Name.LocalName.Equals("product", StringComparison.InvariantCultureIgnoreCase))
                .SingleOrDefault(p => p.Attribute("Id").Value == val.Key).Attribute("Value").Value));

            return dict.Sum(d => d.Value);
        }
    }
}
