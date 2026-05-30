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

    // Property for triggering status effect
    public bool TriggerStatusEffect {get; set;}

    // Count property for status effect 
    public int PoisonCount = 3; 



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


    // This will hold the status effects when triggered to be activated on the player
    public void StatusEffect()
    {
        if(TriggerStatusEffect == true)
        {
        // Setting damage and count to occur per turn 
        int poisonDamage = 1;

        // Apply ststus effect 
        CurrentHP-= poisonDamage;
        Console.WriteLine("Apply status effect to player");

        // Reduce count by 1 per turn
        PoisonCount--;
        Console.WriteLine("Reduced poison Count!");

        if(PoisonCount <= 0)
        {
            TriggerStatusEffect = false;
            Console.WriteLine("The poison has wore off");
        }

        if(CurrentHP< 0)
        {
            CurrentHP= 0; 
        }

        }
    }

  

    // End of battle rewards and level uo check
    public void CheckLevelUp()
    {
        if(Exp>= ExpToLevel)
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
    public List<string> Actions = new List<string>
    {
        "Attack",
        "Defend",
        "Heavy Slash",
        "Double Strike",
        "Poison Strike"
    };

    // Player Function to attack---------------------------------------------------------------------------------------------
    public void PlayerAttack(string input, Enemy enemy, Player player)
    {
        // Take in player input and run action
            switch(input)
            {
                case "1":
                {
                    // Adding battle calc for hit success
                    int attackRoll = GameSettings.RNG.Next(0, 100); 
                    int critHit = GameSettings.RNG.Next(0, 100);
                    int result;

                    if(attackRoll < player.Accuracy)
                    {
                        if(critHit < player.CritChance)
                            {
                                // If crit is triggered we set the ceic param to true to properly calc damage
                                Console.WriteLine("\nYou landed a critical hit!");
                                Console.WriteLine($"You rolled ({attackRoll}) / Acc: {player.Accuracy}!");    
                                result = enemy.TakeDamage(player.Attack, enemy.Defense, true);
                                Console.WriteLine($"You attacked {enemy.Name} for {result} damage!");
                            } else
                            {
                                // Normal damage calc if crit is not triggered
                                Console.WriteLine($"\nYou rolled ({attackRoll}) / Acc: {player.Accuracy}!");
                                result = enemy.TakeDamage(player.Attack, enemy.Defense, false);
                                Console.WriteLine($"You attacked {enemy.Name} for {result} damage!");
                            } 
                    } else
                    {
                        Console.WriteLine($"\nYou rolled ({attackRoll}) / Acc: {player.Accuracy}!");
                        Console.WriteLine("You attack but the enemy dodges!");
                    }

                }
                break;
                case"2":
                {
                    // Player defends, reducing incoming damage for the next turn
                    Console.WriteLine("\nYou brace yourself for the next attack!");
                    player.IsDefending = true;
                } 
                break;
                case "3":
                {
                    if(player.CurrentMP != 0)
                    {
                        int attackRoll = GameSettings.RNG.Next(0, 100); 
                        int critHit = GameSettings.RNG.Next(0, 100);
                        int result;


                        if(attackRoll < player.Accuracy)
                        {
                           if(critHit < player.CritChance)
                            {
                                Console.WriteLine("\nYou landed a critical hit");
                                Console.WriteLine($"You rolled ({attackRoll}) / Acc: {player.Accuracy - 5}");
                                result = enemy.TakeDamage(player.HeavySlash(), enemy.Defense, true);
                                Console.WriteLine($"You hit the {enemy.Name} for {result} damage with a heavy blow!");
                            } else
                            {
                                Console.WriteLine($"\nYou rolled ({attackRoll}) / Acc: {player.Accuracy - 5}");
                                result = enemy.TakeDamage(player.HeavySlash(), enemy.Defense, false);
                                Console.WriteLine($"You hit the {enemy.Name} for {result} damage with a heavy blow!");
                            }
                        } else
                        {
                            Console.WriteLine($"\nYou rolled ({attackRoll}) / Acc: {player.Accuracy}");
                            Console.WriteLine("You swing with a Heavy Slash but the enemy dodges!");
                        }

                        player.CurrentMP -= 2;

                    } else
                    {
                        Console.WriteLine("\nYou attempt to use Heavy Slash but overexert yourself due to not having enough MP!");
                    }
                }
                break;
                case "4":
                {
                    // Double strike allows the player to perform 4 strikes at reduced accuracy 
                    int count = 0;
                    int totalDamage = 0;


                    Console.WriteLine("\nYou used the skill double strike");

                    if(player.CurrentMP >= 4)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            int attackRoll = GameSettings.RNG.Next(0, 100); 
                            int critHit = GameSettings.RNG.Next(0, 100); 
                            
                            if(attackRoll < (player.Accuracy - 20))
                            {
                                if(critHit < player.CritChance)
                                {
                                    Console.WriteLine("You landed a critical hit!");
                                    Console.WriteLine($"You landed strike {i + 1}");

                                    int damage = enemy.TakeDamage(player.Attack, enemy.Defense, true);
                                    totalDamage += damage;
                                    count++;
                                } else
                                {
                                    Console.WriteLine($"You landed strike {i + 1}");

                                    int damage = enemy.TakeDamage(player.Attack, enemy.Defense, false);
                                    totalDamage += damage;
                                    count++;
                                }
                            } else
                            {
                                Console.WriteLine("Double Strike attack missed!");
                            }
                        }

                        Console.WriteLine($"You landed {count} strike(s) for {totalDamage} damage");
                        
                        player.CurrentMP -= 4;

                    } else
                    {
                        Console.WriteLine("\nYou attempt to use Double Strike but overexert yourself due to not having enough MP!");
                    }
                }
                break;
                case "5":
      {
        // Added MP stat and Heavy Slash Ability to player, to give the player an option to deal more damage at the cost of some MP 
        CurrentMP -= 2;

        if(CurrentMP < 0)
        {
          CurrentMP = 0;
        }

        // Add calc to determine if an attack hits
        int attackRoll = GameSettings.RNG.Next(0, 100);

        if((Accuracy - 10) > attackRoll)
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
              PoisonStrike(enemy);
              result = enemy.TakeDamage(damage, enemy.Defense,true);
            } else
            {
              result = enemy.TakeDamage(damage, enemy.Defense, false);
              PoisonStrike(enemy);
            }

            Console.WriteLine($"You attacked {enemy.Name} with Poison Strike for {result} damage!");   
          }

        } else 
        {
          Console.WriteLine($"\nYou rolled {attackRoll} / Acc: {Accuracy}");
          Console.WriteLine($"You attempt Poison Strike but the enemy dodges!");
        }
      }
      break;

            }
    }

    // Player Abilities-------------------------------------------------------------------------------------------------

    // Players poison strike ability 
    public void PoisonStrike(Enemy enemy)
    {
        // Set trigger to enable poison effect 
        enemy.TakeDamage(Attack, enemy.Defense, false);
        enemy.ApplyEffect("poison", 5);

        Console.WriteLine("Apply poison effect");
        Console.WriteLine($"There is {ActiveEffects.Count}");
    }

    // Heavy Slash does 5-10 damage lower accuracy
    public int HeavySlash()
    {
        return GameSettings.RNG.Next(5, 10);
    }

}