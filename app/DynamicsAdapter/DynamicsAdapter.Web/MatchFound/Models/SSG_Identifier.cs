using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicsAdapter.Web.MatchFound.Models
{
    public class SSG_Identifier
    {
        //public Guid? ssg_identifierid { get; set; }
        public string SSG_Identification { get; set; }
        public int StatusCode { get; set; }
        public int  StateCode { get; set; }
        public DateTime ssg_identificationeffectivedate { get; set; }
        public Guid ssg_SearchAPIRequest { get; set; }
        //public String SSG_Idssg_SearchAPIRequestentificationCategoryText { get; set; }

        //public string SSG_SearchApiRequest { get; set; } //SearchApiRequestID
        //public string ssg_SearchAPIRequest { get; set; } //SearchApiRequestID
        //public string SSG_InforamtionSourceText { get; set; }
    }
}
