//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chair80CP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sec_sessions
    {
        public System.Guid id { get; set; }
        public int user_id { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }
        public string ip { get; set; }
        public string agent { get; set; }
        public string browser { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
        public string isp { get; set; }
        public Nullable<decimal> lat { get; set; }
        public Nullable<decimal> lon { get; set; }
        public string timezone { get; set; }
        public Nullable<int> paltform { get; set; }
        public string device_id { get; set; }
    
        public virtual sec_users sec_users { get; set; }
    }
}
