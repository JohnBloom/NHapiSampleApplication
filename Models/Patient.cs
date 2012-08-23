using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapiSampleApplication.Models
{
    public class Patient
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
