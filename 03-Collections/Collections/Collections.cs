﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            // TODO : Implement Fibonacci sequence generator
            List<int> numbers = new List<int>();
            if (count < 0) throw new ArgumentException();
            if (count == 0) return numbers;
            for(int i=1;i<=count;i++)
            {
                numbers.Add(GetFibNumber(i));
            }
            return numbers;
        }

        private static int GetFibNumber(int number)
        {
            if (number == 0) return 0;
            if (number == 1) return number;
            return GetFibNumber(number - 1) + GetFibNumber(number-2);
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
            char[] delimeters = new[] { ',', ' ', '.', '\t', '\n' };
            if (reader == null) throw new ArgumentNullException();
            string text = "";
            while (true)
            {
                string textPart = reader.ReadLine();
                if (textPart == null) break;
                text += textPart;
            }
            var result = text.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
            return result;
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
            // TODO : Implement the tree depth traversal algorithm
            if (root == null) throw new ArgumentNullException();

            Stack<ITreeNode<T>> nodeStack = new Stack<ITreeNode<T>>();
            List<T> treeValues = new List<T>();
            nodeStack.Push(root);
            while (nodeStack.Any())
            {
                var current = nodeStack.Pop();
                treeValues.Add(current.Data);
                if (current.Children == null) continue;

                foreach(var nod in current.Children.Reverse())
                {
                    nodeStack.Push(nod);
                }
            }
            return treeValues;
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
            // TODO : Implement the tree width traversal algorithm
            if (root == null) throw new ArgumentNullException();

            Queue<ITreeNode<T>> nodeQueue = new Queue<ITreeNode<T>>();
            List<T> treeValues = new List<T>();
            nodeQueue.Enqueue(root);
            while (nodeQueue.Any())
            {
                var current = nodeQueue.Dequeue();
                treeValues.Add(current.Data);
                if (current.Children == null) continue;

                foreach (var nod in current.Children)
                {
                    nodeQueue.Enqueue(nod);
                }
            }
            return treeValues;
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
        public static IEnumerable<T[]> GenerateAllPermutations<T>(T[] source, int count)
        {
            // TODO : Implement GenerateAllPermutations method
            if (count > source.Length || count < 0) throw new ArgumentOutOfRangeException();
            List<List<T>> pers = new List<List<T>>();
            if (count == 0) return pers.Select(el=>el.ToArray());
            if (count == 1)
            {
                foreach (var item in source)
                    pers.Add(new List<T> {item});
                return pers.Select(el => el.ToArray());
            }
            else if(count == source.Length)
            {
                pers.Add(new List<T>(source));
                return pers.Select(el => el.ToArray());
            }
            Сombinations(source, new List<int>(), 0, count, pers);
            return pers.Select(el=>el.ToArray());
        }
        public static void Сombinations<T>(T[] array, List<int> skip, int current, int max, List<List<T>> result)
        {
            if (result.Count == 0)
            {
                result.Add(new List<T>());
            }
            if (max == 0)
            {
                return;
            }
            var currentLevelResult = new List<T>(result.Last());

            for (int i = current; i < array.Length; ++i)
            {
                if (skip.Contains(i))
                {
                    continue;
                }

                result.Last().Add(array[i]);
                if (result.Last().Count() != max)
                {
                    Сombinations(array, skip, i + 1, max, result);
                }
                result.Add(new List<T>(currentLevelResult));
            }
            if (result.Last().Count() != max)
            {
                result.Remove(result.Last());
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
            // TODO : Implement GetOrBuildValue method for cache
            if(!dictionary.ContainsKey(key))
            {
                dictionary.Add(key,builder());
            }
            return dictionary[key];
        }

    }
}
