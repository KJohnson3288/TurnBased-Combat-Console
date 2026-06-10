using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Security.AccessControl;

public class Player : Character
{
    public bool IsDefending { get; set; }

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


    public void Heal(int amount)
    {
        CurrentHP+= amount;
        if (CurrentHP> MaxHP)
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

    // List of player actions 
    public List<Ability> abilities = new List<Ability>()
    {
        new Ability("Attack", 0, 0, "Basic Attack", StatusEffectType.None, 0, 0),
        new Ability("Defend", 0, 0, "Reduce Damage Taken", StatusEffectType.None, 0, 0),
        new Ability("Heavy Slash", 2, 0, "Damage between Attack + 5 - Attack + 10", StatusEffectType.None, 0, -10),
        new Ability("Multi-Strike", 4, 0, "Strike 4 times", StatusEffectType.None, 0, -20),
        new Ability("Poison Strike", 2, 0, "Strike with a chance to inflict poison", StatusEffectType.Poison, 40, -10)
    };


    // Player Function to attack -----------------------------------------
    public void PlayerAttack(string input, Enemy enemy)
    {
        // Take in player input and run action
        switch(input)
        {
            case "1":
            {
              BaseAttack(enemy);
            }
            break;
            case"2":
            {
              PlayerDefend();
            } 
            break;
            case "3":
            {
              HeavySlash(enemy);
            }
            break;
            case "4":
            {
              MultiStrike(enemy);
            }
            break;
            case "5":
            {
              PoisonStrike(enemy);
            }
            break;
        }
    }

    // Player abilities-----------------------------------------------------
    

    // Player Attack
    public void BaseAttack(Enemy enemy)
    {
      // Adding battle calc for hit success
      int attackRoll = GameSettings.RNG.Next(0, 100); 
      int critHit = GameSettings.RNG.Next(0, 100);
      int result;

      if(attackRoll < Accuracy)
      {
          if(critHit < CritChance)
              {
                  // If crit is triggered we set the ceic param to true to properly calc damage
                  Console.WriteLine("\nYou landed a critical hit!");
                  Console.WriteLine($"You rolled ({attackRoll}) / Acc: {Accuracy}!");    
                  result = enemy.TakeDamage(Attack, enemy.Defense, true);
                  Console.WriteLine($"You attacked {enemy.Name} for {result} damage!");
              } else
              {
                  // Normal damage calc if crit is not triggered
                  Console.WriteLine($"\nYou rolled ({attackRoll}) / Acc: {Accuracy}!");
                  result = enemy.TakeDamage(Attack, enemy.Defense, false);
                  Console.WriteLine($"You attacked {enemy.Name} for {result} damage!");
              }
      } else
      {
          Console.WriteLine($"\nYou rolled ({attackRoll}) / Acc: {Accuracy}!");
          Console.WriteLine("You attack but the enemy dodges!");
      }
    }

    // Player Defend
    public void PlayerDefend()
    {
        // Player defends, reducing incoming damage for the next turn
        Console.WriteLine("\nYou brace yourself for the next attack!");
        IsDefending = true;
    }

    // Attack Heavy Slash does Base Attack + 5 amd Attack + 10 damage lower accuracy
    public void HeavySlash(Enemy enemy)
    {

      if(CurrentMP != 0)
      {
          int attackRoll = GameSettings.RNG.Next(0, 100); 
          int critHit = GameSettings.RNG.Next(0, 100);
          int damage = GameSettings.RNG.Next(Attack + 5, (Attack + 10));
          int result;


          if(attackRoll < (Accuracy + abilities[2].AccuracyModifier))
          {
              if(critHit < CritChance)
              {
                  Console.WriteLine("\nYou landed a critical hit");
                  Console.WriteLine($"You rolled ({attackRoll}) / Acc: {Accuracy}");
                  result = enemy.TakeDamage(damage, enemy.Defense, true);
                  Console.WriteLine($"You hit the {enemy.Name} for {result} damage with a heavy blow!");
              } else
              {
                  Console.WriteLine($"\nYou rolled ({attackRoll}) / Acc: {Accuracy - 5}");
                  result = enemy.TakeDamage(damage, enemy.Defense, false);
                  Console.WriteLine($"You hit the {enemy.Name} for {result} damage with a heavy blow!");
              }
          } else
          {
              Console.WriteLine($"\nYou rolled ({attackRoll}) / Acc: {Accuracy}");
              Console.WriteLine("You swing with a Heavy Slash but the enemy dodges!");
          }

          CurrentMP -= abilities[2].MPCost;

      } else
      {
          Console.WriteLine("\nYou attempt to use Heavy Slash but overexert yourself due to not having enough MP!");
      }
    }

    // Player Multi-Strike
    public void MultiStrike(Enemy enemy)
    {
      // Double strike allows the player to perform 4 strikes at reduced accuracy 
      int count = 0;
      int totalDamage = 0;
      


      Console.WriteLine("\nYou used the skill Multi-Strike");

      if(CurrentMP >= 4)
      {
          for(int i = 0; i < 4; i++)
          {
              int attackRoll = GameSettings.RNG.Next(0, 100); 
              int critHit = GameSettings.RNG.Next(0, 100); 
              
              if(attackRoll < (Accuracy + abilities[3].AccuracyModifier))
              {
                  if(critHit < CritChance)
                  {
                      Console.WriteLine("You landed a critical hit!");
                      Console.WriteLine($"You landed strike {i + 1}");

                      int damage = enemy.TakeDamage(Attack, enemy.Defense, true);
                      totalDamage += damage;
                      count++;
                  } else
                  {
                      Console.WriteLine($"You landed strike {i + 1}");

                      int damage = enemy.TakeDamage(Attack, enemy.Defense, false);
                      totalDamage += damage;
                      count++;
                  }
              } else
              {
                  Console.WriteLine("Multi-Strike attack missed!");
              }
          }

          Console.WriteLine($"You landed {count} strike(s) for {totalDamage} damage");
          
          CurrentMP -= abilities[3].MPCost;
 
      } else
      {
          Console.WriteLine("\nYou attempt to use Multi-Strike but overexert yourself due to not having enough MP!");
      }
    }

    // Players poison strike ability 
    public void PoisonStrike(Enemy enemy)
    {
        // Added MP stat and Heavy Slash Ability to player, to give the player an option to deal more damage at the cost of some MP 
        CurrentMP -= abilities[4].MPCost;
        

        if(CurrentMP < 0)
        {
          CurrentMP = 0;
        }

        // Add calc to determine if an attack hits
        int attackRoll = GameSettings.RNG.Next(0, 100);

        if(attackRoll < (Accuracy + abilities[4].AccuracyModifier))
        {
          
          if(CurrentMP == 0)
          {
            Console.WriteLine($"\nYou don't have enough MP to use this ability. You drop your guard due to exhaustion!");
          } else 
          {
            int damage = Attack / 2;
 

            Console.WriteLine($"\nYou rolled {attackRoll} / Acc: {Accuracy - 10}");

            // Adding critical chance
            int critHit = GameSettings.RNG.Next(0, 100);
            int result;
            
            if(critHit < CritChance)
            {
              Console.WriteLine("You landed A critical Hit!");
              result = enemy.TakeDamage(damage, enemy.Defense,true);
              enemy.ApplyEffect(StatusEffectType.Poison, 5);
            } else
            {
              result = enemy.TakeDamage(damage, enemy.Defense, false);
              enemy.ApplyEffect(StatusEffectType.Poison, 5);
            }

            Console.WriteLine($"You attacked {enemy.Name} with Poison Strike for {result} damage!");   
          }

        } else 
        {
          Console.WriteLine($"\nYou rolled {attackRoll} / Acc: {Accuracy}");
          Console.WriteLine($"You attempt Poison Strike but the enemy dodges!");
        }
    }

}