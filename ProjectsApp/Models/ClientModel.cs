using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsApp.Models
{
    public class ClientModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string MainContact { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }
        public string CreatedBy { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}