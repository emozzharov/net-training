﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Collections.Tasks {

    /// <summary>
    ///  Tree node item 
    /// </summary>
    /// <typeparam name="T">the type of tree node data</typeparam>
    public interface ITreeNode<T> {
        T Data { get; set; }                             // Custom data
        IEnumerable<ITreeNode<T>> Children { get; set; } // List of childrens
    }


    public class Task {

        /// <summary> Generate the Fibonacci sequence f(x) = f(x-1)+f(x-2) </summary>
        /// <param name="count">the size of a required sequence</param>
        /// <returns>
        ///   Returns the Fibonacci sequence of required count
        /// </returns>
        /// <exception cref="System.InvalidArgumentException">count is less then 0</exception>
        /// <example>
        ///   0 => { }  
        ///   1 => { 1 }    
        ///   2 => { 1, 1 }
        ///   12 => { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144 }
        /// </example>
        public static IEnumerable<int> GetFibonacciSequence(int count) {
            List<int> res = new List<int>();

            if (count < 0)
            {
                throw new ArgumentException();
            }
            
            if (count == 0)
            {
                return res;
            }

            int initial = 0;
            int next = 1;
            int temp;
            res.Add(next);
            for (int i = 1; i < count; i++)
            {
                temp = initial;
                initial = next;
                next += temp;
                res.Add(next);
            }

            return res;
        }

        /// <summary>
        ///    Parses the input string sequence into words
        /// </summary>
        /// <param name="reader">input string sequence</param>
        /// <returns>
        ///   The enumerable of all words from input string sequence. 
        /// </returns>
        /// <exception cref="System.ArgumentNullException">reader is null</exception>
        /// <example>
        ///  "TextReader is the abstract base class of StreamReader and StringReader, which ..." => 
        ///   {"TextReader","is","the","abstract","base","class","of","StreamReader","and","StringReader","which",...}
        /// </example>
        public static IEnumerable<string> Tokenize(TextReader reader) {
            if (reader == null)
            {
                throw new ArgumentNullException();
            }

            char[] delimeters = new[] { ',', ' ', '.', '\t', '\n' };

            List<string> res = new List<string>();

            StringBuilder temp = new StringBuilder();

            while (true)
            {
                int charIndex = reader.Read();
                if (charIndex == -1)
                {
                    if (temp.Length != 0)
                    {
                        res.Add(temp.ToString());
                    }

                    break;
                }

                char intermediate = (char)charIndex;
                bool isDelimeter = IsDelimeter(intermediate);
                if (isDelimeter && temp.Length != 0)
                {
                    res.Add(temp.ToString());
                }

                if (isDelimeter)
                {
                    temp.Clear();
                }
                else
                {
                    temp.Append(intermediate);
                }
            }
            
            
            return res;

            bool IsDelimeter(char charToCompare)
            {
                foreach (var delim in delimeters)
                {
                    if (charToCompare == delim)
                    {
                        return true;
                    }
                }

                return false;
            }
        }



        /// <summary>
        ///   Traverses a tree using the depth-first strategy
        /// </summary>
        /// <typeparam name="T">tree node type</typeparam>
        /// <param name="root">the tree root</param>
        /// <returns>
        ///   Returns the sequence of all tree node data in depth-first order
        /// </returns>
        /// <example>
        ///    source tree (root = 1):
        ///    
        ///                      1
        ///                    / | \
        ///                   2  6  7
        ///                  / \     \
        ///                 3   4     8
        ///                     |
        ///                     5   
        ///                   
        ///    result = { 1, 2, 3, 4, 5, 6, 7, 8 } 
        /// </example>
        public static IEnumerable<T> DepthTraversalTree<T>(ITreeNode<T> root) {
            T[] res = Array.Empty<T>();

            if (root == null)
            {
                throw new ArgumentNullException();
            }

            if (root.Children == null)
            {
                return new T[] { root.Data };
            }

            LinkedList<ITreeNode<T>> queue = new LinkedList<ITreeNode<T>>();
            queue.AddLast(root);

            while (queue.Count > 0)
            {
                ITreeNode<T> picked = queue.First.Value;
                queue.RemoveFirst();

                Array.Resize(ref res, res.Length + 1);
                res[res.Length - 1] = picked.Data;

                if (picked.Children != null)
                {
                    Stack<ITreeNode<T>> tempStack = new Stack<ITreeNode<T>>();
                    
                    foreach (var item in picked.Children)
                    {
                        tempStack.Push(item);
                    }

                    while (tempStack.Count > 0)
                    {
                        queue.AddFirst(tempStack.Pop());
                    }
                }
            }

            return res;
        }

        /// <summary>
        ///   Traverses a tree using the width-first strategy
        /// </summary>
        /// <typeparam name="T">tree node type</typeparam>
        /// <param name="root">the tree root</param>
        /// <returns>
        ///   Returns the sequence of all tree node data in width-first order
        /// </returns>
        /// <example>
        ///    source tree (root = 1):
        ///    
        ///                      1
        ///                    / | \
        ///                   2  3  4
        ///                  / \     \
        ///                 5   6     7
        ///                     |
        ///                     8   
        ///                   
        ///    result = { 1, 2, 3, 4, 5, 6, 7, 8 } 
        /// </example>
        public static IEnumerable<T> WidthTraversalTree<T>(ITreeNode<T> root) {
            T[] res = Array.Empty<T>();

            if (root == null)
            {
                throw new ArgumentNullException();
            }

            if (root.Children == null)
            {
                return new T[] { root.Data };
            }

            Queue<ITreeNode<T>> queue = new Queue<ITreeNode<T>>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                ITreeNode<T> picked = queue.Dequeue();

                Array.Resize(ref res, res.Length + 1);
                res[res.Length - 1] = picked.Data;

                if (picked.Children != null)
                {
                    foreach (var item in picked.Children)
                    {
                        queue.Enqueue(item);
                    }
                }
            }

            return res;
        }



        /// <summary>
        ///   Generates all permutations of specified length from source array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">source array</param>
        /// <param name="count">permutation length</param>
        /// <returns>
        ///    All permuations of specified length
        /// </returns>
        /// <exception cref="System.InvalidArgumentException">count is less then 0 or greater then the source length</exception>
        /// <example>
        ///   source = { 1,2,3,4 }, count=1 => {{1},{2},{3},{4}}
        ///   source = { 1,2,3,4 }, count=2 => {{1,2},{1,3},{1,4},{2,3},{2,4},{3,4}}
        ///   source = { 1,2,3,4 }, count=3 => {{1,2,3},{1,2,4},{1,3,4},{2,3,4}}
        ///   source = { 1,2,3,4 }, count=4 => {{1,2,3,4}}
        ///   source = { 1,2,3,4 }, count=5 => ArgumentOutOfRangeException
        /// </example>
        public static IEnumerable<T[]> GenerateAllPermutations<T>(T[] source, int count) {
            if (count < 0 || count > source.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (count == 0)
            {
                return Enumerable.Empty<T[]>();
            }

            if (source.Length == 1)
            {
                return source.Select(s => new T[] { s });
            }

            if (source.Length == count)
            {
                return new List<T[]>() { source};
            }

            var res = GetPermutations(source, count);

            return res.Select(a => a.ToArray());

            IEnumerable<IEnumerable<U>> GetPermutations<U>(IEnumerable<U> list, int length)
            {
                if (length == 1) return list.Select(t => new U[] { t });
                return GetPermutations(list, length - 1)
                    .SelectMany(t => list.Where(o => 
                    {
                        var el = o as IComparable;
                        return el.CompareTo(t.Last()) > 0;
                    }), (e1, e2) => e1.Concat(new U[] { e2 }));
            }
        }
    }

    public static class DictionaryExtentions {
        
        /// <summary>
        ///    Gets a value from the dictionary cache or build new value
        /// </summary>
        /// <typeparam name="TKey">TKey</typeparam>
        /// <typeparam name="TValue">TValue</typeparam>
        /// <param name="dictionary">source dictionary</param>
        /// <param name="key">key</param>
        /// <param name="builder">builder function to build new value if key does not exist</param>
        /// <returns>
        ///   Returns a value assosiated with the specified key from the dictionary cache. 
        ///   If key does not exist than builds a new value using specifyed builder, puts the result into the cache 
        ///   and returns the result.
        /// </returns>
        /// <example>
        ///   IDictionary<int, Person> cache = new SortedDictionary<int, Person>();
        ///   Person value = cache.GetOrBuildValue(10, ()=>LoadPersonById(10) );  // should return a loaded Person and put it into the cache
        ///   Person cached = cache.GetOrBuildValue(10, ()=>LoadPersonById(10) );  // should get a Person from the cache
        /// </example>
        public static TValue GetOrBuildValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> builder) {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, builder.Invoke());
            }

            return dictionary[key];
        }

    }
}
