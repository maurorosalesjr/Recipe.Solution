using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Recipe.Models
{
  public class RecipeContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Formula> Formulas { get; set; }
    public DbSet<Box> Box { get; set; }

    public RecipeContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}
