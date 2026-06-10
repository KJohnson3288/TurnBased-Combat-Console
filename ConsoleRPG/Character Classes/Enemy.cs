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

        // Reward fields
        EXPRewards = expRewards;
        GoldRewards = goldReward;

        // List for abilities
        EnemyAbilities = enemyAbilities;

    }

    // List for Each Enemy Ability
    


    // Function for enemy attack
    public void EnemyAttack(Enemy enemy, Player player)
    {
            if (enemy.IsAlive())
            {
                // Calculate damage, if player is defending reduce damage by half
                int damage = Attack;
                int result;

                if (player.IsDefending)
                {
                    damage /= 2;
                }

                int attackRoll = GameSettings.RNG.Next(0, 100); 
                int critHit = GameSettings.RNG.Next(0, 100); 

                if(attackRoll < enemy.Accuracy)
                {
                    if(critHit < enemy.CritChance)
                    {
                        Console.WriteLine("\nThe enemy landed a critical hit!");
                        result = player.TakeDamage(damage, player.Defense, true);
                        Console.WriteLine($"The enemy rolled ({attackRoll}) / Acc: {enemy.Accuracy}");
                        Console.WriteLine($"The {enemy.Name} attacks you for {result} damage!");    
                    } else
                    {
                        result = player.TakeDamage(damage, player.Defense, false);
                        Console.WriteLine($"\nThe enemy rolled ({attackRoll}) / Acc: {enemy.Accuracy}");
                        Console.WriteLine($"The {enemy.Name} attacks you for {result} damage!"); 
                    }             
                } else
                {
                    Console.WriteLine($"\nThe enemy rolled ({attackRoll}) / Acc: {enemy.Accuracy}");
                    Console.WriteLine($"You dodged the enemy's attack!");
                }

            }

            // Reset defending status after the enemy's turn
            player.IsDefending = false;
    }
}