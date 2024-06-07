namespace FootballManager.Domain.Enums;

[Flags]
public enum CoachingStyle
{
    Offensive = 2,
    Defensive = 4,
    BallControll = 8,
    CounterAttacking = 16,
    HighPressing = 32
}
