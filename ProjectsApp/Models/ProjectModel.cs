using ProjectsApp.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectsApp.Models
{
    public class ProjectModel
    {

        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Cap { get; set; }
        public string IsActive { get; set; }
        public string Client_Id { get; set; }
        public string CreatedBy { get; set; }

        public int EngineersOnProject { get; set; }

        public int ITsOnProject { get; set; }

        public int ManualWorkersOnProject { get; set; }
        public int ArchitectsOnProject { get; set; }


        public int TotalStaff { get; set; }
        public decimal EstimatedEarnings { get; set; }
        public decimal RiskFactor { get; set; }

        public string ModifiedBy { get; set; }



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date_From { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date_Created { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date_To { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; }

    }
    }
