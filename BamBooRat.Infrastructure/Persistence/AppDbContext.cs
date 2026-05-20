using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }


  public override int SaveChanges()
  {
    UpdateAuditFields();
    return base.SaveChanges();
  }

  public override async Task<int> SaveChangesAsync(
      CancellationToken cancellationToken = default)
  {
    UpdateAuditFields();
    return await base.SaveChangesAsync(cancellationToken);
  }

  public DbSet<Cage> Cages { get; set; }
  public DbSet<Rat> Rats { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    // Cage configuration
    builder.Entity<Cage>(entity =>
    {
      entity.ToTable("Cages");

      entity.HasKey(x => x.Id);

      entity.Property(x => x.Id)
             .HasColumnName("Id")
             .HasDefaultValueSql("NEWSEQUENTIALID()");

      entity.Property(x => x.Name)
             .HasColumnName("Name")
             .HasMaxLength(100)
             .IsRequired();

      entity.Property(x => x.Description)
             .HasColumnName("Description")
             .HasMaxLength(255);

      entity.Property(x => x.Capacity)
             .HasColumnName("Capacity")
             .IsRequired();

      entity.Property(x => x.IsActive)
             .HasColumnName("IsActive")
             .HasDefaultValue(true);

      entity.Property(x => x.CreatedDate)
              .HasColumnName("CreatedDate")
              .HasDefaultValueSql("GETUTCDATE()");

      entity.Property(x => x.UpdatedDate)
           .HasColumnName("UpdatedDate");
    });
    // Rat configuration
    builder.Entity<Rat>(entity =>
    {
      entity.ToTable("Rats");

      entity.HasKey(x => x.Id);

      entity.Property(x => x.Id)
          .HasColumnName("Id")
          .HasDefaultValueSql("NEWSEQUENTIALID()");

      entity.Property(x => x.Code)
          .HasColumnName("Code")
          .HasMaxLength(50)
          .IsRequired();

      entity.HasIndex(x => x.Code)
          .IsUnique();

      entity.Property(x => x.Name)
          .HasColumnName("Name")
          .HasMaxLength(100)
          .IsRequired();

      entity.Property(x => x.Description)
          .HasColumnName("Description")
          .HasMaxLength(255);

      entity.Property(x => x.Type)
          .HasColumnName("Type")
          .IsRequired();

      entity.Property(x => x.Status)
          .HasColumnName("Status")
          .IsRequired();

      entity.Property(x => x.Gender)
          .HasColumnName("Gender")
          .IsRequired();

      entity.Property(x => x.DateOfBirth)
          .HasColumnName("DateOfBirth");


      entity.Property(x => x.CreatedDate)
          .HasColumnName("CreatedDate")
          .HasDefaultValueSql("GETUTCDATE()");

      entity.Property(x => x.UpdatedDate)
          .HasColumnName("UpdatedDate");

      entity.Property(x => x.CageId)
          .HasColumnName("CageId")
          .IsRequired();

      entity.HasOne(x => x.Cage)
          .WithMany(c => c.Rats)
          .HasForeignKey(x => x.CageId)
          .OnDelete(DeleteBehavior.Cascade);
    });
  }
  private void UpdateAuditFields()
  {
    var entries = ChangeTracker.Entries();

    foreach (var entry in entries)
    {
      // UPDATE
      if (entry.State == EntityState.Modified)
      {
        if (entry.Entity is BaseEntity entity)
        {
          entity.UpdatedDate = DateTime.UtcNow;
        }
      }
    }
  }
}