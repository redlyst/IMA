//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PowerAppsCMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProcessDependency
    {
        public int ID { get; set; }
        public int MasterProcessID { get; set; }
        public Nullable<int> PredecessorProcessID { get; set; }
        public System.DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    
        public virtual MasterProcess MasterProcess { get; set; }
        public virtual MasterProcess MasterProcessPredecessor { get; set; }
    }
}
