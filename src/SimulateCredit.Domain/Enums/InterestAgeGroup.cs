namespace SimulateCredit.Domain.Enums;

public enum InterestAgeGroup
{
    UP_TO_25,         // < 25 anos = 5%
    FROM_26_TO_40,    // 26 a 40 anos = 3%
    FROM_41_TO_60,    // 41 a 60 anos = 2%
    ABOVE_60          // > 60 anos = 4%
}
