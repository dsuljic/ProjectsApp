//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectsApp.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Projects
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_From { get; set; }
        public System.DateTime Date_To { get; set; }
        public decimal Cap { get; set; }
        public byte IsActive { get; set; }
        public int Client_Id { get; set; }
        public Nullable<byte> IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> TotalStaff { get; set; }
        public Nullable<int> EngineersOnProject { get; set; }
        public Nullable<int> ITsOnProject { get; set; }
        public Nullable<int> ManualWorkersOnProject { get; set; }
        public Nullable<int> ArchitectsOnProject { get; set; }
        public Nullable<decimal> EstimatedEarnings { get; set; }
        public Nullable<decimal> RiskFactor { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Users Users { get; set; }
    }
}
