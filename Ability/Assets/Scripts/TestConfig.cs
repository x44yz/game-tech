
public struct SkillConfig
{
    public string id { get; set; }
    public string name { get; set; }
    public int value { get; set; }
    public int cooldown { get; set;}
}

public class TestConfig
{
    public static SkillConfig[] skillConfigs = {
        new SkillConfig() {
            id = "heal",
            name = "add hp",
            value = 10,
            cooldown = 10
        },
    };
}
