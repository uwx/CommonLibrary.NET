using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;


using ComLib;
using ComLib.LocationSupport;
using ComLib.Entities;

using CommonLibrary.Tests;



namespace CommonLibrary.Tests.LocationTests
{
    
    /// <summary>
    /// This tests the location parsing using In-Memory repositories.
    /// </summary>
    [TestFixture]
    public class LocationServiceTests
    {
        // private DataAccessObjectBuilder _daoBuilder;
        private int StateId_NY = 0;
        private int StateId_NJ = 0;
        private int StateId_CT = 0;
        private int StateId_ItalyState1 = 0;
        private int CountryId_USA = 0;
        private int CountryId_Italy = 0;


        public LocationServiceTests()
        {
            IRepository<City> cityRepo = new RepositoryInMemory<City>("Id,Name,StateId,CountryId");
            IRepository<State> stateRepo = new RepositoryInMemory<State>("Id,Name,StateId,CountryId");
            IRepository<Country> countryRepo = new RepositoryInMemory<Country>("Id,Name,StateId,CountryId");
            ILocationService locationService = new LocationService(() => countryRepo, () => stateRepo, () => cityRepo);
            Location.Init(locationService);


            CountryId_Italy = Location.CreateCountry("Italy").Item.Id;
            CountryId_USA = Location.CreateCountry("United States").Item.Id;
            Location.CreateCountry("Spain");
            Location.CreateCountry("India");
            Location.CreateCountry("United States", "USA", true);
            Location.CreateCountry("United States", "America", true);
            Location.CreateCountry("United States", "U.S.A", true);
            IList<Country> countries = Location.Countries.GetAll();

            StateId_NY = Location.CreateState("New York", "NY", "USA").Item.Id;
            StateId_NJ = Location.CreateState("New Jersey", "NJ", "USA").Item.Id;
            StateId_CT = Location.CreateState("Connecticut", "CT", "USA").Item.Id;
            StateId_ItalyState1 = Location.CreateState("ItalianState1", "IT1", "Italy").Item.Id;
            Location.CreateState("Florida", "FL", "USA");
            Location.CreateState("California", "CA", "USA");
            IList<State> states = Location.States.GetAll();

            Location.CreateCity("New York", "New York", "USA");
            Location.CreateCity("Brooklyn", "New York", "USA");
            Location.CreateCity("Bronx", "Connecticut", "USA");
            Location.CreateCity("Bronx", "New York", "USA");
            Location.CreateCity("San Francisco", "California", "USA");
            Location.CreateCity("Trenton", "New Jersey", "USA");
            Location.CreateCity("Miami", "Florida", "USA");
            Location.CreateCity("Venice", "ItalianState1", "Italy");
            IList<City> cities = Location.Cities.GetAll();
        }

        
        private ILocationService GetLocationService()
        {
            return Location.Service;
        }
        

        [Ignore]
        public void CanParseZip()
        {
            ILocationService locationService = GetLocationService();

            LocationLookUpResult lookupResult = locationService.Parse("11375");
            Assert.IsTrue(lookupResult.IsLookUpByZip);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.IsTrue(lookupResult.Zip == "11375");

            lookupResult = locationService.Parse("07024");
            Assert.IsTrue(lookupResult.IsLookUpByZip);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.IsTrue(lookupResult.Zip == "07024");
        }


        [Test]
        public void CanParseCityAllLowerCase()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse("bronx");

