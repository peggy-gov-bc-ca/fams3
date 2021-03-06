﻿using AutoMapper;
using DynamicsAdapter.Web.PersonSearch;
using DynamicsAdapter.Web.PersonSearch.Models;
using Fams3Adapter.Dynamics.Address;
using Fams3Adapter.Dynamics.AssetOwner;
using Fams3Adapter.Dynamics.BankInfo;
using Fams3Adapter.Dynamics.Employment;
using Fams3Adapter.Dynamics.Identifier;
using Fams3Adapter.Dynamics.Name;
using Fams3Adapter.Dynamics.OtherAsset;
using Fams3Adapter.Dynamics.Person;
using Fams3Adapter.Dynamics.PhoneNumber;
using Fams3Adapter.Dynamics.RelatedPerson;
using Fams3Adapter.Dynamics.SearchRequest;
using Fams3Adapter.Dynamics.Vehicle;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicsAdapter.Web.Test.PersonSearch
{
    public class SearchResultServiceTest
    {
        private SearchResultService _sut;
        private Mock<ILogger<SearchResultService>> _loggerMock;
        private Mock<ISearchRequestService> _searchRequestServiceMock;
        private Person _fakePerson;

        private SSG_Identifier _fakePersoneIdentifier;
        private SSG_Address _fakePersonAddress;
        private SSG_PhoneNumber _fakePersonPhoneNumber;
        private SSG_Aliase _fakeName;
        private SSG_Identity _fakeRelatedPerson;
        private EmploymentEntity _fakeEmployment;
        private SSG_EmploymentContact _fakeEmploymentContact;
        private SSG_Asset_BankingInformation _fakeBankInfo;
        private PersonEntity _ssg_fakePerson;
        private SSG_AssetOwner _fakeAssetOwner;
        private VehicleEntity _fakeVehicleEntity;
        private AssetOtherEntity _fakeOtherAsset;
        private ProviderProfile _providerProfile;
        private SSG_SearchRequest _searchRequest;
        private CancellationToken _fakeToken;

        private Mock<IMapper> _mapper;

        [SetUp]
        public void Init()
        {

            _loggerMock = new Mock<ILogger<SearchResultService>>();
            _searchRequestServiceMock = new Mock<ISearchRequestService>();
            _mapper = new Mock<IMapper>();
            var validRequestId = Guid.NewGuid();
            var invalidRequestId = Guid.NewGuid();
            var validVehicleId = Guid.NewGuid();
            var validOtherAssetId = Guid.NewGuid();

            _fakePersoneIdentifier = new SSG_Identifier
            {
                SearchRequest = new SSG_SearchRequest
                {
                    SearchRequestId = validRequestId
                }
            };

            _fakePersonAddress = new SSG_Address
            {
                SearchRequest = new SSG_SearchRequest
                {
                    SearchRequestId = validRequestId
                }
            };
            _fakeEmployment = new EmploymentEntity
            {
                SearchRequest = new SSG_SearchRequest
                {
                    SearchRequestId = validRequestId
                }
            };

            _fakeEmploymentContact = new SSG_EmploymentContact
            {
                PhoneNumber = "11111111"
            };

            _fakePersonPhoneNumber = new SSG_PhoneNumber
            {
                SearchRequest = new SSG_SearchRequest
                {
                    SearchRequestId = validRequestId
                }
            };

            _fakeRelatedPerson = new SSG_Identity
            {
                SearchRequest = new SSG_SearchRequest
                {
                    SearchRequestId = validRequestId
                }
            };

            _fakeName = new SSG_Aliase
            {
                SearchRequest = new SSG_SearchRequest
                {
                    SearchRequestId = validRequestId
                }
            };

            _ssg_fakePerson = new PersonEntity
            {
            };

            _searchRequest = new SSG_SearchRequest
            {
                SearchRequestId = validRequestId
            };

            _fakeBankInfo = new SSG_Asset_BankingInformation
            {
                BankName = "bank"
            };

            _fakeVehicleEntity = new VehicleEntity
            {
                SearchRequest = _searchRequest,
                PlateNumber = "AAA.BBB"
            };

            _fakeAssetOwner = new SSG_AssetOwner()
            {
                OrganizationName = "Ford Inc."
            };

            _fakeOtherAsset = new AssetOtherEntity()
            {
                SearchRequest = _searchRequest,
                TypeDescription = "type description"
            };

            _fakePerson = new Person()
            {
                DateOfBirth = DateTime.Now,
                FirstName = "TEST1",
                LastName = "TEST2",
                Identifiers = new List<PersonalIdentifier>()
                        {
                            new PersonalIdentifier()
                            {
                               Value  = "test",
                               IssuedBy = "test",
                               Type = PersonalIdentifierType.BCDriverLicense
                            }
                        },
                Addresses = new List<Address>()
                        {
                            new Address()
                            {
                                AddressLine1 = "AddressLine1",
                                AddressLine2 = "AddressLine2",
                                AddressLine3 = "AddressLine3",
                                StateProvince = "Manitoba",
                                City = "testCity",
                                Type = "residence",
                                CountryRegion= "canada",
                                ZipPostalCode = "p3p3p3",
                                ReferenceDates = new List<ReferenceDate>(){
                                    new ReferenceDate(){ Index=0, Key="Start Date", Value=new DateTime(2019,9,1) },
                                    new ReferenceDate(){ Index=1, Key="End Date", Value=new DateTime(2020,9,1) }
                                },
                                Description = "description"
                            }
                        },
                Phones = new List<Phone>()
                    {
                        new Phone ()
                        {
                            PhoneNumber = "4005678900"
                        }
                    },
                Names = new List<Name>()
                    {
                        new Name ()
                        {
                            FirstName = "firstName"
                        }
                    },
                RelatedPersons = new List<RelatedPerson>()
                {
                    new RelatedPerson(){FirstName="firstName"}
                },

                Employments = new List<Employment>()
                {
                    new Employment()
                    {
                        Occupation = "Occupation",
                        Employer = new Employer()
                        {
                            Phones=new List<Phone>(){
                                new Phone(){ PhoneNumber = "1111111", Type="Phone"}
                            }
                        }
                    }
                },

                BankInfos = new List<BankInfo>()
                {
                    new BankInfo()
                    {
                        BankName = "BankName",
                    }
                },
                Vehicles = new List<Vehicle>() {
                    new Vehicle()
                    {

                    },
                    new Vehicle()
                    {
                        Owners=new List<AssetOwner>()
                        {
                            new AssetOwner(){}
                        }
                    }
                },
                OtherAssets = new List<OtherAsset>()
                {
                    new OtherAsset()
                    {
                        Owners=new List<AssetOwner>()
                        {
                            new AssetOwner(){}
                        }
                    }
                }

            };

            _providerProfile = new ProviderProfile()
            {
                Name = "Other"
            };


            _fakeToken = new CancellationToken();

            _mapper.Setup(m => m.Map<SSG_Identifier>(It.IsAny<PersonalIdentifier>()))
                               .Returns(_fakePersoneIdentifier);

            _mapper.Setup(m => m.Map<SSG_PhoneNumber>(It.IsAny<Phone>()))
                             .Returns(_fakePersonPhoneNumber);

            _mapper.Setup(m => m.Map<SSG_Address>(It.IsAny<Address>()))
                              .Returns(_fakePersonAddress);

            _mapper.Setup(m => m.Map<SSG_Aliase>(It.IsAny<Name>()))
                  .Returns(_fakeName);

            _mapper.Setup(m => m.Map<PersonEntity>(It.IsAny<Person>()))
               .Returns(_ssg_fakePerson);

            _mapper.Setup(m => m.Map<EmploymentEntity>(It.IsAny<Employment>()))
       .Returns(_fakeEmployment);

            _mapper.Setup(m => m.Map<SSG_EmploymentContact>(It.IsAny<Phone>()))
                .Returns(_fakeEmploymentContact);

            _mapper.Setup(m => m.Map<SSG_Identity>(It.IsAny<RelatedPerson>()))
                   .Returns(_fakeRelatedPerson);

            _mapper.Setup(m => m.Map<SSG_Asset_BankingInformation>(It.IsAny<BankInfo>()))
                   .Returns(_fakeBankInfo);

            _mapper.Setup(m => m.Map<VehicleEntity>(It.IsAny<Vehicle>()))
                    .Returns(_fakeVehicleEntity);

            _mapper.Setup(m => m.Map<SSG_AssetOwner>(It.IsAny<AssetOwner>()))
                    .Returns(_fakeAssetOwner);

            _mapper.Setup(m => m.Map<AssetOtherEntity>(It.IsAny<OtherAsset>()))
                    .Returns(_fakeOtherAsset);

            _searchRequestServiceMock.Setup(x => x.CreateIdentifier(It.Is<SSG_Identifier>(x => x.SearchRequest.SearchRequestId == invalidRequestId), It.IsAny<CancellationToken>()))
                .Throws(new Exception("random exception"));

            _searchRequestServiceMock.Setup(x => x.CreateAddress(It.Is<SSG_Address>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<SSG_Address>(new SSG_Address()
                {
                    AddressLine1 = "test full line"
                }));

            _searchRequestServiceMock.Setup(x => x.CreatePhoneNumber(It.Is<SSG_PhoneNumber>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
              .Returns(Task.FromResult<SSG_PhoneNumber>(new SSG_PhoneNumber()
              {
                  TelePhoneNumber = "4007678231"
              }));

            _searchRequestServiceMock.Setup(x => x.CreateName(It.Is<SSG_Aliase>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<SSG_Aliase>(new SSG_Aliase()
                {
                    FirstName = "firstName"
                }));

            _searchRequestServiceMock.Setup(x => x.SavePerson(It.Is<PersonEntity>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<SSG_Person>(new SSG_Person()
            {
                FirstName = "First"
            }));

            _searchRequestServiceMock.Setup(x => x.CreateEmployment(It.Is<EmploymentEntity>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<SSG_Employment>(new SSG_Employment()
            {
                EmploymentId = Guid.NewGuid(),
                Occupation = "Occupation"
            }));

            _searchRequestServiceMock.Setup(x => x.CreateEmploymentContact(It.IsAny<SSG_EmploymentContact>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<SSG_EmploymentContact>(new SSG_EmploymentContact()
            {
                PhoneNumber = "4007678231"
            }));

            _searchRequestServiceMock.Setup(x => x.CreateRelatedPerson(It.Is<SSG_Identity>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
              .Returns(Task.FromResult<SSG_Identity>(new SSG_Identity()
              {
                  FirstName = "firstName"
              }));

            _searchRequestServiceMock.Setup(x => x.CreateBankInfo(It.Is<SSG_Asset_BankingInformation>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<SSG_Asset_BankingInformation>(new SSG_Asset_BankingInformation()
                {
                    BankName = "bankName"
                }));

            _searchRequestServiceMock.Setup(x => x.CreateVehicle(It.Is<VehicleEntity>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<SSG_Asset_Vehicle>(new SSG_Asset_Vehicle()
                {
                    VehicleId = validVehicleId
                }));

            _searchRequestServiceMock.Setup(x => x.CreateAssetOwner(It.Is<SSG_AssetOwner>(x => x.Vehicle.VehicleId == validVehicleId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<SSG_AssetOwner>(new SSG_AssetOwner()
                {
                    Type = "Owner"
                }));

            _searchRequestServiceMock.Setup(x => x.CreateOtherAsset(It.Is<AssetOtherEntity>(x => x.SearchRequest.SearchRequestId == validRequestId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<SSG_Asset_Other>(new SSG_Asset_Other()
                {
                    AssetOtherId = validOtherAssetId
                }));

            _sut = new SearchResultService(_searchRequestServiceMock.Object, _loggerMock.Object, _mapper.Object);

        }

        [Test]
        public async Task valid_Person_should_be_processed_correctly()
        {

            var result = await _sut.ProcessPersonFound(_fakePerson, _providerProfile, _searchRequest, _fakeToken);

            _searchRequestServiceMock
              .Verify(x => x.SavePerson(It.IsAny<PersonEntity>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
                .Verify(x => x.CreateIdentifier(It.IsAny<SSG_Identifier>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
                 .Verify(x => x.CreateAddress(It.IsAny<SSG_Address>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
                .Verify(x => x.CreatePhoneNumber(It.IsAny<SSG_PhoneNumber>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
                .Verify(x => x.CreateName(It.IsAny<SSG_Aliase>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
               .Verify(x => x.CreateRelatedPerson(It.IsAny<SSG_Identity>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
              .Verify(x => x.CreateEmployment(It.IsAny<EmploymentEntity>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
                .Verify(x => x.CreateEmploymentContact(It.IsAny<SSG_EmploymentContact>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
               .Verify(x => x.CreateBankInfo(It.IsAny<SSG_Asset_BankingInformation>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
              .Verify(x => x.CreateVehicle(It.IsAny<VehicleEntity>(), It.IsAny<CancellationToken>()), Times.Exactly(2));

            _searchRequestServiceMock
              .Verify(x => x.CreateOtherAsset(It.IsAny<AssetOtherEntity>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

            _searchRequestServiceMock
                .Verify(x => x.CreateAssetOwner(It.IsAny<SSG_AssetOwner>(), It.IsAny<CancellationToken>()), Times.Exactly(2));

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task valid_Person_with_null_properties_should_be_processed_correctly()
        {
            Person fakePersonNull = new Person()
            {
                DateOfBirth = DateTime.Now,
                FirstName = "TEST1",
                LastName = "TEST2",
                Identifiers = null,
                Addresses = null,
                Phones = null,
                Names = null,
                RelatedPersons = null,
                Employments = null,
                BankInfos = null,
                Vehicles = null,
                OtherAssets = null
            };
            var result = await _sut.ProcessPersonFound(fakePersonNull, _providerProfile, _searchRequest, _fakeToken);

            _searchRequestServiceMock
              .Verify(x => x.SavePerson(It.IsAny<PersonEntity>(), It.IsAny<CancellationToken>()), Times.Once);

            _searchRequestServiceMock
                .Verify(x => x.CreateIdentifier(It.IsAny<SSG_Identifier>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
                 .Verify(x => x.CreateAddress(It.IsAny<SSG_Address>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
                .Verify(x => x.CreatePhoneNumber(It.IsAny<SSG_PhoneNumber>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
                .Verify(x => x.CreateName(It.IsAny<SSG_Aliase>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
               .Verify(x => x.CreateRelatedPerson(It.IsAny<SSG_Identity>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
              .Verify(x => x.CreateEmployment(It.IsAny<EmploymentEntity>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
                .Verify(x => x.CreateEmploymentContact(It.IsAny<SSG_EmploymentContact>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
               .Verify(x => x.CreateBankInfo(It.IsAny<SSG_Asset_BankingInformation>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
              .Verify(x => x.CreateVehicle(It.IsAny<VehicleEntity>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
                .Verify(x => x.CreateAssetOwner(It.IsAny<SSG_AssetOwner>(), It.IsAny<CancellationToken>()), Times.Never);

            _searchRequestServiceMock
                .Verify(x => x.CreateOtherAsset(It.IsAny<AssetOtherEntity>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.AreEqual(true, result);
        }
    }
}
