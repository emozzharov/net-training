using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace Serialization.Tasks
{
    // TODO : Make Company class xml-serializable using DataContractSerializer 
    // Employee.Manager should be serialized as reference
    // Company class has to be forward compatible with all derived versions

    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Serialization.Tasks")]
    //[KnownType("GetDerivedTypes")]
    [KnownType(typeof(IList<Employee>))]
    public class Company
    {
        [DataMember]
        public string Name { get; set; }

        //[DataMember]
        //public string Address { get; set; }

        //[DataMember]
        //public virtual long Capitalization { get; set; }

        [DataMember]
        public IList<Employee> Employee { get; set; }

        private static IEnumerable<Type> GetDerivedTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(t => t.GetTypes()).Where(t => typeof(Company).IsAssignableFrom(t));

            return types;
        }
    }

    [DataContract(IsReference = true, Namespace = "http://schemas.datacontract.org/2004/07/Serialization.Tasks")]
    public abstract class Employee {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public Manager Manager { get; set; }
    }

    [DataContract()]
    public class Worker : Employee {
        [DataMember()]
        public int Salary { get; set; }
    }

    [DataContract(IsReference = true)]
    public class Manager : Employee {
        [DataMember()]
        public int YearBonusRate { get; set; } 
    }

}
