using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncIO
{
    public static class Tasks
    {


        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the synchronous way and can be used to compare the performace of sync \ async approaches. 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <returns>The sequence of downloaded url content</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEnumerable<string> GetUrlContent(this IEnumerable<Uri> uris) 
        {
            List<string> res = new List<string>();
            
            foreach (var u in uris)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(u);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var str = reader.ReadToEnd();
                    }
                }

                res.Add(u.ToString());
            }

            return res;
            //object locker = new object();
            //lock (locker)
            //{
            //    List<string> res = new List<string>();
            //    foreach (var u in uris)
            //    {
            //        res.Add(u.ToString());
            //    }

            //    return res; 
            //}
        }



        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the asynchronous way and can be used to compare the performace of sync \ async approaches. 
        /// 
        /// maxConcurrentStreams parameter should control the maximum of concurrent streams that are running at the same time (throttling). 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <param name="maxConcurrentStreams">Max count of concurrent request streams</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContentAsync(this IEnumerable<Uri> uris, int maxConcurrentStreams)
        {
            ConcurrentBag<string> bag = new ConcurrentBag<string>();
            Parallel.ForEach(uris, new ParallelOptions { MaxDegreeOfParallelism = maxConcurrentStreams }, uri => AddToList(uri));

            return bag;

            void AddToList(Uri uri)
            {
                bag.Add(uri.ToString());
            }
        }


        /// <summary>
        /// Calculates MD5 hash of required resource.
        /// 
        /// Method has to run asynchronous. 
        /// Resource can be any of type: http page, ftp file or local file.
        /// </summary>
        /// <param name="resource">Uri of resource</param>
        /// <returns>MD5 hash</returns>
        public static Task<string> GetMD5Async(this Uri resource)
        {
            // TODO : Implement GetMD5Async
            throw new NotImplementedException();
        }

    }



}
