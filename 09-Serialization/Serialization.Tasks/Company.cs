using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Serialization.Tasks
{
    // TODO : Make Company class xml-serializable using DataContractSerializer 
    // Employee.Manager should be serialized as reference
    // Company class has to be forward compatible with all derived versions

    //class Person : IExtensibleDataObject
    //{
    //    // To implement the IExtensibleDataObject interface,
    //    // you must implement the ExtensionData property. The property
    //    // holds data from future versions of the class for backward
    //    // compatibility.
    //    private ExtensionDataObject extensionDataObject_value;
    //    public ExtensionDataObject ExtensionData
    //    {
    //        get;

    //        set;

    //    }
    //    [DataMember]
    //    public string Name;
    //}

    [DataContract(IsReference = true)]
    [KnownType(typeof(Worker))]
    public class Company : IExtensibleDataObject
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IList<Employee> Employee { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract]
    [KnownType(typeof(Manager))]
    public abstract class Employee
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public Manager Manager { get; set; }
    }

    [DataContract]
    public class Worker : Employee
    {

        [DataMember]
        public int Salary { get; set; }
    }

    [DataContract]
    public class Manager : Employee
    {

        [DataMember]
        public int YearBonusRate { get; set; }
    }
}
