﻿using Fams3Adapter.Dynamics.Address;
using Fams3Adapter.Dynamics.Person;
using Fams3Adapter.Dynamics.PhoneNumber;
using Fams3Adapter.Dynamics.SearchRequest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fams3Adapter.Dynamics.Employment
{
    public class SSG_EmploymentContact 
    {

        [JsonProperty("ssg_description")]
        public string Description { get; set; }

        [JsonProperty("ssg_emailaddress")]
        public string Email { get; set; }

        [JsonProperty("ssg_faxnumber")]
        public string FaxNumber { get; set; }

        [JsonProperty("ssg_phoneextension")]
        public string PhoneExtension { get; set; }

        [JsonProperty("ssg_phonenumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("ssg_phonetype")]
        public string PhoneType { get; set; }

        [JsonProperty("ssg_EmploymentId")]
        public virtual SSG_Employment Employment { get; set; }

        [JsonProperty("statecode")]
        public int StateCode { get; set; }

        [JsonProperty("statuscode")]
        public int StatusCode { get; set; }
    }


}
