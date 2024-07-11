using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    public class DataSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "admin", "shopper" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Add an admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com"
            };

            var adminPassword = "AdminPassword123!";
            var adminUserInDb = await userManager.FindByEmailAsync(adminUser.Email);
            if (adminUserInDb == null)
            {
                var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "admin");
                }
            }

            var shopperUsers = new[]
            {
                new ApplicationUser
                {
                    UserName = "shopper1@example.com",
                    Email = "shopper1@example.com"
                },
                new ApplicationUser
                {
                    UserName = "shopper2@example.com",
                    Email = "shopper2@example.com"
                }
            };

            var shopperPassword = "ShopperPassword123!";
            foreach (var shopperUser in shopperUsers)
            {
                var shopperUserInDb = await userManager.FindByEmailAsync(shopperUser.Email);
                if (shopperUserInDb == null)
                {
                    var createShopperUser = await userManager.CreateAsync(shopperUser, shopperPassword);
                    if (createShopperUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(shopperUser, "shopper");
                    }
                }
            }
        }

        public static async Task SeedOrdersAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!context.Orders.Any())
                {
                    var orders = new[]
                    {
                        new Order { Id = 1, ProductName = "Laptop", Quantity = 5, UserId = "70bfd920-d9e0-4587-9f97-f98aebb0b9bb"},
                        new Order { Id = 2, ProductName = "Smartphone", Quantity = 10, UserId = "70bfd920-d9e0-4587-9f97-f98aebb0b9bb" },
                        new Order { Id = 3, ProductName = "Headphones", Quantity = 15, UserId = "70bfd920-d9e0-4587-9f97-f98aebb0b9bb" },
                        new Order { Id = 4, ProductName = "Monitor", Quantity = 8, UserId = "3a9b9840-5a02-4d89-8550-fd5a1bbce21e"},
                        new Order { Id = 5, ProductName = "Keyboard", Quantity = 20, UserId = "3a9b9840-5a02-4d89-8550-fd5a1bbce21e"},
                        new Order { Id = 6, ProductName = "Mouse", Quantity = 25, UserId = "3a9b9840-5a02-4d89-8550-fd5a1bbce21e" }
                    };

                    context.Orders.AddRange(orders);
                    await context.SaveChangesAsync();
                }
            }
        }


        public static async Task SeedInventoryAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!context.Inventory.Any())
                {
                    var inventoryItems = new[]
                    {
                        new Inventory { Id = 1, ProductName = "Laptop", QuantityInWarehouse = 50 },
                        new Inventory { Id = 2, ProductName = "Smartphone", QuantityInWarehouse = 100 },
                        new Inventory { Id = 3, ProductName = "Headphones", QuantityInWarehouse = 150 },
                        new Inventory { Id = 4, ProductName = "Monitor", QuantityInWarehouse = 80 },
                        new Inventory { Id = 5, ProductName = "Keyboard", QuantityInWarehouse = 200 },
                        new Inventory { Id = 6, ProductName = "Mouse", QuantityInWarehouse = 250 }
                    };

                    context.Inventory.AddRange(inventoryItems);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
