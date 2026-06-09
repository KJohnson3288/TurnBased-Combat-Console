public class Ability
{
    public string? Name {get; set;}
    public int MPCost {get; set;}
    public int Damage {get; set;}
    public int Cooldown {get; set;}
    public int CurrentCooldown {get; set;}
    public string Description {get; set;}
    public StatusEffectType Effect {get; set;}
    public int EffectChance {get; set;}
    public int AccuracyModifier {get; set;}

    

//  Character ability constructor
    public Ability(string name, int mpCost, int cooldown, string description, StatusEffectType effect, int effectChance, int accuracyModifier)
    {
        Name = name;
        MPCost = mpCost;
        Cooldown = cooldown;
        CurrentCooldown = 0;
        Description = description;
        Effect = effect;
        EffectChance = effectChance;
        AccuracyModifier = accuracyModifier;
    }

}