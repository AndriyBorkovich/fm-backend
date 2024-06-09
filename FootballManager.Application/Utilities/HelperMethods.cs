using FootballManager.Domain.Enums;

namespace FootballManager.Application.Utilities
{
    public static class HelperMethods
    {
        public static (int, int, int) GetPositionsCount(Formation formation)
        {
            int defenders, midfielders, forwards;
            switch (formation)
            {
                case Formation.ThreeFiveTwo:
                    defenders = 3; midfielders = 5; forwards = 2;
                    break;
                case Formation.ThreeFourThree:
                    defenders = 3; midfielders = 4; forwards = 3;
                    break;
                case Formation.ThreeFourOneTwo:
                    defenders = 3; midfielders = 5; forwards = 2;
                    break;
                case Formation.ThreeThreeThreeOne:
                    defenders = 3; midfielders = 6; forwards = 1;
                    break;
                case Formation.ThreeThreeOneThree:
                    defenders = 3; midfielders = 4; forwards = 3;
                    break;
                case Formation.FourFourTwo:
                    defenders = 4; midfielders = 4; forwards = 2;
                    break;
                case Formation.FourThreeThree:
                    defenders = 4; midfielders = 3; forwards = 3;
                    break;
                case Formation.FourFiveOne:
                    defenders = 4; midfielders = 5; forwards = 1;
                    break;
                case Formation.FourTwoThreeOne:
                    defenders = 4; midfielders = 2; forwards = 3;
                    break;
                case Formation.FourOneFourOne:
                    defenders = 4; midfielders = 5; forwards = 1;
                    break;
                case Formation.FourFourOneOne:
                    defenders = 4; midfielders = 5; forwards = 1;
                    break;
                case Formation.FourThreeOneTwo:
                    defenders = 4; midfielders = 5; forwards = 2;
                    break;
                case Formation.FourTwoTwoTwo:
                    defenders = 4; midfielders = 4; forwards = 2;
                    break;
                case Formation.FourTwoFour:
                    defenders = 4; midfielders = 2; forwards = 4;
                    break;
                case Formation.FourOneThreeTwo:
                    defenders = 4; midfielders = 4; forwards = 2;
                    break;
                case Formation.FiveThreeTwo:
                    defenders = 5; midfielders = 3; forwards = 2;
                    break;
                case Formation.FiveFourOne:
                    defenders = 5; midfielders = 4; forwards = 1;
                    break;
                case Formation.FiveThreeOneOne:
                    defenders = 5; midfielders = 4; forwards = 1;
                    break;
                case Formation.FiveTwoThree:
                    defenders = 5; midfielders = 2; forwards = 3;
                    break;
                default:
                    defenders = 4; midfielders = 4; forwards = 2;
                    break;
            }

            return (defenders, midfielders, forwards);
        }
    }
}
