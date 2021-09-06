
public class TestDef
{
    // one day = 48s
    public const float UnitRealHourToGameSecond = 48f / 24f;
    
    public static float RealHourToGameSeconds(float hour)
    {
        return hour * UnitRealHourToGameSecond;
    }
}