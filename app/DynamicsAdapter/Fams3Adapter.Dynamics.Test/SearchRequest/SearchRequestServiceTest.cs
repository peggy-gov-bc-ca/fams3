﻿using Fams3Adapter.Dynamics.Address;
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
using Fams3Adapter.Dynamics.Types;
using Fams3Adapter.Dynamics.Vehicle;
using Moq;
using NUnit.Framework;
using Simple.OData.Client;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Fams3Adapter.Dynamics.Test.SearchRequest
{
    public class SearchRequestServiceTest
    {
        private Mock<IODataClient> odataClientMock;

        private readonly Guid testId = Guid.Parse("6AE89FE6-9909-EA11-B813-00505683FBF4");
        private readonly Guid testPersonId = Guid.Parse("6AE89FE6-9909-EA11-1111-00505683FBF4");
        private readonly Guid testVehicleId = Guid.Parse("8AE89FE6-9909-EA11-1901-00005683FBF9");
        private readonly Guid testAssetOtherId = Guid.Parse("77789FE6-9909-EA11-1901-000056837777");

        private SearchRequestService _sut;

        [SetUp]
        public void SetUp()
        {
            odataClientMock = new Mock<IODataClient>();

            odataClientMock.Setup(x => x.For<SSG_Identifier>(null).Set(It.Is<SSG_Identifier>(x => x.Identification == "identificationtest"))
            .InsertEntryAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new SSG_Identifier()
            {
                Identification = "test"
            })
            );

            odataClientMock.Setup(x => x.For<SSG_Country>(null)
                            .Filter(It.IsAny<Expression<Func<SSG_Country, bool>>>())
                            .FindEntryAsync(It.IsAny<CancellationToken>()))
                            .Returns(Task.FromResult<SSG_Country>(new SSG_Country()
                            {
                                CountryId = Guid.NewGuid(),
                                Name = "Canada"
                            }));

            odataClientMock.Setup(x => x.For<SSG_CountrySubdivision>(null)
                .Filter(It.IsAny<Expression<Func<SSG_CountrySubdivision, bool>>>())
                .FindEntryAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<SSG_CountrySubdivision>(new SSG_CountrySubdivision()
                {
                    CountrySubdivisionId = Guid.NewGuid(),
                    Name = "British Columbia"
                }));

            odataClientMock.Setup(x => x.For<SSG_Address>(null).Set(It.Is<SSG_Address>(x => x.AddressLine1 == "address full text"))
            .InsertEntryAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new SSG_Address()
            {
                AddressLine1 = "test"
            })
            );

            odataClientMock.Setup(x => x.For<SSG_PhoneNumber>(null).Set(It.Is<SSG_PhoneNumber>(x => x.TelePhoneNumber == "4007678231"))
           .InsertEntryAsync(It.IsAny<CancellationToken>()))
           .Returns(Task.FromResult(new SSG_PhoneNumber()
           {
               TelePhoneNumber = "4007678231"
           })
           );

            odataClientMock.Setup(x => x.For<SSG_Aliase>(null).Set(It.Is<SSG_Aliase>(x => x.FirstName == "firstName"))
            .InsertEntryAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new SSG_Aliase()
            {
               FirstName = "firstName"
            })
            );

            odataClientMock.Setup(x => x.For<SSG_Person>(null).Set(It.Is<PersonEntity>(x => x.FirstName == "First"))
          .InsertEntryAsync(It.IsAny<CancellationToken>()))
          .Returns(Task.FromResult(new SSG_Person()
          {
              FirstName = "FirstName",
              PersonId = testPersonId
          })
          );

        odataClientMock.Setup(x => x.For<SSG_Identity>(null).Set(It.Is<SSG_Identity>(x => x.FirstName == "First"))
        .InsertEntryAsync(It.IsAny<CancellationToken>()))
        .Returns(Task.FromResult(new SSG_Identity()
        {
            FirstName = "FirstName"
        })
        );

         odataClientMock.Setup(x => x.For<SSG_Employment>(null).Set(It.Is<EmploymentEntity>(x => x.BusinessOwner == "Business Owner"))
         .InsertEntryAsync(It.IsAny<CancellationToken>()))
         .Returns(Task.FromResult(new SSG_Employment()
         {
             BusinessOwner = "Business Owner"
         })
         );

            odataClientMock.Setup(x => x.For<SSG_EmploymentContact>(null).Set(It.Is<SSG_EmploymentContact>(x => x.PhoneNumber == "12345678"))
            .InsertEntryAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new SSG_EmploymentContact()
            {
             PhoneNumber = "12345678"
            })
            );

            odataClientMock.Setup(x => x.For<SSG_Asset_BankingInformation>(null).Set(It.Is<SSG_Asset_BankingInformation>(x => x.BankName == "bank"))
            .InsertEntryAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new SSG_Asset_BankingInformation()
            {
              BankName = "bank",
            })
            );

            odataClientMock.Setup(x => x.For<SSG_Asset_Vehicle>(null).Set(It.Is<VehicleEntity>(x => x.PlateNumber == "AAA.BBB"))
             .InsertEntryAsync(It.IsAny<CancellationToken>()))
             .Returns(Task.FromResult(new SSG_Asset_Vehicle()
             {
                 VehicleId = testVehicleId
             })
             );

            odataClientMock.Setup(x => x.For<SSG_AssetOwner>(null).Set(It.Is<SSG_AssetOwner>(x => x.FirstName == "firstName"))
             .InsertEntryAsync(It.IsAny<CancellationToken>()))
             .Returns(Task.FromResult(new SSG_AssetOwner()
             {
                 FirstName = "firstName"
             })
             );

            odataClientMock.Setup(x => x.For<SSG_Asset_Other>(null).Set(It.Is<AssetOtherEntity>(x => x.AssetDescription == "asset description"))
             .InsertEntryAsync(It.IsAny<CancellationToken>()))
             .Returns(Task.FromResult(new SSG_Asset_Other()
             {
                 AssetOtherId = testAssetOtherId
             })
             );

            _sut = new SearchRequestService(odataClientMock.Object);
        }


        [Test]
        public async Task with_correct_searchRequestid_upload_identifier_should_success()
        {
            var identifier = new SSG_Identifier()
            {
                Identification = "identificationtest",
                //IdentificationEffectiveDate = DateTime.Now,
                StateCode = 0,
                StatusCode = 1,
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreateIdentifier(identifier, CancellationToken.None);

            Assert.AreEqual("test", result.Identification);
        }


        [Test]
        public async Task with_correct_searchRequestid_upload_person_should_success()
        {
            var person = new PersonEntity()
            {
                FirstName = "First",
                LastName = "lastName",
                MiddleName = "middleName",
                ThirdGivenName = "Third",
                DateOfBirth = null,
                DateOfDeath = null,
                DateOfDeathConfirmed = false,
                Incacerated = 86000071,
                StateCode = 0,
                StatusCode = 1,
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId }
            };

            var result = await _sut.SavePerson(person, CancellationToken.None);

            Assert.AreEqual("FirstName", result.FirstName);
            Assert.AreEqual(testPersonId, result.PersonId);
        }
        [Test]
        public async Task with_correct_searchRequestid_upload_phone_number_should_success()
        {
            var phone = new SSG_PhoneNumber()
            {

                Date1 = DateTime.Now,
                Date1Label = "Effective Date",
                Date2 = new DateTime(2001, 1, 1),
                Date2Label = "Expiry Date",
                TelePhoneNumber = "4007678231",
                StateCode = 0,
                StatusCode = 1,
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreatePhoneNumber(phone, CancellationToken.None);

            Assert.AreEqual("4007678231", result.TelePhoneNumber);
        }

        [Test]
        public async Task with_correct_searchRequestid_upload_address_should_success()
        {
            var address = new SSG_Address()
            {
                AddressLine1 = "address full text",
                CountryText = "canada",
                CountrySubdivisionText = "British Columbia",
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreateAddress(address, CancellationToken.None);

            Assert.AreEqual("test", result.AddressLine1);
        }

        [Test]
        public async Task with_correct_searchRequestid_upload_name_should_success()
        {
            var name = new SSG_Aliase()
            {
                FirstName = "firstName",
                LastName = "lastName",
                MiddleName = "middleName",
                Comments = "testComments",
                Type = PersonNameCategory.LegalName.Value,
                Notes = "notes",
                ThirdGivenName ="thirdName",
                SupplierTypeCode = "legal",
                Date1 = new DateTime(2001,1,1),
                Date1Label = "date1lable",
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreateName(name, CancellationToken.None);

            Assert.AreEqual("firstName", result.FirstName);
        }

        [Test]
        public async Task with_correct_searchRequestid_upload_related_person_should_success()
        {
            var relatedPerson = new SSG_Identity()
            {
                FirstName = "First",
                LastName = "lastName",
                MiddleName = "middleName",
                ThirdGivenName = "otherName",
                Type = PersonRelationType.Friend.Value,
                Notes = "notes",
                SupplierRelationType = "friend",
                Date1 = new DateTime(2001, 1, 1),
                Date1Label = "date1lable",
                Date2 = new DateTime(2005, 1, 1),
                Date2Label = "date2lable",
                Gender = GenderType.Female.Value,
                Description = "description",
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreateRelatedPerson(relatedPerson, CancellationToken.None);

            Assert.AreEqual("FirstName", result.FirstName);
        }


        [Test]
        public async Task with_correct_searchRequestid_upload_employment_should_succed()
        {
            var employment = new EmploymentEntity()
            {
                BusinessOwner= "Business Owner",
                BusinessName = "Business Name",
                Notes = "notes",
                Date1 = new DateTime(2001, 1, 1),
                Date1Label = "date1lable",
                Date2 = new DateTime(2005, 1, 1),
                Date2Label = "date2lable",
        
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreateEmployment(employment, CancellationToken.None);

            Assert.AreEqual("Business Owner", result.BusinessOwner);
        }

        [Test]
        public async Task with_correct_employmentid_upload_employmentcontact_should_succed()
        {
            var employmentContact = new SSG_EmploymentContact()
            {
                Employment = new SSG_Employment() { EmploymentId = testId },
                PhoneNumber = "12345678"
            };

            var result = await _sut.CreateEmploymentContact(employmentContact, CancellationToken.None);

            Assert.AreEqual("12345678", result.PhoneNumber);
        }

        [Test]
        public async Task with_correct_searchRequestid_upload_bank_info_should_success()
        {
            var bankInfo = new SSG_Asset_BankingInformation()
            {
                BankName = "bank",
                TransitNumber = "123456",
                AccountNumber = "123456",
                Branch = "branch",
                Notes = "notes",
                Date1 = new DateTime(2001, 1, 1),
                Date1Label = "date1lable",
                Date2 = new DateTime(2005, 1, 1),
                Date2Label = "date2lable",
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreateBankInfo(bankInfo, CancellationToken.None);

            Assert.AreEqual("bank", result.BankName);
        }

        [Test]
        public async Task with_correct_searchRequestid_upload_vehicle_should_success()
        {
            var vehicle = new VehicleEntity()
            {
                Discription="car color, type",
                PlateNumber="AAA.BBB",
                Notes = "notes",
                Date1 = new DateTime(2001, 1, 1),
                Date1Label = "date1lable",
                Date2 = new DateTime(2005, 1, 1),
                Date2Label = "date2lable",
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreateVehicle(vehicle, CancellationToken.None);

            Assert.AreEqual(testVehicleId, result.VehicleId);
        }

        [Test]
        public async Task with_correct_vehicleID_upload_Assetowner_should_success()
        {
            var owner = new SSG_AssetOwner()
            {
                FirstName = "firstName",
                Notes = "notes",
            };

            var result = await _sut.CreateAssetOwner(owner, CancellationToken.None);

            Assert.AreEqual("firstName", result.FirstName);
        }

        [Test]
        public async Task with_correct_searchRequestid_upload_otherasset_should_success()
        {
            var assetOther = new AssetOtherEntity()
            {
                AssetDescription = "asset description",
                Description = "description",
                TypeDescription="asset type description",
                Notes = "notes",
                Date1 = new DateTime(2001, 1, 1),
                Date1Label = "date1lable",
                Date2 = new DateTime(2005, 1, 1),
                Date2Label = "date2lable",
                SearchRequest = new SSG_SearchRequest() { SearchRequestId = testId },
                Person = new SSG_Person() { PersonId = testPersonId }
            };

            var result = await _sut.CreateOtherAsset(assetOther, CancellationToken.None);

            Assert.AreEqual(testAssetOtherId, result.AssetOtherId);
        }
    }
}
