using E_Commmerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_Commmerce.ViewModels.User;
using E_Commmerce.ViewModels;

namespace E_Commmerce.Data;

public class ApplicationDbcontext :IdentityDbContext<ApplicationUser>
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
    {
    }
   


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        builder.Entity<Category>()
            .HasData(
                    new Category { Id = 1, Name = "Electronics" },
                    new Category { Id = 2, Name = "Clothes" },
                    new Category { Id = 3, Name = "Shoes" },
                    new Category { Id = 4, Name = "Books" },
                    new Category { Id = 5, Name = "Furniture" },
                    new Category { Id = 6, Name = "Accessories" },
                    new Category { Id = 7, Name = "Beauty" },
                    new Category { Id = 8, Name = "Sports" },
                    new Category { Id = 9, Name = "Health" },
                    new Category { Id = 10, Name = "Toys" },
                    new Category { Id = 11, Name = "Food" },
                    new Category { Id = 12, Name = "Beverages" },
                    new Category { Id = 13, Name = "Home" },
                    new Category { Id = 14, Name = "Garden" },
                    new Category { Id = 15, Name = "Tools" },
                    new Category { Id = 16, Name = "Automotive" }
                    );

        builder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .OnDelete(DeleteBehavior.Cascade);
    }
   


    public DbSet<RoleViewModel> RoleViewModel { get; set; } = default!;

    

}
