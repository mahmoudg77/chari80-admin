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
    
    public partial class trip_share_details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public trip_share_details()
        {
            this.trip_book = new HashSet<trip_book>();
        }
    
        public int id { get; set; }
        public int trip_share_id { get; set; }
        public Nullable<int> seats { get; set; }
        public Nullable<System.DateTime> start_at_date { get; set; }
        public Nullable<System.TimeSpan> start_at_time { get; set; }
        public Nullable<decimal> from_lat { get; set; }
        public Nullable<decimal> from_lng { get; set; }
        public string from_plc { get; set; }
        public Nullable<decimal> to_lat { get; set; }
        public Nullable<decimal> to_lng { get; set; }
        public string to_plc { get; set; }
        public Nullable<int> gender_id { get; set; }
        public Nullable<int> booked_seats { get; set; }
        public Nullable<decimal> seat_cost { get; set; }
        public Nullable<int> trip_direction { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<System.Guid> guid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<trip_book> trip_book { get; set; }
        public virtual trip_share trip_share { get; set; }
        public virtual tbl_genders tbl_genders { get; set; }
    }
}
