using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Models;
namespace PhoneBook;

public partial class PhoneBookDbSaiko2307b2Context : DbContext
{
    public PhoneBookDbSaiko2307b2Context()
    {
    }

    public PhoneBookDbSaiko2307b2Context(DbContextOptions<PhoneBookDbSaiko2307b2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contacts__3214EC07CAB4834C");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
