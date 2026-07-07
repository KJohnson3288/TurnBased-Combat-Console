public static class AbilityDB
{
  // This is the Databased for abilities that will be used by characters in this game
    public static Ability Attack = new Ability("Attack", 1, 1, 0, 0, "Basic Attack", StatusEffectType.None, 0, 0, 0);

    public static Ability Defend = new Ability("Defend", 1, 0, 0, 2, "Reduce Damage Taken", StatusEffectType.Defend, 100, 1, 0);

    public static Ability HeavySlash = new Ability("Heavy Slash", 1, GameSettings.RNG.Next(1, 3), 2, 2, "Damage between Attack * 1 - Attack * 3", StatusEffectType.None, 0, 0, -10);

    public static Ability MultiStrike = new Ability("Multi-Strike", 2, 1, 4, 3, "Strike 4 times", StatusEffectType.None, 0, 0, -20);
    
    public static Ability PoisonStrike = new Ability("Poison Strike", 1, 1, 2, 3, "Strike with a chance to inflict poison", StatusEffectType.Poison, 40, 2, -10);

}