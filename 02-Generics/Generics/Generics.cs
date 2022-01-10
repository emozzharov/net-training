﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Task.Generics {

	public static class ListConverter {

		private static char ListSeparator = ',';  // Separator used to separate values in string

		/// <summary>
		///   Converts a source list into a string representation
		/// </summary>
		/// <typeparam name="T">type  of list items</typeparam>
		/// <param name="list">source list</param>
		/// <returns>
		///   Returns the string representation of a list 
		/// </returns>
		/// <example>
		///   { 1,2,3,4,5 } => "1,2,3,4,5"
		///   { '1','2','3','4','5'} => "1,2,3,4,5"
		///   { true, false } => "True,False"
		///   { ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.Cyan } => "Black,Blue,Cyan"
		///   { new TimeSpan(1, 0, 0), new TimeSpan(0, 0, 30) } => "01:00:00,00:00:30",
		/// </example>
		public static string ConvertToString<T>(this IEnumerable<T> list) {
			if (list is null)
            {
				return null;
            }
			return string.Join(ListSeparator.ToString(), list);
		}

		/// <summary>
		///   Converts the string respresentation to the list of items
		/// </summary>
		/// <typeparam name="T">required type of output items</typeparam>
		/// <param name="list">string representation of the list</param>
		/// <returns>
		///   Returns the list of items from specified string
		/// </returns>
		/// <example>
		///  "1,2,3,4,5" for int => {1,2,3,4,5}
		///  "1,2,3,4,5" for char => {'1','2','3','4','5'}
		///  "1,2,3,4,5" for string => {"1","2","3","4","5"}
		///  "true,false" for bool => { true, false }
		///  "Black,Blue,Cyan" for ConsoleColor => { ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.Cyan }
		///  "1:00:00,0:00:30" for TimeSpan =>  { new TimeSpan(1, 0, 0), new TimeSpan(0, 0, 30) },
		///  </example>
		public static IEnumerable<T> ConvertToList<T>(this string list) {
			if (list == null)
            {
				return null;
            }

			var splittedList = list.Split(ListSeparator);
			List<T> result = new List<T>();

			foreach(var item in splittedList)
            {
                try
                {
                    result.Add((T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(item));
                }
                catch (Exception)
                {
					continue; // Particular exception is not specified because there are only two types of exceptions might be thrown.
                }
			}

			return result;
		}
	}

	public static class ArrayExtentions {

		/// <summary>
		///   Swaps the one element of source array with another
		/// </summary>
		/// <typeparam name="T">required type of</typeparam>
		/// <param name="array">source array</param>
		/// <param name="index1">first index</param>
		/// <param name="index2">second index</param>
		public static void SwapArrayElements<T>(this T[] array, int index1, int index2) {
			var temp = array[index1];
			array[index1] = array[index2];
			array[index2] = temp;
		}

		/// <summary>
		///   Sorts the tuple array by specified column in ascending or descending order
		/// </summary>
		/// <param name="array">source array</param>
		/// <param name="sortedColumn">index of column</param>
		/// <param name="ascending">true if ascending order required; otherwise false</param>
		/// <example>
		///   source array : 
		///   { 
		///     { 1, "a", false },
		///     { 3, "b", false },
		///     { 2, "c", true  }
		///   }
		///   result of SortTupleArray(array, 0, true) is sort rows by first column in a ascending order: 
		///   { 
		///     { 1, "a", false },
		///     { 2, "c", true  },
		///     { 3, "b", false }
		///   }
		///   result of SortTupleArray(array, 1, false) is sort rows by second column in a descending order: 
		///   {
		///     { 2, "c", true  },
		///     { 3, "b", false }
		///     { 1, "a", false },
		///   }
		/// </example>
		public static void SortTupleArray<T1, T2, T3>(this Tuple<T1, T2, T3>[] array, int sortedColumn, bool ascending) {
			// The following solution is rather complicated because of avoidance LINQ usage.
            switch (sortedColumn)
            {
				case 0:
					T1[] keys1 = Array.Empty<T1>();
					foreach (var item in array)
                    {
						Array.Resize(ref keys1, keys1.Length + 1);
						keys1[keys1.Length - 1] = item.Item1;
                    }

					Array.Sort(keys1, array);
					break;
				case 1:
					T2[] keys2 = Array.Empty<T2>();
					foreach (var item in array)
					{
						Array.Resize(ref keys2, keys2.Length + 1);
						keys2[keys2.Length - 1] = item.Item2;
					}

					Array.Sort(keys2, array);
					break;
				case 2:
					T3[] keys3 = Array.Empty<T3>();
					foreach (var item in array)
					{
						Array.Resize(ref keys3, keys3.Length + 1);
						keys3[keys3.Length - 1] = item.Item3;
					}

					Array.Sort(keys3, array);
					break;
				default:
					throw new IndexOutOfRangeException();
			}

			if (!ascending)
            {
				Array.Reverse(array);
            }
		}
	}

	/// <summary>
	///   Generic singleton class
	/// </summary>
	/// <example>
	///   This code should return the same MyService object every time:
	///   MyService singleton = Singleton<MyService>.Instance;
	/// </example>
	public static class Singleton<T>
		where T : class, new()
	{
		private static object locker = new object();
		private static readonly T instance = new T();

		public static T Instance {
			get 
			{
                lock (locker)
                {
					return instance;
                }
			}
		}
	}



	public static class FunctionExtentions {
		/// <summary>
		///   Tries to invoke the specified function up to 3 times if the result is unavailable 
		/// </summary>
		/// <param name="function">specified function</param>
		/// <returns>
		///   Returns the result of specified function, if WebException occurs duaring request then exception should be logged into trace 
		///   and the new request should be started (up to 3 times).
		/// </returns>
		/// <example>
		///   Sometimes if network is unstable it is required to try several request to get data:
		///   
		///   Func<string> f1 = ()=>(new System.Net.WebClient()).DownloadString("http://www.google.com/");
		///   string data = f1.TimeoutSafeInvoke();
		///   
		///   If the first attemp to download data is failed by WebException then exception should be logged to trace log and the second attemp should be started.
		///   The second attemp has the same workflow.
		///   If the third attemp fails then this exception should be rethrow to the application.
		/// </example>
		public static T TimeoutSafeInvoke<T>(this Func<T> function) {
			int count = 0;
			T res = default;
			do
			{
                try
                {
                    res = function.Invoke();
					break;
                }
                catch (Exception e)
                {
					Trace.WriteLine(e);
					count++;
					if (count >= 3)
                    {
						throw e;
                    }
                }
			}
			while (count <= 3);

			return res;
		}


		/// <summary>
		///   Combines several predicates using logical AND operator 
		/// </summary>
		/// <param name="predicates">array of predicates</param>
		/// <returns>
		///   Returns a new predicate that combine the specified predicated using AND operator
		/// </returns>
		/// <example>
		///   var result = CombinePredicates(new Predicate<string>[] {
		///            x=> !string.IsNullOrEmpty(x),
		///            x=> x.StartsWith("START"),
		///            x=> x.EndsWith("END"),
		///            x=> x.Contains("#")
		///        })
		///   should return the predicate that identical to 
		///   x=> (!string.IsNullOrEmpty(x)) && x.StartsWith("START") && x.EndsWith("END") && x.Contains("#")
		///
		///   The following example should create predicate that returns true if int value between -10 and 10:
		///   var result = CombinePredicates(new Predicate<int>[] {
		///            x=> x>-10,
		///            x=> x<10
		///       })
		/// </example>
		public static Predicate<T> CombinePredicates<T>(Predicate<T>[] predicates) {
			return (item) => 
			{
				foreach (Predicate<T> predicate in predicates)
				{
					if (!predicate(item))
					{
						return false;
					}
				}
				return true;
			};
		}
	}
}
