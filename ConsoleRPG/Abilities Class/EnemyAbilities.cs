using System;
using System.Collections.Generic;

public static class EnemyAbilities
{
  // Goblin Abilities
  public static List<Ability> GoblinAbilities = new List<Ability>()
  {
      new Ability("Attack", 0, 0, "Basic attack.", StatusEffectType.None, 0, 0),
      new Ability("Quick Strike", 0, 0, "Guarantees first action next turn.", StatusEffectType.None, 0, 0)
  };

  // Hob Goblin Abilities
  public static List<Ability> HobGoblinAbilities = new List<Ability>()
  {
      new Ability("Attack", 0, 0, "Basic attack.", StatusEffectType.None, 0, 0),
      new Ability("Quick Strike", 0, 0, "Guarantees first action next turn.", StatusEffectType.None, 0, 0),
      new Ability("Brace", 0, 2, "Reduce incoming damage by 50%.", StatusEffectType.Buff, 100, 0)
  };

  // Goblin Mage Abilities
  public static List<Ability> GoblinMageAbilities = new List<Ability>()
  {
      new Ability("Attack", 0, 0, "Basic attack.", StatusEffectType.None, 0, 0),
      new Ability("Fireball", 0, 2, "High damage. May inflict Burn.", StatusEffectType.Burn, 30, -10),
      new Ability("Ice Spike", 0, 2, "Medium damage. May inflict Frost.", StatusEffectType.Frost, 30, 0),
      new Ability("Poison Gas", 0, 4, "May poison the target.", StatusEffectType.Poison, 60, 15)
  };

  // Goblin Orc Abilities
  public static List<Ability> GoblinOrcAbilities = new List<Ability>()
  {
      new Ability("Attack", 0, 0, "Basic attack.", StatusEffectType.None, 0, 0),
      new Ability("Bash", 0, 3, "Attack twice.", StatusEffectType.None, 0, -15),
      new Ability("Brace", 0, 2, "Reduce incoming damage by 50%.", StatusEffectType.Buff, 100, 0)
  };

  // Green Goblin Abilities
  public static List<Ability> GreenGoblinAbilities = new List<Ability>()
  {
      new Ability("Attack", 0, 0, "Basic attack.", StatusEffectType.None, 0, 0),
      new Ability("Quick Strike", 0, 3, "Guarantees first action next turn.", StatusEffectType.None, 0, 10),
      new Ability("Smoke Screen", 0, 4, "Reduce player accuracy.", StatusEffectType.Blind, 100, 20),
      new Ability("Poison Strike", 0, 3, "Poisons the target.", StatusEffectType.Poison, 60, -10)
  };

  // ** Boss Abilities **

  // Goblin Ogre Abilities
  public static List<Ability> GoblinOgreAbilities = new List<Ability>()
  {
      new Ability("Attack", 0, 0, "Basic attack.", StatusEffectType.None, 0, 0),
      new Ability("Fire Blast", 0, 3, "Heavy fire damage. May inflict Burn.", StatusEffectType.Burn, 40, -15),
      new Ability("Bash", 0, 4, "Attack twice.", StatusEffectType.None, 0, -15),
      new Ability("Brace", 0, 2, "Reduce incoming damage by 50%.", StatusEffectType.Buff, 100, 0),
      new Ability("Quake", 0, 5, "Heavy damage and may reduce Speed and Accuracy.", StatusEffectType.Frost, 100, -20)
  };

  // Goblin Champion Abilities
  public static List<Ability> GoblinChampionAbilities = new List<Ability>()
  {
      new Ability("Attack", 0, 0, "Basic attack.", StatusEffectType.None, 0, 0),
      new Ability("Champion's Might", 0, 5, "Increase Attack, Defense, and Speed.", StatusEffectType.Buff, 100, 0),
      new Ability("Lightning Strike", 0, 3, "May stun the target.", StatusEffectType.Stun, 20, 0),
      new Ability("Brace", 0, 2, "Reduce incoming damage by 50%.", StatusEffectType.Buff, 100, 0),
      new Ability("Quick Strike", 0, 3, "Guarantees first action next turn.", StatusEffectType.None, 0, 10),
      new Ability("Impact Slash", 0, 5, "Massive damage with reduced accuracy.", StatusEffectType.None, 0, -25)
  };
}


 