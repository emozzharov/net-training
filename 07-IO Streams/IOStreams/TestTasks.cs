using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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
			Uri planetNameUri = PackUriHelper.CreatePartUri(
									  new Uri(@"/xl/sharedStrings.xml", UriKind.Relative));;
			Uri planetRadiusUri = PackUriHelper.CreatePartUri(
									  new Uri(@" /xl/worksheets/sheet1.xml", UriKind.Relative));

			using (Package package = Package.Open(xlsxFileName, FileMode.Open))
			{
				var planetNameStream = package.GetPart(planetNameUri).GetStream();
				var planetRadiusStream = package.GetPart(planetRadiusUri).GetStream();
				XDocument planetDocument = XDocument.Load(planetNameStream);
				XDocument radiusDocument = XDocument.Load(planetRadiusStream);
				var names = planetDocument.Descendants().Where(t=>t.Name.LocalName.Equals("t")).Select(t=>t.Value).ToList();
				var radius = radiusDocument.Descendants().Where(t=>t.Name.LocalName.Equals("v") && t.Value.Length > 1).Select(t=>t.Value).ToList();
				names.RemoveAt(names.Count() - 1);
				var planetsInfo = names.Zip(radius, (f, s) => new PlanetInfo() { Name = f, MeanRadius = (double)Math.Round(decimal.Parse(s), 2) });
				return planetsInfo;
			}
		}


		/// <summary>
		/// Calculates hash of stream using specifued algorithm
		/// </summary>
		/// <param name="stream">source stream</param>
		/// <param name="hashAlgorithmName">hash algorithm ("MD5","SHA1","SHA256" and other supported by .NET)</param>
		/// <returns></returns>
		public static string CalculateHash(this Stream stream, string hashAlgorithmName)
		{
			// TODO : Implement CalculateHash method
			var algorithm = HashAlgorithm.Create(hashAlgorithmName);
			if (algorithm == null) throw new ArgumentException();
			var hash = algorithm.ComputeHash(stream);
			StringBuilder sOutput = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sOutput.Append(hash[i].ToString("X2"));
			}
			return sOutput.ToString();
		}


		/// <summary>
		/// Returns decompressed strem from file. 
		/// </summary>
		/// <param name="fileName">source file</param>
		/// <param name="method">method used for compression (none, deflate, gzip)</param>
		/// <returns>output stream</returns>
		public static Stream DecompressStream(string fileName, DecompressionMethods method)
		{
			// TODO : Implement DecompressStream method
			switch(method)
            {
				case DecompressionMethods.GZip:
                    {
						FileStream str = new FileStream(fileName,FileMode.Open);
						GZipStream dec = new GZipStream(str,CompressionMode.Decompress);
						return dec;
                    }
					break;
					
				case DecompressionMethods.Deflate:
                    {
						FileStream str = new FileStream(fileName, FileMode.Open);
						DeflateStream dec = new DeflateStream(str, CompressionMode.Decompress);
						return dec;
                    }
					break;
			}
			return new FileStream(fileName,FileMode.Open);
		}


		/// <summary>
		/// Reads file content econded with non Unicode encoding
		/// </summary>
		/// <param name="fileName">source file name</param>
		/// <param name="encoding">encoding name</param>
		/// <returns>Unicoded file content</returns>
		public static string ReadEncodedText(string fileName, string encoding)
		{
			// TODO : Implement ReadEncodedText method
			throw new NotImplementedException();
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
