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
                
                if (!context.Genres.Any())
                {
                    context.Genres.AddRange(new List<GenreModel>()
                    {
                        new GenreModel() {
                            Genre = "Absurda kino"
                        },
                        new GenreModel() {
                            Genre = "Animācijas"
                        },
                        new GenreModel() {
                            Genre = "Apokaliptiska"
                        },
                        new GenreModel() {
                            Genre = "Avangarda"
                        },
                        new GenreModel() {
                            Genre = "Biogrāfija"
                        },
                        new GenreModel() {
                            Genre = "Drāma"
                        },
                        new GenreModel() {
                            Genre = "Ekspresionisms"
                        },
                        new GenreModel() {
                            Genre = "Episka"
                        },
                        new GenreModel() {
                            Genre = "Erotika"
                        },
                        new GenreModel() {
                            Genre = "Fantāzija"
                        },
                        new GenreModel() {
                            Genre = "Film noir‎"
                        },
                        new GenreModel() {
                            Genre = "Karš"
                        },
                        new GenreModel() {
                            Genre = "Katastrofa"
                        },
                        new GenreModel() {
                            Genre = "Kinokomēdija"
                        },
                        new GenreModel() {
                            Genre = "Mistērija"
                        },
                        new GenreModel() {
                            Genre = "Muzikāla"
                        },
                        new GenreModel() {
                            Genre = "Neo-noir"
                        },
                        new GenreModel() {
                            Genre = "Piedzīvojumi"
                        },
                        new GenreModel() {
                            Genre = "Politika"
                        },
                        new GenreModel() {
                            Genre = "Romantika"
                        },
                        new GenreModel() {
                            Genre = "Satīra"
                        },
                        new GenreModel() {
                            Genre = "Sirreālisms"
                        },
                        new GenreModel() {
                            Genre = "Spiegi"
                        },
                        new GenreModel() {
                            Genre = "Spraiga sižeta"
                        },
                        new GenreModel() {
                            Genre = "Šausmu"
                        },
                        new GenreModel() {
                            Genre = "Trilleri"
                        },
                        new GenreModel() {
                            Genre = "Vesterni"
                        },
                        new GenreModel() {
                            Genre = "Vēsturiska"
                        },
                        new GenreModel() {
                            Genre = "Ziemassvētku"
                        },
                        new GenreModel() {
                            Genre = "Zinātniskā fantastika"
                        }
                    });
                    context.SaveChanges();
                }

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
                    var adminUsername = "Admin";
                    var existingUserWithSameName = await userManager.FindByNameAsync(adminUsername);
                    if (existingUserWithSameName == null)
                    {
                        var newAdminUser = new AppUser()
                        {
                            UserName = adminUsername,
                            Email = "admin@medijasskapis.net",
                            EmailConfirmed = true,
                        };

                        var createUserResult = await userManager.CreateAsync(newAdminUser, "AdminPass22$");
                        if (createUserResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newAdminUser, UserRoles.Administrator);
                        }
                        else
                        {
                            throw new Exception($"Admin user creation failed: {string.Join(", ", createUserResult.Errors.Select(x => x.Description))}");
                        }
                    }
                    else{}
                }


                var modEmail = "mod@medijasskapis.net";

                var appModEmail = await userManager.FindByEmailAsync(modEmail);
                if (appModEmail == null)
                {
                    var newModUsername = "NewMod";
                    var existingUserWithSameName = await userManager.FindByNameAsync(newModUsername);
                    if (existingUserWithSameName == null)
                    {
                        var newModUser = new AppUser()
                        {
                            UserName = newModUsername,
                            Email = "moderator@medijasskapis.net",
                            EmailConfirmed = true,
                        };

                        var createModUserResult = await userManager.CreateAsync(newModUser, "ModeratorPass22$");
                        if (createModUserResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newModUser, UserRoles.Moderator);
                        }
                        else
                        {
                            throw new Exception($"Moderator user creation failed: {string.Join(", ", createModUserResult.Errors.Select(x => x.Description))}");
                        }
                    }
                    else{}
                }



                var userEmail = "user@medijasskapis.net";

                var appUserEmail = await userManager.FindByEmailAsync(userEmail);
                if (appUserEmail == null)
                {
                    var newUserUsername = "NewUser";
                    var existingUserWithSameName = await userManager.FindByNameAsync(newUserUsername);
                    if (existingUserWithSameName == null)
                    {
                        var newAppUser = new AppUser()
                        {
                            UserName = newUserUsername,
                            Email = "user@medijasskapis.net",
                            EmailConfirmed = true,
                        };

                        var createAppUserResult = await userManager.CreateAsync(newAppUser, "UserOfAppPass22$");
                        if (createAppUserResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                        }
                        else
                        {
                            throw new Exception($"Regular user creation failed: {string.Join(", ", createAppUserResult.Errors.Select(x => x.Description))}");
                        }
                    }
                    else{}
                }


            }
        }
    }
}
