
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Falcon.MtG
{

using System;
    using System.Collections.Generic;
    
public partial class Set
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Set()
    {

        this.Printings = new HashSet<Printing>();

    }


    public int ID { get; set; }

    public string Code { get; set; }

    public string KeyruneCode { get; set; }

    public string Name { get; set; }

    public System.DateTime Date { get; set; }

    public Nullable<int> SetTypeID { get; set; }

    public Nullable<int> BlockID { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Printing> Printings { get; set; }

    public virtual SetType SetType { get; set; }

    public virtual Block Block { get; set; }

}

}
