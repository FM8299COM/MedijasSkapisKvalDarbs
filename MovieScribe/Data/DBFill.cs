using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using MovieScribe.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Identity;
using MovieScribe.Data.Static;

namespace MovieScribe.Data
{
    public class DBFill
    {
        public static void Initialize(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DBContext>();
                context.Database.EnsureCreated();


                //Distributorss
                if (!context.Distributors.Any())
                {
                    context.Distributors.AddRange(new List<DistributorModel>()
                    {
                        new DistributorModel() {
                            Name = "Walt Disney",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new DistributorModel() {
                            Name = "Warner Bros.",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new DistributorModel() {
                            Name = "Sony Pictures",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new DistributorModel() {
                            Name = "Universal",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new DistributorModel() {
                            Name = "20th CenturyFox",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new DistributorModel() {
                            Name = "ParamountPictures",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        }
                    });
                    context.SaveChanges();
                }
                //Studios
                if (!context.Studios.Any())
                {
                    context.Studios.AddRange(new List<StudioModel>()
                    {
                        new StudioModel() {
                            Name = "Ģilde",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new StudioModel() {
                            Name = "Jura Podnieka Studija",
                            Description = "Jura Podnieka studija ir godalgota filmu un multimediālo risinājumu producēšanas kompānija ar vairāk kā 20 gadu pieredzi, kas nodrošina pilnu ražošanas ciklu, sākot no izpētes līdz filmešanai, montāžai un skaņas apstrādei.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new StudioModel() {
                            Name = "Labvakar",
                            Description = "tie ir neaizmirstami televīzijas raidījumi un prātā paliekošas dokumentālās filmas",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                    });
                    context.SaveChanges();
                }

                //Producers
                if (!context.Producers.Any())
                {
                    context.Producers.AddRange(new List<ProducerModel>()
                    {
                        new ProducerModel() {
                            Name = "Kevin",
                            Middle_Name = "",
                            Surname = "Feige",
                            Age = 49,
                            Biography = "Kevin Feige (born June 2, 1973) is an American film and television producer who has been the president of Marvel Studios and the primary producer of the Marvel Cinematic Universe franchise since 2007.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new ProducerModel() {
                            Name = "Kathleen",
                            Middle_Name = "",
                            Surname = "Kennedy",
                            Age = 69,
                            Biography = "Kathleen Kennedy (born June 5, 1953) is an American film producer and president of Lucasfilm.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new ProducerModel() {
                            Name = "Jerome",
                            Middle_Name = "Leon",
                            Surname = "Bruckheimer",
                            Age = 79,
                            Biography = "Jerome Leon Bruckheimer (born September 21, 1943) is an American film and television producer. He has been active in the genres of action, drama, fantasy, and science fiction.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new ProducerModel() {
                            Name = "David",
                            Middle_Name = "Jonathan",
                            Surname = "Heyman",
                            Age = 61,
                            Biography = "David Jonathan Heyman (born 26 July 1961) is a British film producer and the founder of Heyday Films.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new ProducerModel() {
                            Name = "Neal",
                            Middle_Name = "H.",
                            Surname = "Moritz",
                            Age = 63,
                            Biography = "Neal H. Moritz (born June 6, 1959) is an American film producer and founder of Original Film.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new ProducerModel() {
                            Name = "Frank",
                            Middle_Name = "Wilton",
                            Surname = "Marshall",
                            Age = 76,
                            Biography = "Frank Wilton Marshall (born September 13, 1946) is an American film producer and director.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new ProducerModel() {
                            Name = "James",
                            Middle_Name = "Francis",
                            Surname = "Cameron",
                            Age = 68,
                            Biography = "James Francis Cameron (born August 16, 1954) is a Canadian filmmaker.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new ProducerModel() {
                            Name = "Charles",
                            Middle_Name = "",
                            Surname = "Roven",
                            Age = 73,
                            Biography = "Charles Roven (born August 2, 1949) is an American film producer and the president and co-founder of Atlas Entertainment.",
                            ImageData = null,
                            ImageMimeType = null,
                        }
                    });
                    context.SaveChanges();
                }

                //Movies
                if (!context.Media.Any())
                {
                    context.Media.AddRange(new List<MediaModel>()
                    {
                        new MediaModel() {
                            Title = "Shazam! Fury of the Gods",
                            Runtime = 5,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            Aired = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)),
                            Type = Enumerators.MediaType.Filma,
                            ProducerID = 1,
                            DistributorID = 6,
                            StudioID = 1,
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new MediaModel() {
                            Title = "Everything Everywhere All at Once",
                            Runtime = 5,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            Aired = DateOnly.FromDateTime(DateTime.Now.AddYears(5)),
                            Type = Enumerators.MediaType.Filma,
                            ProducerID = 2,
                            DistributorID = 5,
                            StudioID = 2,
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new MediaModel() {
                            Title = "Scream VI", 
                            Runtime = 5, 
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            Aired = DateOnly.FromDateTime(DateTime.Now.AddYears(-4)),
                            Type = Enumerators.MediaType.Filma,
                            ProducerID = 3,
                            DistributorID = 4,
                            StudioID = 3,
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new MediaModel() {
                            Title = "The Whale", 
                            Runtime = 5, 
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            Aired = DateOnly.FromDateTime(DateTime.Now.AddYears(-25)),
                            Type = Enumerators.MediaType.Filma,
                            ProducerID = 4,
                            DistributorID = 2,
                            StudioID = 1,
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new MediaModel() {
                            Title = "65",
                            Runtime = 5,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            Aired = DateOnly.FromDateTime(DateTime.Now.AddYears(-10)),
                            Type = Enumerators.MediaType.Filma,
                            ProducerID = 5,
                            DistributorID = 2,
                            StudioID = 1,
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new MediaModel() {
                            Title = "Cocaine Bear",
                            Runtime = 5,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            Aired = DateOnly.FromDateTime(DateTime.Now.AddYears(5)),
                            Type = Enumerators.MediaType.Filma,
                            ProducerID = 6,
                            DistributorID = 1,
                            StudioID = 3,
                            ImageData = null,
                            ImageMimeType = null,
                        }
                    });
                    context.SaveChanges();
                }

                //Actors
                if (!context.Actors.Any())
                {
                    context.Actors.AddRange(new List<ActorModel>()
                    {
                        new ActorModel() {
                            Name = "Kevin",
                            Middle_Name = "LLLLL",
                            Surname = "Feige",
                            ImageData = null,
                            ImageMimeType = null,
                            Age = 49,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                        },
                        new ActorModel() {
                            Name = "Kennedy",
                            Middle_Name = "Kennedy",
                            Surname = "Kennedy",
                            ImageData = null,
                            ImageMimeType = null,
                            Age = 69,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                        },
                        new ActorModel() {
                            Name = "Leon",
                            Middle_Name = "",
                            Surname = "Bruckheimer",
                            ImageData = null,
                            ImageMimeType = null,
                            Age = 79,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                        },
                        new ActorModel() {
                            Name = "David",
                            Middle_Name = "Jonathan",
                            Surname = "Jonathan",
                            ImageData = null,
                            ImageMimeType = null,
                            Age = 61,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                        },
                        new ActorModel() {
                            Name = "Neal",
                            Middle_Name = "H.",
                            Surname = "Moritz",
                            ImageData = null,
                            ImageMimeType = null,
                            Age = 63,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                        },
                        new ActorModel() {
                            Name = "Frank",
                            Middle_Name = "Wilton",
                            Surname = "Marshall",
                            ImageData = null,
                            ImageMimeType = null,
                            Age = 76,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                        },
                        new ActorModel() {
                            Name = "James",
                            Middle_Name = "Francis",
                            Surname = "Cameron",
                            ImageData = null,
                            ImageMimeType = null,
                            Age = 68,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                        },
                        new ActorModel() {
                            Name = "Charles",
                            Middle_Name = "",
                            Surname = "Roven",
                            ImageData = null,
                            ImageMimeType = null,
                            Age = 73,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                        }
                    });
                    context.SaveChanges();
                }

                //Writers
                if (!context.Writers.Any())
                {
                    context.Writers.AddRange(new List<WriterModel>()
                    {
                        new WriterModel() {
                            Name = "Billy",
                            Middle_Name = "",
                            Surname = "Wilder",
                            Age = 44,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new WriterModel() {
                            Name = "Robert",
                            Middle_Name = "Towne",
                            Surname = "Quentino",
                            Age = 67,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new WriterModel() {
                            Name = "Quentin",
                            Middle_Name = "",
                            Surname = "Tarantino",
                            Age = 54,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        },
                        new WriterModel() {
                            Name = "Woody",
                            Middle_Name = "",
                            Surname = "Allen",
                            Age = 34,
                            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar placerat augue, eget egestas ligula. Morbi dictum aliquam feugiat. Proin consequat, velit sed hendrerit sollicitudin, augue metus suscipit sem, et condimentum ipsum sem vitae quam.",
                            ImageData = null,
                            ImageMimeType = null,
                        }
                    });
                    context.SaveChanges();
                }

                //Genres
                if (!context.Genres.Any())
                {
                    context.Genres.AddRange(new List<GenreModel>()
                    {
                        new GenreModel() {
                            Genre = "Horror"
                        },
                        new GenreModel() {
                            Genre = "Sci-fi"
                        },
                        new GenreModel() {
                            Genre = "Action"
                        },
                        new GenreModel() {
                            Genre = "Comedy"
                        },
                        new GenreModel() {
                            Genre = "Drama"
                        },
                        new GenreModel() {
                            Genre = "Thriller"
                        },
                        new GenreModel() {
                            Genre = "Fantasy"
                        },
                        new GenreModel() {
                            Genre = "Adventure"
                        },
                        new GenreModel() {
                            Genre = "War"
                        },
                        new GenreModel() {
                            Genre = "Psychological Thriller"
                        },
                        new GenreModel() {
                            Genre = "Mystery"
                        },
                        new GenreModel() {
                            Genre = "Anime"
                        },
                        new GenreModel() {
                            Genre = "Documentary"
                        },
                        new GenreModel() {
                            Genre = "Crime"
                        }
                    });
                    context.SaveChanges();
                }

                //Languages
                if (!context.Languages.Any())
                {
                    context.Languages.AddRange(new List<LanguageModel>()
                    {
                        new LanguageModel() {
                            Language = "Latviesu"
                        },
                        new LanguageModel() {
                            Language = "Krievu"
                        }
                    });
                    context.SaveChanges();
                }





                // SUB TABLES
                //Actors Movies
                if (!context.Actors_SUB_Media.Any())
                {
                    context.Actors_SUB_Media.AddRange(new List<ActorsSUBMediaModel>()
                    {
                        new ActorsSUBMediaModel() {
                            ActorID = 1,
                            MediaID = 1
                        },
                        new ActorsSUBMediaModel() {
                            ActorID = 1,
                            MediaID = 2
                        },
                        new ActorsSUBMediaModel() {
                            ActorID = 2,
                            MediaID = 1
                        },
                        new ActorsSUBMediaModel() {
                            ActorID = 3,
                            MediaID = 3
                        },
                        new ActorsSUBMediaModel() {
                            ActorID = 5,
                            MediaID = 4
                        },
                        new ActorsSUBMediaModel() {
                            ActorID = 5,
                            MediaID = 5
                        },
                        new ActorsSUBMediaModel() {
                            ActorID = 5,
                            MediaID = 1
                        },
                        new ActorsSUBMediaModel() {
                            ActorID = 7,
                            MediaID = 1
                        }
                    });
                    context.SaveChanges();
                }

                //Genres Movies
                if (!context.Genres_SUB_Media.Any())
                {
                    context.Genres_SUB_Media.AddRange(new List<GenresSUBMediaModel>()
                    {
                        new GenresSUBMediaModel() {
                            GenreID = 1,
                            MediaID = 1
                        },
                        new GenresSUBMediaModel() {
                            GenreID = 2,
                            MediaID = 2
                        },
                        new GenresSUBMediaModel() {
                            GenreID = 3,
                            MediaID = 3
                        },
                        new GenresSUBMediaModel() {
                            GenreID = 4,
                            MediaID = 4
                        },
                        new GenresSUBMediaModel() {
                            GenreID = 5,
                            MediaID = 5
                        },
                        new GenresSUBMediaModel() {
                            GenreID = 6,
                            MediaID = 6
                        },
                        new GenresSUBMediaModel() {
                            GenreID = 7,
                            MediaID = 1
                        },
                        new GenresSUBMediaModel() {
                            GenreID = 8,
                            MediaID = 2
                        },
                        new GenresSUBMediaModel() {
                            GenreID = 9,
                            MediaID = 3
                        }
                    });
                    context.SaveChanges();
                }
                //Languages Movies
                if (!context.Languages_SUB_Media.Any())
                {
                    context.Languages_SUB_Media.AddRange(new List<LanguagesSUBMediaModel>()
                    {
                        new LanguagesSUBMediaModel() {
                            LanguageID = 1,
                            MediaID = 1
                        },
                        new LanguagesSUBMediaModel() {
                            LanguageID = 1,
                            MediaID = 2
                        },
                        new LanguagesSUBMediaModel() {
                            LanguageID = 1,
                            MediaID = 3
                        },
                        new LanguagesSUBMediaModel() {
                            LanguageID = 1,
                            MediaID = 4
                        },
                        new LanguagesSUBMediaModel() {
                            LanguageID = 1,
                            MediaID = 5
                        },
                        new LanguagesSUBMediaModel() {
                            LanguageID = 1,
                            MediaID = 6
                        },
                    });
                    context.SaveChanges();
                }
                //Writers Movies
                if (!context.Writers_SUB_Media.Any())
                {
                    context.Writers_SUB_Media.AddRange(new List<WritersSUBMediaModel>()
                    {
                        new WritersSUBMediaModel() {
                            WriterID = 1,
                            MediaID = 1
                        },
                        new WritersSUBMediaModel() {
                            WriterID = 2,
                            MediaID = 2
                        },
                        new WritersSUBMediaModel() {
                            WriterID = 3,
                            MediaID = 3
                        },
                        new WritersSUBMediaModel() {
                            WriterID = 4,
                            MediaID = 4
                        },
                        new WritersSUBMediaModel() {
                            WriterID = 1,
                            MediaID = 5
                        },
                        new WritersSUBMediaModel() {
                            WriterID = 4,
                            MediaID = 6
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    
        public static async Task SeedUsersAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                
                if(!await roleManager.RoleExistsAsync(UserRoles.Administrator))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Administrator));
                }
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }
                if (!await roleManager.RoleExistsAsync(UserRoles.Moderator))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));
                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var adminEmail = "admin@medijasskapis.net";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "Admin",
                        Email = "admin@medijasskapis.net",
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAdminUser, "AdminPass22$");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Administrator);
                }
                var modEmail = "mod@medijasskapis.net";

                var appModEmail = await userManager.FindByEmailAsync(modEmail);
                if (appModEmail == null)
                {
                    var newModUser = new AppUser()
                    {
                        UserName = "NewMod",
                        Email = "moderator@medijasskapis.net",
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newModUser, "ModeratorPass22$");
                    await userManager.AddToRoleAsync(newModUser, UserRoles.Moderator);
                }

                var userEmail = "user@medijasskapis.net";

                var appUserEmail = await userManager.FindByEmailAsync(userEmail);
                if (appUserEmail == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "NewUser",
                        Email = "user@medijasskapis.net",
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, "UserPass22$");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
