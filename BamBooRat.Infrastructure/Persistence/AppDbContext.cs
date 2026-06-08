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
    public DbSet<Breeding> Breedings { get; set; }
    public DbSet<CageTransfer> CageTransfers { get; set; }
    public DbSet<WeightHistory> weightHistories { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Global query filter for soft delete
        builder.Entity<Breeding>().HasQueryFilter(u => !u.IsDeleted);

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

        builder.Entity<Breeding>()
               .HasOne(b => b.Cage)
               .WithMany()
               .HasForeignKey(b => b.CageId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Breeding>()
            .HasOne(b => b.Male)
            .WithMany(r => r.MaleBreedings)
            .HasForeignKey(b => b.MaleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Breeding>()
            .HasOne(b => b.Female)
            .WithMany(r => r.FemaleBreedings)
            .HasForeignKey(b => b.FemaleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Breeding>().ToTable("Breedings");


        builder.Entity<CageTransfer>(entity =>
           {
               entity.Property(x => x.Id)
               .HasColumnName("Id")
               .HasDefaultValueSql("NEWSEQUENTIALID()");

               entity.Property(x => x.Note)
               .HasColumnName("Note")
               .HasMaxLength(500);

               entity.Property(x => x.TransferDate)
               .HasColumnName("TransferDate")
               .IsRequired();

               entity.Property(x => x.Reason)
               .HasColumnName("Reason")
               .HasConversion<int>();

               entity.HasOne(x => x.Rat)
               .WithMany(x => x.CageTransfers)
               .HasForeignKey(x => x.RatId)
               .OnDelete(DeleteBehavior.Restrict);

               entity.HasOne(x => x.FromCage)
               .WithMany(x => x.OutgoingTransfers)
               .HasForeignKey(x => x.FromCageId)
               .OnDelete(DeleteBehavior.Restrict);

               entity.HasOne(x => x.ToCage)
               .WithMany(x => x.IncomingTransfers)
               .HasForeignKey(x => x.ToCageId)
               .OnDelete(DeleteBehavior.Restrict);
               entity.ToTable("CageTransfers");
           });

        builder.Entity<WeightHistory>(entity =>
        {
            entity.Property(x => x.Id)
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()");

            entity.Property(x => x.Note)
            .HasColumnName("Note")
            .HasMaxLength(500);

            entity.Property(x => x.RecordedDate)
            .HasColumnName("RecordedDate")
            .IsRequired();

            entity.Property(x => x.Weight)
            .HasColumnName("Weight")
            .IsRequired()
            .HasPrecision(10, 2);

            entity.HasOne(x => x.Rat)
            .WithMany(x => x.WeightHistories)
            .HasForeignKey(x => x.RatId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable("WeightHistories");
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