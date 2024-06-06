using System.ComponentModel;

namespace FootballManager.Domain.Enums;

public enum Formation
{
    // 3 Defenders
    [Description("3-5-2")]
    ThreeFiveTwo = 352,

    [Description("3-4-3")]
    ThreeFourThree = 343,

    [Description("3-4-1-2")]
    ThreeFourOneTwo = 3412,

    [Description("3-3-3-1")]
    ThreeThreeThreeOne = 3331,

    [Description("3-3-1-3")]
    ThreeThreeOneThree = 3313,

    // 4 Defenders
    [Description("4-4-2")]
    FourFourTwo = 442,

    [Description("4-3-3")]
    FourThreeThree = 433,

    [Description("4-5-1")]
    FourFiveOne = 451,

    [Description("4-2-3-1")]
    FourTwoThreeOne = 4231,

    [Description("4-1-4-1")]
    FourOneFourOne = 4141,

    [Description("4-4-1-1")]
    FourFourOneOne = 4411,

    [Description("4-3-1-2")]
    FourThreeOneTwo = 4312,

    [Description("4-2-2-2")]
    FourTwoTwoTwo = 4222,

    [Description("4-2-4")]
    FourTwoFour = 424,

    [Description("4-1-3-2")]
    FourOneThreeTwo = 4132,

    // 5 Defenders
    [Description("5-3-2")]
    FiveThreeTwo = 532,

    [Description("5-4-1")]
    FiveFourOne = 541,

    [Description("5-3-1-1")]
    FiveThreeOneOne = 5311,

    [Description("5-2-3")]
    FiveTwoThree = 523
}
