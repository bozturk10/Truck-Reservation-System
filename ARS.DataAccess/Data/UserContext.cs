using ARS.DataAccess;
//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARS.DataAccess.DataSQL
{
    //public class UserContext : DbContext
    //{
    //    public UserContext(): base("DefaultConnection")
    //    {

    //    }
    //    public DbSet<User> Users { get; set; }
    //    public DbSet<Role> Roles { get; set; }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        //Configure default schema
    //        modelBuilder.HasDefaultSchema("Admin");

    //        //Map entity to table
    //        modelBuilder.Entity<User>().ToTable("User");
    //        modelBuilder.Entity<Role>().ToTable("Role", "dbo");
            
    //        //modelBuilder.Entity<User>()
    //        //    .HasMany(u => u.Roles)
    //        //    .WithMany(r => r.Users)
    //        //    .Map(m =>
    //        //    {
    //        //        m.ToTable("UserRoles");
    //        //        m.MapLeftKey("UserId");
    //        //        m.MapRightKey("RoleId");
    //        //    });
    //    }


    //}
}
