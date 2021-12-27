using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
