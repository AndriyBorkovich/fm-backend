using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.Configurations;

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.HasData(
            new List<Club>()
            {
                new Club()
                {
                    Id = 1,
                    CreatedDate = DateTime.UtcNow,
                    Name = "Real Madrid",
                    StadiumName = "Santiago Bernabeu",
                    Type = ClubType.Standard
                }
            });
    }
}