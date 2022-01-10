using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Reflection.Tasks
{
    public static class CommonTasks
    {

        /// <summary>
        /// Returns the lists of public and obsolete classes for specified assembly.
        /// Please take attention: classes (not interfaces, not structs)
        /// </summary>
        /// <param name="assemblyName">name of assembly</param>
        /// <returns>List of public but obsolete classes</returns>
        public static IEnumerable<string> GetPublicObsoleteClasses(string assemblyName) {
            return Assembly.Load(assemblyName).GetTypes().Where(t => t.IsClass && t.IsPublic && System.Attribute.GetCustomAttributes(t).Any(a =>
            {
                if (a is ObsoleteAttribute)
                {
                    return true;
                }

                return false;
            })).Select(t => t.Name);
        }

        /// <summary>
        /// Returns the value for required property path
        /// </summary>
        /// <example>
        ///  1) 
        ///  string value = instance.GetPropertyValue("Property1")
        ///  The result should be equal to invoking statically
        ///  string value = instance.Property1;
        ///  2) 
        ///  string name = instance.GetPropertyValue("Property1.Property2.FirstName")
        ///  The result should be equal to invoking statically
        ///  string name = instance.Property1.Property2.FirstName;
        /// </example>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="obj">source object to get property from</param>
        /// <param name="propertyPath">dot-separated property path</param>
        /// <returns>property value of obj for required propertyPath</returns>
        public static T GetPropertyValue<T>(this object obj, string propertyPath) {
            Type type = obj.GetType();

            var res = GetProperty(obj, propertyPath);

            try
            {
                return (T)res;
            }
            catch
            {
                return default;
            }

            object GetProperty(object incomingObject, string propName)
            {
                var splittedPath = propName.Split(new char[] { '.' }, 2);
                Type propType = incomingObject.GetType();

                if (propType.GetProperties().Any(p => p.Name.Equals(splittedPath[0])) && splittedPath.Length > 1)
                {
                    return GetProperty(propType.GetProperties()
                        .SingleOrDefault(p => p.Name.Equals(splittedPath[0])).GetValue(incomingObject), splittedPath[1]);
                }

                if (splittedPath.Length > 1 && propType.GetProperties().Any(p => p.Name.Equals(splittedPath[1])))
                {
                    return propType.GetProperties().SingleOrDefault(p => p.Name.Equals(splittedPath[1])).GetValue(incomingObject);
                }

                // returns property of the incoming object.
                return propType.GetProperties().SingleOrDefault(p => p.Name == propName).GetValue(incomingObject);
            }
        }


        /// <summary>
        /// Assign the value to the required property path
        /// </summary>
        /// <example>
        ///  1)
        ///  instance.SetPropertyValue("Property1", value);
        ///  The result should be equal to invoking statically
        ///  instance.Property1 = value;
        ///  2)
        ///  instance.SetPropertyValue("Property1.Property2.FirstName", value);
        ///  The result should be equal to invoking statically
        ///  instance.Property1.Property2.FirstName = value;
        /// </example>
        /// <param name="obj">source object to set property to</param>
        /// <param name="propertyPath">dot-separated property path</param>
        /// <param name="value">assigned value</param>
        public static void SetPropertyValue(this object obj, string propertyPath, object value) {
            Type type = obj.GetType();

            SetProperty(obj, propertyPath, value);

            object SetProperty(object incomingObject, string propName, object val)
            {
                var props = incomingObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (!propName.Contains('.'))
                {
                    var parentClass = incomingObject.GetType().BaseType;

                    parentClass.GetProperty(propName).GetSetMethod(true).Invoke(incomingObject, new object[] { val });
                    return null;
                }

                var splittedPath = propName.Split(new char[] { '.' }, 2);
                if (splittedPath.Length > 1 && props.Any(p => p.Name == splittedPath[0]))
                {
                    return SetProperty(props.FirstOrDefault(p => p.Name == splittedPath[0]).GetValue(incomingObject), splittedPath[1], val);
                }

                props.FirstOrDefault(p => p.Name == propName).SetValue(incomingObject, val);
                return null;
            }
        }


    }
}
