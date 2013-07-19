//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eStoreModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public Customer()
        {
            this.Orders = new HashSet<Order>();
            this.webpages_Roles = new HashSet<webpages_Roles>();
        }
    
        public int CustomerID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<int> Age { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string Mailcode { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Creditcardtype { get; set; }
    
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }
    }
}