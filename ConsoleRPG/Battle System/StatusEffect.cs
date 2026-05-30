public class StatusEffect
{
    public string Type {get; set;}
    public int Duration {get; set;}

    public StatusEffect(string type, int duration)
    {
        Type = type;
        Duration = duration;
    }
}