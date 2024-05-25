using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasData(
            new List<Player>()
            {
                new Player()
                {
                    Id = 1,
                    CreatedDate = DateTime.UtcNow,
                    Name = "Cristiano Ronaldo",
                    BirthDay = new DateTime(1985, 2, 5),
                    ClubId = 1
                }
            });
    }
}