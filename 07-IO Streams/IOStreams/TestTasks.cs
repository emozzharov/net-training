using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace IOStreams
{

	public static class TestTasks
	{
		/// <summary>
		/// Parses Resourses\Planets.xlsx file and returns the planet data: 
		///   Jupiter     69911.00
		///   Saturn      58232.00
		///   Uranus      25362.00
		///    ...
		/// See Resourses\Planets.xlsx for details
		/// </summary>
		/// <param name="xlsxFileName">source file name</param>
		/// <returns>sequence of PlanetInfo</returns>
		public static IEnumerable<PlanetInfo> ReadPlanetInfoFromXlsx(string xlsxFileName)
		{
			// TODO : Implement ReadPlanetInfoFromXlsx method using System.IO.Packaging + Linq-2-Xml

			// HINT : Please be as simple & clear as possible.
			//        No complex and common use cases, just this specified file.
			//        Required data are stored in Planets.xlsx archive in 2 files:
			//         /xl/sharedStrings.xml      - dictionary of all string values
			//         /xl/worksheets/sheet1.xml  - main worksheet

			Package package;
			string sharedStringsAsString, sheet1AsString;
			using (FileStream fs = File.Open(xlsxFileName, FileMode.Open))
			{
				package = Package.Open(fs);
				PackagePart sharedString = package.GetPart(new Uri("/xl/sharedStrings.xml", UriKind.Relative));
				PackagePart sheet = package.GetPart(new Uri("/xl/worksheets/sheet1.xml", UriKind.Relative));

				byte[] arr;
				using (var stream = sharedString.GetStream())
				{
					arr = new byte[stream.Length];
					stream.Read(arr, 0, (int)stream.Length);
				}

				sharedStringsAsString = System.Text.Encoding.Default.GetString(arr);

				using (var stream = sheet.GetStream())
				{
					arr = new byte[stream.Length];
					stream.Read(arr, 0, (int)stream.Length);
				}

				sheet1AsString = System.Text.Encoding.Default.GetString(arr);
			}

			XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

			var sharedStringsXml = XDocument.Parse(sharedStringsAsString);
			var sheetXml = XDocument.Parse(sheet1AsString);

			var planets = sharedStringsXml.Root.Descendants().Where(e => e.Name == ns + "t").Select(el => el.Value);
			var radiuses = sheetXml.Root.Descendants().Where(e => e.Name == ns + "row").Elements(ns + "c")
				.Where(e => e.Descendants().Count() != 0 && e.Attributes().All(a => a.Name != "t")).Select(el => el.Element(ns + "v").Value);

			NumberFormatInfo numberFormat = new NumberFormatInfo();
			numberFormat.NumberDecimalSeparator = ".";

			var zip = radiuses.Zip(planets, (rad, plan) => new PlanetInfo() { Name = plan, MeanRadius = Convert.ToDouble(rad, numberFormat) });

			return zip;
		}


		/// <summary>
		/// Calculates hash of stream using specifued algorithm
		/// </summary>
		/// <param name="stream">source stream</param>
		/// <param name="hashAlgorithmName">hash algorithm ("MD5","SHA1","SHA256" and other supported by .NET)</param>
		/// <returns></returns>
		public static string CalculateHash(this Stream stream, string hashAlgorithmName)
		{
			HashAlgorithm hash = HashAlgorithm.Create(hashAlgorithmName);
			if (hash is null)
            {
				throw new ArgumentException();
            }

			byte[] arr = new byte[stream.Length];
			stream.Read(arr, 0, (int)stream.Length);

			var hashArr = hash.ComputeHash(arr);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hashArr.Length; i++)
            {
				sb.Append(hashArr[i].ToString("X2"));
            }

			return sb.ToString();
		}


		/// <summary>
		/// Returns decompressed strem from file. 
		/// </summary>
		/// <param name="fileName">source file</param>
		/// <param name="method">method used for compression (none, deflate, gzip)</param>
		/// <returns>output stream</returns>
		public static Stream DecompressStream(string fileName, DecompressionMethods method)
		{
            using (FileStream inputFs = File.OpenRead(fileName))
            {
                MemoryStream ms = new MemoryStream();

                if (method == DecompressionMethods.GZip)
                {
                    GZipStream gz = new GZipStream(inputFs, CompressionMode.Decompress, true);
                    gz.CopyTo(ms);
                }
                else if (method == DecompressionMethods.Deflate)
                {
                    var ds = new DeflateStream(inputFs, CompressionMode.Decompress, true);
					ds.CopyTo(ms);
                }
                else
                {
                    inputFs.CopyTo(ms);
                }

				ms.Seek(0, SeekOrigin.Begin);
				return ms;
			}
		}


		/// <summary>
		/// Reads file content econded with non Unicode encoding
		/// </summary>
		/// <param name="fileName">source file name</param>
		/// <param name="encoding">encoding name</param>
		/// <returns>Unicoded file content</returns>
		public static string ReadEncodedText(string fileName, string encoding)
		{
			using (FileStream fs = File.OpenRead(fileName))
            {
				Encoding incomingEncoding = Encoding.GetEncoding(encoding);
				Encoding utf = Encoding.UTF8;

				byte[] arr = new byte[fs.Length];
				fs.Read(arr, 0, (int)fs.Length);
				byte[] encoded = Encoding.Convert(incomingEncoding, utf, arr);

				return utf.GetString(encoded);
            }
		}
	}


	public class PlanetInfo : IEquatable<PlanetInfo>
	{
		public string Name { get; set; }
		public double MeanRadius { get; set; }

		public override string ToString()
		{
			return string.Format("{0} {1}", Name, MeanRadius);
		}

		public bool Equals(PlanetInfo other)
		{
			return Name.Equals(other.Name)
				&& MeanRadius.Equals(other.MeanRadius);
		}
	}



}
