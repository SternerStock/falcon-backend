
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
    
public partial class Pricing
{

    public int ID { get; set; }

    public int PrintingID { get; set; }

    public System.DateTime Date { get; set; }

    public double Price { get; set; }

    public bool Foil { get; set; }



    public virtual Printing Printing { get; set; }

}

}