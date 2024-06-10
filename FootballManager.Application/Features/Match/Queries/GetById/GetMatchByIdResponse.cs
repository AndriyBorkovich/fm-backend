namespace FootballManager.Application.Features.Match.Queries.GetById
{
    public class GetMatchByIdResponse
    {
        public DateTime MatchDate { get; set; }
        public string HomeTeamName { get; set; }
        public int HomeTeamGoals { get; set; }
        public string AwayTeamName { get; set; }
        public int AwayTeamGoals { get; set; }
        public string Result { get; set; }
        public List<GoalEvent> Goals { get; set; }
        public List<CardEvent> Cards { get; set; }
    }

    public class GoalEvent
    {
        public int Minute { get; set; }
        public string Scorer { get; set; }
        public string Assist { get; set; }
        public bool IsHomeTeam { get; set; }
    }

    public class CardEvent
    {
        public int Minute { get; set; }
        public string PlayerName { get; set; }
        public string CardType { get; set; }
        public bool IsHomeTeam { get; set; }
    }
}
