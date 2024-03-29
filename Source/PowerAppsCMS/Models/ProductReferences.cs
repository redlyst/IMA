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
    
    public partial class ProductReferences
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductReferences()
        {
            this.Products = new HashSet<Products>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProductGroupID { get; set; }
        public decimal MHPBIH { get; set; }
        public decimal MHPBOH { get; set; }
        public decimal MHFabIH { get; set; }
        public decimal MHFabOH { get; set; }
        public decimal MHPaintingIH { get; set; }
        public decimal MHPaintingOH { get; set; }
        public decimal Factor { get; set; }
        public bool IsOperatorOr { get; set; }
        public System.DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    
        public virtual ProductGroup ProductGroups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
