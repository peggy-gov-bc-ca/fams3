﻿using System.ComponentModel;

namespace BcGov.Fams3.SearchApi.Contracts.Person
{
    public class Address : PersonalInfo
    { 
        [Description("The type of address")]
        public string Type { get; set; }

        [Description("The Address Line 1")]
        public string AddressLine1 { get; set; }

        [Description("The Address Line 2")]
        public string AddressLine2 { get; set; }

        [Description("The Address Line 2")]
        public string AddressLine3 { get; set; }

        [Description("The Address Province or state")]
        public string StateProvince { get; set; }

        [Description("The Address City")]
        public string City { get; set; }

        [Description("The Address Country")]
        public string CountryRegion { get; set; }

        [Description("The Address Zip or Postal Code")]
        public string ZipPostalCode { get; set; }

    }
}
