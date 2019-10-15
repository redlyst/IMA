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
    
    public partial class SalesOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SalesOrder()
        {
            this.PROSalesOrders = new HashSet<PROSalesOrder>();
            this.Units = new HashSet<Unit>();
        }
    
        public int ID { get; set; }
        public string Number { get; set; }
        public string CustomerName { get; set; }
        public string PartNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string LineItem { get; set; }
        public int Quantity { get; set; }
        public System.DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public string DocumentType { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public string Customer { get; set; }
        public string CustomerPONo { get; set; }
        public Nullable<System.DateTime> CustomerPODate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string MaterialDescription { get; set; }
        public Nullable<int> Plant { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROSalesOrder> PROSalesOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Unit> Units { get; set; }
    }
}
