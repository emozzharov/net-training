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


            var uriPlanet = new Uri(@"/xl/sharedStrings.xml", UriKind.Relative);
            var uriRadius = new Uri(@"/xl/worksheets/sheet1.xml", UriKind.Relative);

            using (Package xlPackage = Package.Open(xlsxFileName, FileMode.Open, FileAccess.Read))
            {
                var planetName = xlPackage.GetPart(uriPlanet).GetStream();
                var planetRadius = xlPackage.GetPart(uriRadius).GetStream();

                var planet = XDocument.Load(planetName);
                var radius = XDocument.Load(planetRadius);

                var resultName = planet.Descendants()
                    .Where(x => x.Name.LocalName == "t")
                    .Select(x => x.Value)
                    .Distinct()
                    .Take(8)
                    .ToList();
                var resultRadius = radius.Descendants()
                    .Where(x => x.Name.LocalName == "v")
                    .Select(x => x.Value)
                    .Where(x => x.Length > 3)
                    .Select(x => Math.Round(decimal.Parse(x), 2))
                    .ToList();

                var totalResult = resultName.Zip(resultRadius, (first, second) =>
                new PlanetInfo() { Name = first, MeanRadius = ((double)second) });


                return totalResult;
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
            var hashAlgorithm = HashAlgorithm.Create(hashAlgorithmName);

            if (hashAlgorithm is null)
            {
                throw new ArgumentException("Exception!");
            }

            var data = hashAlgorithm.ComputeHash(stream);

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2").ToUpper());
            }

            return sBuilder.ToString();
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
            FileStream sourceStream = new FileStream(fileName, FileMode.Open);

            switch (method)
            {
                case DecompressionMethods.GZip:

                    GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress);

                    return decompressionStream;


                case DecompressionMethods.Deflate:

                    DeflateStream decompressionStreamD = new DeflateStream(sourceStream, CompressionMode.Decompress);

                    return decompressionStreamD;


                default:
                    return sourceStream;
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
            // TODO : Implement ReadEncodedText method
            var result = File.ReadAllText(fileName, Encoding.GetEncoding(encoding));

            return result.ToString();
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
