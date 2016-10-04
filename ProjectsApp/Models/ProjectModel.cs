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

        public Nullable <int> EngineersOnProject { get; set; }

        public Nullable<int> ITsOnProject { get; set; }

        public Nullable<int> ManualWorkersOnProject { get; set; }
        public Nullable<int> ArchitectsOnProject { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date_From { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date_Created { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date_To { get; set; }
      
    }
    }