            Assert.IsTrue(lookupResult.LookUpType == LocationLookUpType.City);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.State, "New York");
            Assert.AreEqual(lookupResult.City, "Bronx");
            Assert.AreEqual(lookupResult.StateAbbr, "NY");
            Assert.AreEqual(lookupResult.StateId, StateId_NY);
            Assert.AreEqual(lookupResult.CountryId, CountryId_USA);
        }


        [Test]
        public void CanParseCityAllCAPS()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse("BRONX");

            Assert.IsTrue(lookupResult.LookUpType == LocationLookUpType.City);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.State, "New York");
            Assert.AreEqual(lookupResult.City, "Bronx");
            Assert.AreEqual(lookupResult.StateAbbr, "NY");
            Assert.AreEqual(lookupResult.StateId, StateId_NY);
            Assert.AreEqual(lookupResult.CountryId, CountryId_USA);
        }


        [Test]
        public void CanParseKnownCityInEuropeWithMixedCase()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse("veNice");

            Assert.IsTrue(lookupResult.LookUpType == LocationLookUpType.City);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.State, "ItalianState1");
            Assert.AreEqual(lookupResult.City, "Venice");
            Assert.AreEqual(lookupResult.StateAbbr, "IT1");
            Assert.AreEqual(lookupResult.StateId, StateId_ItalyState1);
            Assert.AreEqual(lookupResult.CountryId, CountryId_Italy);
        }


        [Test]
        public void CanParseByCityStateFullName()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse("nEw YOrk,new york");

            Assert.IsTrue(lookupResult.IsLookUpByCity);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.State, "New York");
            Assert.AreEqual(lookupResult.City, "New York");
            Assert.AreEqual(lookupResult.StateAbbr, "NY");
            Assert.AreEqual(lookupResult.StateId, StateId_NY);
            Assert.AreEqual(lookupResult.CountryId, CountryId_USA);
        }


        [Test]
        public void CanParseByCityCountryInEuropeFullName()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse(" Venice , Italy ");

            Assert.IsTrue(lookupResult.IsLookUpByCity);
            Assert.IsTrue(lookupResult.IsValid);            
            Assert.AreEqual(lookupResult.City, "Venice");
            Assert.AreEqual(lookupResult.CountryId, CountryId_Italy);
        }


        [Test]
        public void CanParseByCityStateInEuropeWithInvalidCountry()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse(" Venice , United States ");

            Assert.IsTrue(lookupResult.IsLookUpByCityCountry);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.City, "Venice");
            Assert.AreEqual(lookupResult.CountryId, CountryId_USA);
        }


        [Test]
        public void CanParseByKnownCityInWithDifferentState()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse(" bronx , Connecticut ");

            Assert.IsTrue(lookupResult.IsLookUpByCityState);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.City, "bronx");
            Assert.AreEqual(lookupResult.StateId, StateId_CT);
            Assert.AreEqual(lookupResult.CountryId, CountryId_USA);
        }


        [Test]
        public void CanParseByKnownCityWithStateResultingInSimpleCitySearch()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse(" bronx , New York ");

            Assert.IsTrue(lookupResult.IsLookUpByCity);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.City, "Bronx");
            Assert.AreEqual(lookupResult.StateId, StateId_NY);
            Assert.AreEqual(lookupResult.CountryId, CountryId_USA);
        }


        [Test]
        public void CanParseByStateAbbr()
        {
            ILocationService locationService = GetLocationService();

            LocationLookUpResult lookupResult = locationService.Parse("ny");
            Assert.IsTrue(lookupResult.IsLookUpByState);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.State.ToLower(), "new york");
            Assert.AreEqual(lookupResult.StateId, StateId_NY);
        }


        [Test]
        public void CanParseByStateFullName()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse("nEw Jersey");

            Assert.IsTrue(lookupResult.IsLookUpByState);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.State.ToLower(), "new jersey");
            Assert.AreEqual(lookupResult.StateId, StateId_NJ);
        }


        [Test]
        public void CanParseByCityStateAbbr()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse("nEw YOrk,ny");

            Assert.IsTrue(lookupResult.IsLookUpByCity);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.State, "New York");
            Assert.AreEqual(lookupResult.City, "New York");
            Assert.AreEqual(lookupResult.StateAbbr, "NY");
            Assert.AreEqual(lookupResult.StateId, StateId_NY);
            Assert.AreEqual(lookupResult.CountryId, CountryId_USA);
        }


        [Test]
        public void CanParseByCountry()
        {
            ILocationService locationService = GetLocationService();
            LocationLookUpResult lookupResult = locationService.Parse("Italy");

            Assert.IsTrue(lookupResult.IsLookUpByCountry);
            Assert.IsTrue(lookupResult.IsValid);
            Assert.AreEqual(lookupResult.Country, "Italy");
            Assert.AreEqual(lookupResult.CountryId, CountryId_Italy);
        }
    }


    [TestFixture]
    public class AddressTests
    {
    }
     
}
