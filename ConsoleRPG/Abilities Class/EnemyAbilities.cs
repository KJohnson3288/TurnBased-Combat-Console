using System;
using System.Collections.Generic;

public static class EnemyAbilities
{
  // Enemy ability list are set here, This can be altered and changed to add or remove abilities from a characters ability pool
  
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


 