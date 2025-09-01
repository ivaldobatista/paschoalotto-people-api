using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paschoalotto.People.Domain.People;

namespace Paschoalotto.People.Infrastructure.Persistence.Configurations;

public sealed class IndividualConfiguration : IEntityTypeConfiguration<Individual>
{
    public void Configure(EntityTypeBuilder<Individual> b)
    {
        b.ToTable("Individuals");
        b.HasKey(x => x.Id);

        b.Property(x => x.FullName)
            .HasMaxLength(200)
            .IsRequired();

        b.OwnsOne(x => x.Cpf, v =>
        {
            v.Property(p => p.Value)
             .HasColumnName("Cpf")
             .HasMaxLength(11)
             .IsRequired();
            v.HasIndex(p => p.Value).IsUnique();
        });

        b.Property(x => x.BirthDate).IsRequired();
        b.Property(x => x.Gender).IsRequired();

        b.OwnsOne(x => x.Email, v =>
        {
            v.Property(p => p.Value)
             .HasColumnName("Email")
             .HasMaxLength(255)
             .IsRequired();
            v.HasIndex(p => p.Value);
        });

        b.OwnsOne(x => x.Phone, v =>
        {
            v.Property(p => p.Value)
             .HasColumnName("Phone")
             .HasMaxLength(32)
             .IsRequired();
        });

        b.OwnsOne(x => x.Address, v =>
        {
            v.Property(p => p.Street).HasMaxLength(120).HasColumnName("Street").IsRequired();
            v.Property(p => p.Number).HasMaxLength(15).HasColumnName("Number").IsRequired();
            v.Property(p => p.Complement).HasMaxLength(60).HasColumnName("Complement");
            v.Property(p => p.District).HasMaxLength(80).HasColumnName("District").IsRequired();
            v.Property(p => p.City).HasMaxLength(80).HasColumnName("City").IsRequired();
            v.Property(p => p.State).HasMaxLength(40).HasColumnName("State").IsRequired();
            v.Property(p => p.ZipCode).HasMaxLength(20).HasColumnName("ZipCode").IsRequired();
            v.Property(p => p.Country).HasMaxLength(60).HasColumnName("Country").IsRequired();
        });

        b.Property(x => x.PhotoPath).HasMaxLength(255);
        b.Property(x => x.CreatedAtUtc).IsRequired();
        b.Property(x => x.UpdatedAtUtc).IsRequired();

        b.HasIndex(x => x.FullName);
    }
}
