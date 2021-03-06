using Newtonsoft.Json;
using System;
using Fams3Adapter.Dynamics.SearchRequest;
using Fams3Adapter.Dynamics.SearchApiRequest;
using Fams3Adapter.Dynamics.Person;

namespace Fams3Adapter.Dynamics.Identifier
{
    public class SSG_Identifier : DynamicsEntity
    {
        [JsonProperty("ssg_identification")]
        public string Identification { get; set; }

        [JsonProperty("ssg_suppliertypecode")]
        public string SupplierTypeCode { get; set; }

        [JsonProperty("ssg_identificationcategorytext")]
        public int? IdentifierType { get; set; }

        [JsonProperty("ssg_notes")]
        public string Notes { get; set; }

        [JsonProperty("ssg_description")]
        public string Description { get; set; }

        [JsonProperty("ssg_issuedby")]
        public string IssuedBy { get; set; }

        [JsonProperty("ssg_SearchRequest")]
        public virtual SSG_SearchRequest SearchRequest { get; set; }

        [JsonProperty("ssg_PersonId")]
        public virtual SSG_Person Person { get; set; }

        [JsonProperty("ssg_informationsourcetext")]
        public int? InformationSource { get; set; }

     }

}
