using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Security.AccessControl;

public class Player : Character
{
     // Fields for rewards
    public int Gold {get; set;}
    public int Exp {get; set;}
    public int Level = 0;
    public int ExpToLevel = 100;


    public Player(string name, int maxHP, int maxMP, int attack, int defense, int speed, int accuracy, int critChance)
    {
        Name = name;
        MaxHP= maxHP;
        MaxMP = maxMP;
        CurrentHP = maxHP;
        CurrentMP = maxMP;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Accuracy = accuracy;
        CritChance = critChance;
        IsDefending = false;
    }


    // List of player abilities 
    public List<Ability> abilities = new List<Ability>()
    {
      AbilityDB.Attack,
      AbilityDB.Defend,
      AbilityDB.HeavySlash,
      AbilityDB.MultiStrike,
      AbilityDB.PoisonStrike
    };


    // Player Function to choose the ability -----------------------------------------
    public Ability Choice(string input)
    {
      switch(input)
      {
        case "1":
          return AbilityDB.Attack;
        case "2":
          return AbilityDB.Defend;
        case "3":
          return AbilityDB.HeavySlash;
        case "4":
          return AbilityDB.MultiStrike;
        case "5":
          return AbilityDB.PoisonStrike;
        default:
          return AbilityDB.Attack;
      }
    }

    

    // Player Attack ----------------------------------------
    
    public void PlayerAttack(Character enemy, Ability ability)
    {
      if(CurrentMP >= ability.MPCost)
      {
        CurrentMP -= ability.MPCost;

        CombatSystem.ExecuteDamageAbility(this, enemy, ability);
      } else
      {
        Console.WriteLine($"\n{Name} does not have enough MP!");
        CombatSystem.ExecuteDamageAbility(this, enemy, AbilityDB.Attack);
      }

    }


    public void Heal(int amount)
    {
        CurrentHP+= amount;
        if (CurrentHP > MaxHP)
        {
            CurrentHP= MaxHP;
        }

        CurrentMP += amount;
        if (CurrentMP > MaxMP)
        {
            CurrentMP = MaxMP;
        }
    }
  

    // End of battle rewards and level uo check
    public void CheckLevelUp()
    {
        if(Exp >= ExpToLevel)
        {
            Console.WriteLine($"\nYou have leveled up!");

            // Subtract the remainder to carry over towards the next level
            Exp -= ExpToLevel;
            
            // increase level
            Level += 1;


            // Update player stats 
            MaxHP += 10;
            MaxMP += 5;
            Attack += 2;
            Defense += 1;
            Speed += 1; 

            Console.WriteLine($"HP increased!: {MaxHP - 10} > {MaxHP}");
            Console.WriteLine($"MP increased!: {MaxMP - 5} > {MaxMP}");
            Console.WriteLine($"Attack increased!: {Attack - 2} > {Attack}");
            Console.WriteLine($"Defense increased!: {Defense - 1} > {Defense}");
            Console.WriteLine($"Speed increased!: {Speed - 1} > {Speed}");

            
            // Refill HP / MP and increase level requirement
            CurrentHP= MaxHP;
            CurrentMP = MaxMP;
            ExpToLevel += 50; 
        }
    }

}