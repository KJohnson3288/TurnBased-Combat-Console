using System;
using System.Collections.Generic;

public static class EnemyAbilities
{
  // Goblin Abilities
  public static List<Ability> GoblinAbilities = new List<Ability>()
  {
    AbilityDB.Attack,
    AbilityDB.Defend
  };

  // Hob Goblin Abilities
  public static List<Ability> HobGoblinAbilities = new List<Ability>()
  {
    AbilityDB.Attack,
    AbilityDB.PoisonStrike
  };

  // ** Boss Abilities **

  // Big Goblin
  public static List<Ability> GoblinChampionAbilities = new List<Ability>()
  {
    AbilityDB.Attack,
    AbilityDB.Defend,
    AbilityDB.MultiStrike
  };
}


 