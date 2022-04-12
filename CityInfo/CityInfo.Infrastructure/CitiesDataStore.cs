using CityInfo.Domain;
using System.Collections.Generic;

namespace CityInfo.Infrastructure
{
    public class CitiesDataStore : ICitiesDataStore
    {
        public List<City> Cities { get; set; }
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            //init dummy data
            Cities = new List<City>()
            {
                new City()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "The most visited urban park in the United States"
                        },
                        new PointOfInterest()
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "A 102-story skyscraper located in Midtown Manhattan"
                        }
                    }
                },
                new City()
                {
                    Id = 2,
                    Name = "Cape Town",
                    Description = "The one with the table mountain",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Id = 3,
                            Name = "Table Mountain",
                            Description = "The flat Mountain"
                        },
                        new PointOfInterest()
                        {
                            Id = 2,
                            Name = "The Big Hole",
                            Description = "The mine that produces diamonds"
                        }
                    }
                },
                new City()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Id = 5,
                            Name = "Eiffel Tower",
                            Description = "A wrought iron lattice tower on the Champ de Mars,"
                        },
                        new PointOfInterest()
                        {
                            Id = 6,
                            Name = "The Louvre",
                            Description = "The world's largest museum."
                        }
                    }
                },
            };
        }
    }
}
