﻿using BcGov.Fams3.SearchApi.Contracts.Person;
using BcGov.Fams3.SearchApi.Contracts.PersonSearch;
using BcGov.Fams3.SearchApi.Core.Adapters.Configuration;
using BcGov.Fams3.SearchApi.Core.Adapters.Models;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchAdapter.Sample.SearchRequest
{
    /// <summary>
    /// The SearchRequestConsumer consumes search ordered events, execute the search a publish a response back to the searchApi
    /// </summary>
    public class SearchRequestConsumer : IConsumer<PersonSearchOrdered>
    {

        private readonly ILogger<SearchRequestConsumer> _logger;
        private readonly ProviderProfile _profile;
        private readonly IValidator<Person> _personSearchValidator;


        public SearchRequestConsumer(
            IValidator<Person> personSearchValidator,
            IOptions<ProviderProfileOptions> profile,
            ILogger<SearchRequestConsumer> logger)
        {
            _personSearchValidator = personSearchValidator;
            _profile = profile.Value;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PersonSearchOrdered> context)
        {
            _logger.LogInformation($"Successfully handling new search request [{context.Message.Person}]");

            _logger.LogWarning("Sample Adapter, do not use in PRODUCTION.");

            if (await ValidatePersonSearch(context))
            {

                if (string.Equals(context.Message.Person.FirstName, "exception", StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("Exception from Sample Adapter, Your name is no exception");

                await context.Publish<PersonSearchCompleted>(BuildFakePersonSearchCompleted(context.Message));
            }
        }

        private async Task<bool> ValidatePersonSearch(ConsumeContext<PersonSearchOrdered> context)
        {

            _logger.LogDebug("Attempting to validate the personSearch");
            var validation = _personSearchValidator.Validate(context.Message.Person);

            if (validation.IsValid)
            {
                await context.Publish<PersonSearchAccepted>(new DefaultPersonSearchAccepted(context.Message.SearchRequestId, _profile, context.Message.FileId));
            }
            else
            {
                _logger.LogInformation("PersonSearch does not have sufficient information for the adapter to proceed the search.");

                var rejectionEvent = new PersonSearchRejectedEvent(context.Message.SearchRequestId, context.Message.FileId, _profile);

                validation.Errors.ToList().ForEach(x => rejectionEvent.AddValidationResult(new DefaultValidationResult()
                {
                    PropertyName = x.PropertyName,
                    ErrorMessage = x.ErrorMessage
                }));

                await context.Publish<PersonSearchRejected>(rejectionEvent);
            }

            return validation.IsValid;

        }

        public PersonSearchCompleted BuildFakePersonSearchCompleted(PersonSearchOrdered personSearchOrdered)
        {

            return new PersonSearchCompletedSample()
            {
                ProviderProfile = _profile,
                SearchRequestId = personSearchOrdered.SearchRequestId,
                FileId = personSearchOrdered.FileId,
                TimeStamp = DateTime.Now,
                MatchedPersons = new List<Person>()
                {
                    new Person(){
                        FirstName = personSearchOrdered.Person.FirstName,
                        LastName = personSearchOrdered.Person.LastName,
                        DateOfBirth = personSearchOrdered.Person.DateOfBirth,
                        Incacerated = "N",
                        DateDeathConfirmed = false,
                        DateOfDeath = null,
                        Gender = "F",
                        MiddleName = "FoundMiddleName",
                        OtherName = "FoundOtherName",
                        Notes = "some notes",
                        Height = "178",
                        Complexion = "Dark",
                        DistinguishingFeatures = "None",
                        EyeColour = "Hazel",
                        HairColour = "Blonde",
                        WearGlasses = "No",
                        Weight= "200",
                        Identifiers = new List<PersonalIdentifier>()
                        {

                            new PersonalIdentifier()
                            {
                                Type = PersonalIdentifierType.BCDriverLicense,
                                TypeCode = "BCDL",
                                Description = "Sample Identifier Description",
                                Notes = "Sample Identifier Notes",
                                IssuedBy = "BC",
                                Value = new Random().Next(0, 50000).ToString(),
                                ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Effective Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="Expiration Date", Value=new DateTime(2020,9,1) }
                                }


                            },
                             new PersonalIdentifier()
                            {
                                Type = PersonalIdentifierType.BCID,
                                TypeCode = "BCID",
                                Description = "Sample Identifier Description",
                                Notes = "Sample Identifier Notes",
                                IssuedBy = "BC",
                                Value = new Random().Next(0, 50000).ToString(),
                                ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Effective Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="Expiration Date", Value=new DateTime(2020,9,1) }
                                }


                            }

                        },
                        Addresses = new List<Address>()
                        {
                            new Address()
                            {
                                Type = "mailing",
                                AddressLine1 = "address in line 1",
                                AddressLine2 = "address in line 2",
                                AddressLine3 = "address in line 3",
                                StateProvince = "British Columbia",
                                City = "victoria" ,
                                CountryRegion= "canada",
                                ZipPostalCode = "t4t4t4",
                                ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="End Date", Value=new DateTime(2020,9,1) }
                                },
                                Description = "description",
                                Notes = "notes"
                            },
                             new Address()
                            {
                                Type = "unknown",
                                AddressLine1 = "residence address in line 1",
                                AddressLine2 = "residence address in line 2",
                                StateProvince = "British Columbia",
                                City = "vancouver" ,
                                CountryRegion="canada",
                                ZipPostalCode = "5r5r5r",
                                ReferenceDates = null,
                                Description = "description2",
                                Notes = "notes2"
                            }
                        },
                        Names = new List<Name>()
                        {
                            new Name()
                            {
                                Type = "legal name",
                                FirstName = "firstName",
                                LastName = "LastName",
                                MiddleName = "MiddleName",
                                  ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="End Date", Value=new DateTime(2020,9,1) }
                                },
                                Description = "Sample Name"
                            }
                        },
                        Phones = new List<Phone>()
                        {
                            new Phone
                            {
                                Description = "Sample Phone",
                                Extension = "1233",
                                Notes = "Notes",
                                PhoneNumber ="768990123",
                                Type = "home",
                                 ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="End Date", Value=new DateTime(2020,9,1) }
                                }

                            },
                            new Phone
                            {
                                Description = "Sample Phone",
                                Extension = "1233",
                                PhoneNumber ="768990123",
                                Type = "work",
                                 ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="End Date", Value=new DateTime(2020,9,1) }
                                }
                            }

                        },
                        Employments = new List<Employment>()
                        {
                            new Employment
                            {
                                EmploymentConfirmed = true,
                                IncomeAssistance = true,
                                IncomeAssistanceStatus = "Real Status",
                                 Notes = "Sample Notes",
                                 Occupation = "Occupation",
                                 ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="End Date", Value=new DateTime(2020,9,1) }
                                },
                                 Website = "www.websitejob.com",
                                 Employer = new Employer
                                 {
                                     Address = new Address
                                     {
                                         AddressLine1 = "Address 1",
                                         AddressLine2 = "Address 2",
                                         AddressLine3 = "Address 3",
                                         City = "City",
                                         StateProvince = "AB",
                                         CountryRegion = "Canada",
                                         ZipPostalCode = "VR4 123"
                                     },
                                     ContactPerson = "Surname FirstName",
                                     Name = "Sample Company",
                                     OwnerName = "Sample Company Owner",
                                     Phones = new List<Phone>()
                                     {
                                         new Phone {PhoneNumber = "12345678", Extension ="123", Type ="Phone"},
                                         new Phone {PhoneNumber = "901237123", Extension ="123", Type ="Fax"},
                                         new Phone {PhoneNumber = "762349303", Extension ="123", Type ="Phone"}
                                     }
                                 }
                            },
                              new Employment
                            {
                                EmploymentConfirmed = false,
                                IncomeAssistance = false,
                                IncomeAssistanceStatus = "Real Status",
                                 Notes = "Sample Notes",
                                 Occupation = "Occupation",
                                 ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="End Date", Value=new DateTime(2020,9,1) }
                                },
                                 Website = "www.websitejob.com",
                                 Employer = new Employer
                                 {
                                     Address = new Address
                                     {
                                         AddressLine1 = "Address 1",
                                         AddressLine2 = "Address 2",
                                         AddressLine3 = "Address 3",
                                         City = "City",
                                         StateProvince = "AB",
                                         CountryRegion = "Canada",
                                         ZipPostalCode = "VR4 123"
                                     },
                                     ContactPerson = "Surname FirstName",
                                     Name = "Sample Company",
                                     OwnerName = "Sample Company Owner",
                                     Phones = new List<Phone>()
                                     {
                                         new Phone {PhoneNumber = "12345678", Extension ="123", Type ="Phone"},
                                         new Phone {PhoneNumber = "901237123", Extension ="123", Type ="Fax"},
                                         new Phone {PhoneNumber = "762349303", Extension ="123", Type ="Phone"}
                                     }
                                 }
                            }
                        },
                        RelatedPersons = new List<RelatedPerson>()
                        {
                            new RelatedPerson
                            {
                                Description = "RelatedPersonDescription",
                                DateOfBirth = new DateTime(1987, 1,1),
                                Notes = "RelatedPerson Notes",
                                FirstName ="SampleRelateFirst",
                                LastName="SampleRelateLast",
                                MiddleName="SampleRelateMiddle",
                                OtherName="SampleRelateOther",
                                Type = "Wife",
                                Gender="F",
                                 ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Relation Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="Relation End Date", Value=new DateTime(2020,9,1) }
                                }
                            }
                        },
                        BankInfos = new List<BankInfo>()
                        {
                            new BankInfo()
                            {
                                BankName="BankName",
                                AccountNumber="AccountNumber123",
                                Branch="Branch",
                                TransitNumber="TransitNumber123",
                                ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Account Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="Account Expired Date", Value=new DateTime(2020,9,1) }
                                },
                                Description="Description",
                                Notes="Notes"
                            }
                        },
                        Vehicles = new List<Vehicle>()
                        {
                            new Vehicle()
                            {
                                OwnershipType="SINGLE OWNER",
                                Vin="ABCDEFGHIJK",
                                PlateNumber="AAA.BBB",
                                ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Account Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="Account Expired Date", Value=new DateTime(2020,9,1) }
                                },
                                Description="2020 BRAND TRUCK $DR / CAR AWD",
                                Notes="vehicle Notes",
                                Owners=new List<AssetOwner>()
                                {
                                    new AssetOwner()
                                    {
                                        FirstName = "owner first name",
                                        LastName = "owner last name",
                                        MiddleName ="owner middle name",
                                        OtherName = "owner other name",
                                        Description="",
                                        Type="lease",
                                        Notes="notes",
                                        OrganizationName="TRUCK COMPANY INC."
                                    }
                                }
                            }
                        },
                        OtherAssets= new List<OtherAsset>()
                        {
                            new OtherAsset()
                            {
                                TypeDescription="type description",
                                ReferenceDescription="reference description",
                                ReferenceValue="reference value",
                                ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="Expired Date", Value=new DateTime(2020,9,1) }
                                },
                                Description="other asset description",
                                Notes="other asset Notes",
                                Owners=new List<AssetOwner>()
                                {
                                    new AssetOwner()
                                    {
                                        FirstName = "owner other first name",
                                        LastName = "owner other last name",
                                        MiddleName ="owner other middle name",
                                        OtherName = "owner other other name",
                                        Description="other description",
                                        Type="other type",
                                        Notes="notes",
                                        OrganizationName="owner other org name"
                                    }
                                }
                            }
                        },
                        CompensationClaims = new List<CompensationClaim>(){
                            new CompensationClaim()
                            {
                                ClaimNumber="claimNumber",
                                ClaimantNumber="claimant121",
                                ClaimStatus="Processing",
                                ClaimType="Disable compensation",
                                Description="dis",
                                Notes="compensation notes",
                                ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="compensation Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="compensation Expired Date", Value=new DateTime(2020,9,1) }
                                },
                                BankInfo=new BankInfo()
                                {
                                    BankName="compensation BankName",
                                    AccountNumber="compensation AccountNumber123",
                                    Branch="Branch-compensation",
                                    TransitNumber="TransitNumber123-compensation",
                                    ReferenceDates = new List<ReferenceDate>(){
                                        new ReferenceDate(){ Index=0, Key="Account Start Date", Value=new DateTime(2019,9,1) },
                                        new ReferenceDate(){ Index=1, Key="Account Expired Date", Value=new DateTime(2020,9,1) }
                                    },
                                    Description="compensation bank Description",
                                    Notes="compensation bank Notes"
                                },
                                Employer=new Employer()
                                {
                                     Address = new Address
                                     {
                                         AddressLine1 = "compensation Employer Address 1",
                                         AddressLine2 = "compensation Employer Address 2",
                                         AddressLine3 = "compensation Employer Address 3",
                                         City = "compensation Employer City",
                                         StateProvince = "AB",
                                         CountryRegion = "Canada",
                                         ZipPostalCode = "VR4 123"
                                     },
                                     ContactPerson = "compensation Employer Surname FirstName",
                                     Name = "compensation Employer Sample Company",
                                     OwnerName = "compensation Employer Sample Company Owner",
                                     Phones = new List<Phone>()
                                     {
                                         new Phone {PhoneNumber = "11111111", Extension ="123", Type ="Phone"},
                                         new Phone {PhoneNumber = "22222222", Extension ="123", Type ="Fax"},
                                         new Phone {PhoneNumber = "33333333", Extension ="123", Type ="Phone"}
                                     }
                                }
                            }
                        }
                    }
                }
            };

        }

    }
}