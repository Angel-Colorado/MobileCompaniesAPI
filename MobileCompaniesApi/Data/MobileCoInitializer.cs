using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using MobileCompaniesApi.Models;
using System;

namespace MobileCompaniesApi.Data
{
    public static class MobileCoInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            MobileCoContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<MobileCoContext>();
            
            try
            {
                //Delete the database if you need to apply a new Migration
                //context.Database.EnsureDeleted();
                //Create the database if it does not exist and apply the Migration
                context.Database.Migrate();

                // To randomly generate data
                Random random = new Random();

                // Look for any Company. Since we can't have Antennas without Companies
                if (!context.Companies.Any())
                {
                    context.Companies.AddRange(
                        new Company { Name = "Telus" },
                        new Company { Name = "Bell" },
                        new Company { Name = "Rogers" },
                        new Company { Name = "Virgin" }
                        );
                    context.SaveChanges();
                }

                if (!context.Antennas.Any())
                {
                    context.Antennas.AddRange(
                        new Antenna
                        {
                            Name = "Bus Terminal",
                            Latitude = 43.160053940468m,
                            Longitude = -79.24367561493041m,
                            Description = "St. Catharines' Bus Terminal",
                            Image = "https://www.stcatharines.ca/en/rotatingimages/defaultInterior/Downtown-Banner.jpg",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Bell").ID
                        },
                        new Antenna
                        {
                            Name = "Body Shop Fitness",
                            Latitude = 43.147372915112705m,
                            Longitude = -79.2521341490332m,
                            Description = "173 St. Paul Crescent, St. Catharines",
                            Image = "https://img1.wsimg.com/isteam/ip/be227faf-0dc1-4377-b0e4-ddcfc6df8587/unnamed1.jpg/:/rs=w:1300,h:800",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Bell").ID
                        },
                        new Antenna
                        {
                            Name = "Adult Learning Centre",
                            Latitude = 43.17054895496507m,
                            Longitude = -79.2322195002799m,
                            Description = "145 Niagara St, St. Catharines",
                            Image = "https://mallmaverick.imgix.net/web/property_managers/16/properties/277/stores/bramaleacitycentre-bramalea-city-centre-bell/logo_aHR0cHM6Ly9tYWxsbWF2ZXJpY2suaW1naXgubmV0L3Byb3BlcnR5X21hbmFnZXJzLzE2L3Byb3BlcnRpZXMvMjc3L3N0b3Jlcy80MjA4NA==",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Bell").ID
                        },
                        new Antenna
                        {
                            Name = "Walmart",
                            Latitude = 43.15183774045605m,
                            Longitude = -79.26592082844036m,
                            Description = "420 Vansickle Rd, St. Catharines",
                            Image = "https://imageio.forbes.com/specials-images/imageserve/1209106512/Daily-Life-During-Coronavirus-Epidemic-In-Toronto/960x0.jpg",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Rogers").ID
                        },
                        new Antenna
                        {
                            Name = "NC DJP",
                            Latitude = 43.15237194979515m,
                            Longitude = -79.16312095145626m,
                            Description = "Niagara College DJP Campus",
                            Image = "https://www.niagaracollege.ca/wp-content/uploads/notl-campus-2020-large.jpg",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Rogers").ID
                        },
                        new Antenna
                        {
                            Name = "NC Welland Book store",
                            Latitude = 43.01516420186208m,
                            Longitude = -79.2633439712294m,
                            Description = "Niagara College Welland Campus",
                            Image = "https://www.niagaracollege.ca/wp-content/uploads/welland-campus-2020-large.jpg",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Telus").ID
                        },
                        new Antenna
                        {
                            Name = "NC Welland Tim Hortons",
                            Latitude = 43.015471707289564m,
                            Longitude = -79.26317670086553m,
                            Description = "Niagara College Welland Campus",
                            Image = "https://d2qkj1k602maha.cloudfront.net/assets/TH-CA/img.png",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Telus").ID
                        },
                        new Antenna
                        {
                            Name = "NC Welland M201",
                            Latitude = 43.01469430354032m,
                            Longitude = -79.2625388852205m,
                            Description = "Niagara College Welland Campus",
                            Image = "https://www.niagaracollege.ca/wp-content/uploads/welland-campus-2020-large.jpg",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Telus").ID
                        },
                        new Antenna
                        {
                            Name = "NC Welland Residence",
                            Latitude = 43.01706886056943m,
                            Longitude = -79.26147193713857m,
                            Description = "Niagara College Welland Campus",
                            Image = "https://www.niagaracollege.ca/wp-content/uploads/welland-campus-2020-large.jpg",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Telus").ID
                        }
                        );
                    context.SaveChanges();
                }

                if (!context.Devices.Any())
                {
                    context.Devices.AddRange(
                        new Device
                        {
                            Name = "An expensive phone",
                            Model = "NK-MF87STFU",
                            Manufacturer = "Nokia",
                            Type = "Phone",
                            Username = "erik@hotmail.com",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Bell").ID
                        },
                        new Device
                        {
                            Name = "Anna F",
                            Model = "AF-TG34",
                            Manufacturer = "Blackberry",
                            Type = "Phone",
                            Username = "anna@outlook.com",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Bell").ID
                        },
                        new Device
                        {
                            Name = "Terrence M",
                            Model = "TB-ST29AUXT",
                            Manufacturer = "Samsung",
                            Type = "Tablet",
                            Username = "terrence@yahoo.com",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Rogers").ID
                        },
                        new Device
                        {
                            Name = "Angel Colorado",
                            Model = "SM-N975F",
                            Manufacturer = "samsung",
                            Type = "Phone",
                            Username = "angelc@hotmail.com",
                            CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Telus").ID
                        }
                        );
                    context.SaveChanges();
                }

                if (!context.Positions.Any())
                {
                    context.Positions.AddRange(
                        new Position
                        {
                            Latitude = 43.01516420186208m,
                            Longitude = -79.2633439712294m,
                            DeviceID = context.Devices.FirstOrDefault(c => c.Name == "Anna F").ID
                        },
                        new Position
                        {
                            Latitude = 43.01706886056943m,
                            Longitude = -79.26147193713857m,
                            DeviceID = context.Devices.FirstOrDefault(c => c.Name == "Terrence M").ID
                        }
                        );
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
