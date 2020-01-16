using AutoMapper;
using DynamicsAdapter.Web.PersonSearch.Models;
using Fams3Adapter.Dynamics.Address;
using Fams3Adapter.Dynamics.Identifier;
using Fams3Adapter.Dynamics.Name;
using System;
using System.Collections.Generic;
using System.Linq;
using BcGov.Fams3.SearchApi.Contracts.Person;
using System.Threading.Tasks;

namespace DynamicsAdapter.Web.Mapping
{
    public class FullTextResolver : IValueResolver<Address, SSG_Address, string>
    {
        public string Resolve(Address source, SSG_Address dest, string fullText, ResolutionContext context)
        {
            return $"{source.AddressLine1} {source.AddressLine2} {source.AddressLine3} {source.City} {source.StateProvince} {source.CountryRegion} {source.ZipPostalCode}";
        }
    }

    public class FullNameResolver : IValueResolver<Name, SSG_Aliase, string>
    {
        public string Resolve(Name source, SSG_Aliase dest, string fullName, ResolutionContext context)
        {
            return $"{source.FirstName} {source.MiddleName} {source.LastName}";
        }
    }

    public class ReferenceDatesResolver : IValueResolver<SSG_Identifier, PersonalIdentifierActual, IEnumerable<BcGov.Fams3.SearchApi.Contracts.Person.ReferenceDate>>
    {
        public IEnumerable<BcGov.Fams3.SearchApi.Contracts.Person.ReferenceDate> Resolve(SSG_Identifier source, PersonalIdentifierActual dest, IEnumerable<BcGov.Fams3.SearchApi.Contracts.Person.ReferenceDate> destMember, ResolutionContext context)
        {
            var list = new List<BcGov.Fams3.SearchApi.Contracts.Person.ReferenceDate>();
            if (source.IdentificationEffectiveDate != null )
            {
                ReferenceDateActual issuedDate = new ReferenceDateActual()
                {
                    Index = 0,
                    Key = nameof(source.IdentificationEffectiveDate),
                    Value =  (DateTime)source.IdentificationEffectiveDate
                };
                list.Add(issuedDate);
            }

            if (source.IdentificationExpirationDate != null)
            {
                ReferenceDateActual expiredDate = new ReferenceDateActual()
                {
                    Index = 1,
                    Key = nameof(source.IdentificationExpirationDate),
                    Value = (DateTime)source.IdentificationExpirationDate
                };
                list.Add(expiredDate);
            }

            return list;
        }
    }

    public class IdentificationEffectiveDateResolver : IValueResolver<PersonalIdentifier, SSG_Identifier, DateTime?>
    {
        public DateTime? Resolve(PersonalIdentifier source, SSG_Identifier dest, DateTime? destMember, ResolutionContext context)
        {
            return source.ReferenceDates?.SingleOrDefault(m => m.Index == 0)?.Value.DateTime;
        }
    }

    public class IdentificationExpirationDateResolver : IValueResolver<PersonalIdentifier, SSG_Identifier, DateTime?>
    {
        public DateTime? Resolve(PersonalIdentifier source, SSG_Identifier dest, DateTime? destMember, ResolutionContext context)
        {
            return source.ReferenceDates?.SingleOrDefault(m => m.Index == 1)?.Value.DateTime; 
        }
    }
}
