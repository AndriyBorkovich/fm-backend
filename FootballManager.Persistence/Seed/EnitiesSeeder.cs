using Bogus;
using EFCore.BulkExtensions;
using FootballManager.Application.Features.Match.Commands.BulkSimulate;
using FootballManager.Application.Features.Match.Commands.Simulate;
using FootballManager.Application.Utilities;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Persistence.DatabaseContext;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Bogus.DataSets.Name;

namespace FootballManager.Persistence.Seed
{
    public class EnitiesSeeder(IServiceProvider serviceProvider, ISender sender) : IEntitiesSeeder
    {
        /// <summary>
        /// Seed data only for top 5 leagues
        /// </summary>
        public async Task Run()
        {
            var premierLeagueClubs = new List<string>
            {
                "Arsenal", "Aston Villa", "Bournemouth", "Brentford", "Brighton & Hove Albion",
                "Burnley", "Chelsea", "Crystal Palace", "Everton", "Fulham",
                "Liverpool", "Luton Town", "Manchester City", "Manchester United", "Newcastle United",
                "Nottingham Forest", "Sheffield United", "Tottenham Hotspur", "West Ham United", "Wolverhampton Wanderers"
            };
            var premierLeagueStadiums = new List<string>
            {
                "Emirates Stadium", "Villa Park", "Vitality Stadium", "Brentford Community Stadium", "Amex Stadium",
                "Turf Moor", "Stamford Bridge", "Selhurst Park", "Goodison Park", "Craven Cottage",
                "Anfield", "Kenilworth Road", "Etihad Stadium", "Old Trafford", "St James' Park",
                "City Ground", "Bramall Lane", "Tottenham Hotspur Stadium", "London Stadium", "Molineux Stadium"
            };

            var laLigaClubs = new List<string>
            {
                "Alavés", "Almería", "Athletic Bilbao", "Atlético Madrid", "Barcelona",
                "Cádiz", "Celta Vigo", "Elche", "Espanyol", "Getafe",
                "Granada", "Las Palmas", "Mallorca", "Osasuna", "Rayo Vallecano",
                "Real Betis", "Real Madrid", "Real Sociedad", "Sevilla", "Valencia",
                "Valladolid", "Villarreal"
            };
            var laLigaStadiums = new List<string>
            {
                "Mendizorrotza", "Power Horse Stadium", "San Mamés", "Wanda Metropolitano", "Camp Nou",
                "Nuevo Mirandilla", "Abanca-Balaídos", "Martínez Valero", "RCDE Stadium", "Coliseum Alfonso Pérez",
                "Nuevo Los Cármenes", "Gran Canaria", "Visit Mallorca Estadi", "El Sadar", "Vallecas",
                "Benito Villamarín", "Santiago Bernabéu", "Reale Arena", "Ramón Sánchez Pizjuán", "Mestalla",
                "José Zorrilla", "Estadio de la Cerámica"
            };

            var serieAClubs = new List<string>
            {
                "Atalanta", "Bologna", "Cagliari", "Empoli", "Fiorentina",
                "Frosinone", "Genoa", "Hellas Verona", "Inter Milan", "Juventus",
                "Lazio", "Lecce", "AC Milan", "Napoli", "Roma",
                "Salernitana", "Sassuolo", "Spezia", "Torino", "Udinese"
            };
            var serieAStadiums = new List<string>
            {
                "Gewiss Stadium", "Stadio Renato Dall'Ara", "Unipol Domus", "Stadio Carlo Castellani", "Stadio Artemio Franchi",
                "Stadio Benito Stirpe", "Stadio Luigi Ferraris", "Stadio Marcantonio Bentegodi", "San Siro", "Allianz Stadium",
                "Stadio Olimpico", "Stadio Via del Mare", "San Siro", "Stadio Diego Armando Maradona", "Stadio Olimpico",
                "Stadio Arechi", "Stadio Città del Tricolore", "Stadio Alberto Picco", "Stadio Olimpico Grande Torino", "Dacia Arena"
            };

            var bundesligaClubs = new List<string>
            {
                "Augsburg", "Bayer Leverkusen", "Bayern Munich", "Bochum", "Borussia Dortmund",
                "Borussia Mönchengladbach", "Eintracht Frankfurt", "Freiburg", "Heidenheim", "Hoffenheim",
                "Köln", "Mainz", "RB Leipzig", "Union Berlin", "Stuttgart",
                "Werder Bremen", "Wolfsburg"
            };
            var bundesligaStadiums = new List<string>
            {
                "WWK Arena", "BayArena", "Allianz Arena", "Vonovia Ruhrstadion", "Signal Iduna Park",
                "Borussia-Park", "Deutsche Bank Park", "Europa-Park Stadion", "Voith-Arena", "PreZero Arena",
                "RheinEnergieStadion", "Mewa Arena", "Red Bull Arena", "Stadion An der Alten Försterei", "Mercedes-Benz Arena",
                "Weser-Stadion", "Volkswagen Arena"
            };

            var ligue1Clubs = new List<string>
            {
                "Ajaccio", "Angers", "Auxerre", "Brest", "Clermont",
                "Lens", "Lille", "Lorient", "Lyon", "Marseille",
                "Monaco", "Montpellier", "Nantes", "Nice", "Paris Saint-Germain",
                "Reims", "Rennes", "Strasbourg", "Toulouse", "Troyes"
            };
            var ligue1Stadiums = new List<string>
            {
                "Stade François Coty", "Stade Raymond Kopa", "Stade Abbé-Deschamps", "Stade Francis-Le Blé", "Stade Gabriel Montpied",
                "Stade Bollaert-Delelis", "Stade Pierre-Mauroy", "Stade du Moustoir", "Groupama Stadium", "Stade Vélodrome",
                "Stade Louis II", "Stade de la Mosson", "Stade de la Beaujoire", "Allianz Riviera", "Parc des Princes",
                "Stade Auguste-Delaune", "Roazhon Park", "Stade de la Meinau", "Stadium de Toulouse", "Stade de l'Aube"
            };

            var leagues = new Dictionary<string, (List<string> Clubs, List<string> Stadiums)>
            {
                { "Premier League", (premierLeagueClubs, premierLeagueStadiums) },
                { "La Liga", (laLigaClubs, laLigaStadiums) },
                { "Serie A", (serieAClubs, serieAStadiums) },
                { "Bundesliga", (bundesligaClubs, bundesligaStadiums) },
                { "Ligue 1", (ligue1Clubs, ligue1Stadiums) }
            };

            var coachFaker = new Faker<Coach>()
               .RuleFor(co => co.CreatedDate, f => DateTime.UtcNow)
               .RuleFor(co => co.Name, f => f.Name.FullName(Gender.Male))
               .RuleFor(co => co.Nationality, f => f.Address.Country())
               .RuleFor(co => co.BirthDay, f => f.Date.Past(60, DateTime.Now.AddYears(-25)))
               .RuleFor(co => co.CoachingStyle, f => f.PickRandom<CoachingStyle>())
               .RuleFor(co => co.PreferredFormation, f => f.PickRandom<Formation>());

            var clubsByLeague = new Dictionary<string, List<Club>>();

            foreach (var league in leagues.Keys)
            {
                var leagueClubs = new List<Club>();

                for (var i = 0; i < leagues[league].Clubs.Count; i++)
                {
                    var club = new Club
                    {
                        CreatedDate = DateTime.UtcNow,
                        Name = leagues[league].Clubs[i],
                        StadiumName = leagues[league].Stadiums[i],
                        Type = ClubType.Standard,
                        Coach = coachFaker.Generate()
                    };

                    club.Players = SeedPlayersForClub(club.Coach);

                    leagueClubs.Add(club);
                }

                clubsByLeague.Add(league, leagueClubs);
            }

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FootballManagerContext>();

            foreach (var league in clubsByLeague.Keys)
            {
                await context.BulkInsertAsync(clubsByLeague[league], b => b.IncludeGraph = true);
            }

            // generate league and put corresponding clubs in it
            var championships = new List<Championship>();
            foreach (var league in clubsByLeague.Keys)
            {
                var championship = new Championship
                {
                    CreatedDate = DateTime.UtcNow,
                    Name = league!,
                    Country = GetCountryByLeague(league!),
                    Type = ChampionshipType.CoutryLeague,
                    ParticipatingClubs = clubsByLeague[league!]
                };
                championships.Add(championship);
                foreach (var club in clubsByLeague[league])
                {
                    club.ParticipatingChampionships ??= [];
                    club.ParticipatingChampionships.Add(championship);
                }
            }

            await context.BulkInsertAsync(championships, b => b.IncludeGraph = true);
            foreach (var league in clubsByLeague.Keys)
            {
                await context.BulkUpdateAsync(clubsByLeague[league], b => b.IncludeGraph = true);
            }

            // Generate seasons for each championship
            var seasons = new List<Season>();
            foreach (var championship in championships)
            {
                for (var year = DateTime.Now.Year - 2; year < DateTime.Now.Year; year++)
                {
                    var season = new Season
                    {
                        CreatedDate = DateTime.UtcNow,
                        StartYear = year,
                        EndYear = year + 1,
                        ChampionshipId = championship.Id,
                        Championship = championship,
                    };
                    seasons.Add(season);
                }
            }

            await context.BulkInsertAsync(seasons, b => b.IncludeGraph = true);

            foreach (var season in seasons)
            {
                await GenerateMatchesForSeason(season);
            }

            await context.BulkUpdateAsync(seasons, b => b.IncludeGraph = true);

            static string GetCountryByLeague(string leagueName)
            {
                return leagueName switch
                {
                    "Premier League" => "England",
                    "La Liga" => "Spain",
                    "Serie A" => "Italy",
                    "Bundesliga" => "Germany",
                    "Ligue 1" => "France",
                    _ => "England"
                };
            }

            static List<Player> SeedPlayersForClub(Coach coach)
            {
                var genericPlayerFaker = new Faker<Player>()
                   .RuleFor(p => p.CreatedDate, f => DateTime.UtcNow)
                   .RuleFor(p => p.Name, f => f.Name.FullName(Gender.Male))
                   .RuleFor(p => p.Nationality, f => f.Address.Country())
                   .RuleFor(p => p.BirthDay, f => f.Date.Past(40, DateTime.Now.AddYears(-16)));

                // seeder for the minimum available team for matches
                var goalieFaker = genericPlayerFaker.RuleFor(p => p.Position, f => PlayerPosition.Goalkeeper);
                var defFaker = genericPlayerFaker.RuleFor(p => p.Position, f => PlayerPosition.Defender);
                var midFaker = genericPlayerFaker.RuleFor(p => p.Position, f => PlayerPosition.Midfielder);
                var forwFaker = genericPlayerFaker.RuleFor(p => p.Position, f => PlayerPosition.Forward);
                var randPosFaker = genericPlayerFaker.RuleFor(p => p.Position, f => f.PickRandom<PlayerPosition>());

                var (defenersCount, midfieldersCount, forwardsCount) = HelperMethods.GetPositionsCount(coach.PreferredFormation);

                return
                    [
                    .. goalieFaker.Generate(1),
                    .. defFaker.Generate(defenersCount),
                    .. midFaker.Generate(midfieldersCount),
                    .. forwFaker.Generate(forwardsCount),
                    .. randPosFaker.Generate(new Random().Next(10, 20)) // bench
                    ];
            }

            async Task GenerateMatchesForSeason(Season season)
            {
                var rand = new Random();

                var championship = season.Championship;

                var pairs = new List<SimulateMatchCommand>();

                foreach (var homeClub in championship.ParticipatingClubs)
                {
                    foreach (var awayClub in championship.ParticipatingClubs)
                    {
                        if (homeClub.Id != awayClub.Id)
                        {
                            pairs.Add(new SimulateMatchCommand(homeClub.Id, awayClub.Id, season.Id));
                        }
                    }
                }
                await sender.Send(new BulkMatchSimulationCommand(pairs));
            }
        }
    }
}
