using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicsAdapter.Web.MatchFound.Models
{
    public class SSG_Identifier
    {
        public DateTime SSG_IdentificationExpirationDate { get; set; }
        public DateTime SSG_IdentificationEffectiveDate { get; set; }
        public string SSG_Identification { get; set; }
        public String SSG_IdentificationCategoryText { get; set; }
        
        public string SSG_SearchApiRequest { get; set; } //SearchApiRequestID
        public string SSG_InforamtionSourceText { get; set; }
    }
}
