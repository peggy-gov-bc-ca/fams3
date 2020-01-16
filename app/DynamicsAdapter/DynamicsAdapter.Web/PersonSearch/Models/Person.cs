using BcGov.Fams3.SearchApi.Contracts.Person;
using Fams3Adapter.Dynamics.Identifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicsAdapter.Web.PersonSearch.Models
{
    public class PersonalIdentifierActual :PersonalIdentifier, BcGov.Fams3.SearchApi.Contracts.Person.PersonalIdentifier
    {
        public string Value { get; set; }

        public BcGov.Fams3.SearchApi.Contracts.Person.PersonalIdentifierType Type { get; set; }

        public string TypeCode { get; set; }

        public string IssuedBy { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }

        public IEnumerable<BcGov.Fams3.SearchApi.Contracts.Person.ReferenceDate> ReferenceDates { get; set; }
    }

    public class PhoneNumberActual :PhoneNumber, BcGov.Fams3.SearchApi.Contracts.Person.PhoneNumber
    {
        public string SuppliedBy { get; set; }

        public DateTime? Date { get; set; }

        public string DateType { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneNumberType { get; set; }
    }

    public class AddressActual : Address, BcGov.Fams3.SearchApi.Contracts.Person.Address
    {
        public string Type { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string StateProvince { get; set; }

        public string City { get; set; }

        public string CountryRegion { get; set; }

        public string ZipPostalCode { get; set; }

        public string SuppliedBy { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class NameActual : Name, BcGov.Fams3.SearchApi.Contracts.Person.Name
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Type { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; }
    }

    public class ReferenceDateActual : ReferenceDate, BcGov.Fams3.SearchApi.Contracts.Person.ReferenceDate
    {
        public int Index { get; set; }

        public string Key { get; set; }

        public DateTime Value { get; set; }
    }

    public class PersonActual : PersonSearchRequest, BcGov.Fams3.SearchApi.Contracts.Person.Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public IEnumerable<BcGov.Fams3.SearchApi.Contracts.Person.PersonalIdentifier> Identifiers { get; set; }

        public IEnumerable<BcGov.Fams3.SearchApi.Contracts.Person.Address> Addresses { get; set; }

        public IEnumerable<BcGov.Fams3.SearchApi.Contracts.Person.PhoneNumber> PhoneNumbers { get; set; }

        public IEnumerable<BcGov.Fams3.SearchApi.Contracts.Person.Name> Names { get; set; }
    }
}
