using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

public class Enemy : Character
{
    // Reward properties
    public int EXPRewards {get; set;}
    public int GoldRewards {get; set;}
    public List<Ability> EnemyAbilities {get; set;}


    public Enemy(string name, int maxHP, int attack, int defense, int speed, int accuracy, int critChance, int expRewards, int goldReward, List<Ability> enemyAbilities)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Accuracy = accuracy;
        CritChance = critChance;
        IsDefending = false;

        // Reward fields
        EXPRewards = expRewards;
        GoldRewards = goldReward;

        // List for abilities
        EnemyAbilities = enemyAbilities;
    }












    // Enemy Abilities -------------------------------------------------------

    // Function for choosing the enemyABility
    public Ability ChoooseEnemyAbility()
    {

      List<Ability> availableAbilities = new List<Ability>();

      foreach(Ability ability in EnemyAbilities)
      {
        if(ability.IsReady())
        {
          availableAbilities.Add(ability);
        }
      }

      if(availableAbilities.Count == 0)
      {
          return EnemyAbilities[0];
      }

      return availableAbilities[GameSettings.RNG.Next(availableAbilities.Count)];
    }

    // Function for enemy attack
    public void EnemyAttack(Player player)
    {
      Ability selectedAbility = ChoooseEnemyAbility();

      CombatSystem.ExecuteDamageAbility(this, player, selectedAbility);
      
      selectedAbility.StartCooldown();
    }

}