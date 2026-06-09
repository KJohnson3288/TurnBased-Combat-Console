using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;


public static class Battle
{
    private static List<Func<Enemy>> enemyPool = new List<Func<Enemy>>()
    {
        () => new Enemy("Goblin", 6, 2, 2, 6, 60, 5, 15, 20),            // High Speed / Ave Accuracy 
        () => new Enemy("Hob Goblin", 8, 3, 2, 12, 40, 8, 25, 20),        // Higher Speed / More Strength / Less Accuracy
        () => new Enemy("Goblin Mage", 10, 1, 1, 6, 70, 3, 35, 20),      // Low Strength / Low Speed / Has Abilities 
        () => new Enemy("Goblin Orc", 12, 3, 3, 35, 4, 10, 50, 20),       // Higher Hp / Higher Strength / Lower Speed / Lower Accuracy      
        () => new Enemy("Green Goblin", 10, 4, 2, 8, 50, 10, 50, 20),     // Balanced / High Attack / Abilities
    };

    // Boss List
    private static List<Func<Enemy>> bossPool = new List<Func<Enemy>>()
    {
        () => new Enemy("Goblin Ogre", 25, 6, 6, 6, 30, 15, 75, 20),      // Tank Boss - Slow / Low Accuracy / High damage / Abilities 
        () => new Enemy("Goblin Champion", 20, 5, 5, 10, 60, 12, 100, 20)   // Strength Boss / High Attack / Defend / Abilities
    };


    // Function for random generating enemies
    private static Enemy GenerateEnemy()
    {
        return enemyPool[GameSettings.RNG.Next(enemyPool.Count)]();
    }

    // Boss Generation
    private static Enemy GenerateBoss()
    {
        return bossPool[GameSettings.RNG.Next(bossPool.Count)]();
    }

    public static bool Run(Player player, bool bossBattle)
    {
        // Base Varible to generate enemy
        Enemy enemy;

        if(bossBattle)
        {
          // Prompt the player about the encounter
          enemy = GenerateBoss();
        } else
        {
          // Prompt the player about the encounter
          enemy = GenerateEnemy();
        }


        Console.WriteLine($"A wild {enemy.Name} appears!");

        // Battle loop, will continue to run until either the player or the enemy is defeated
        while (player.IsAlive() && enemy.IsAlive())
        {

            // Running Check for status effect
            player.ProcessStatusEffect();
            enemy.ProcessStatusEffect();

            if(!player.IsAlive() || !enemy.IsAlive())
            {
                break;
            }


            // Display current HP of both the player and the enemy
            Console.WriteLine($"\n\nPlayer Lvl({player.Level}) HP: {player.CurrentHP}/{player.MaxHP} MP: {player.CurrentMP}/{player.MaxMP} | {enemy.Name} HP: {enemy.CurrentHP} / {enemy.MaxHP}\n");


            // Prompt the player for their action
            Console.WriteLine("Choose your action:");

            for(int i = 0; i < player.Abilities.Count; i++)
            {
                Console.WriteLine($"({i + 1}) {player.Abilities[i].Name}");
            }

            string input;

            // Adding conditional options to run simulation without player input
            if(GameSettings.SimulationMode)
            {
                // Random setting for simulation mode
                 input = GameSettings.RNG.Next(1, player.Abilities.Count + 1).ToString();
                 Console.WriteLine($"\nInput: {input}");
            } else
            {
                int choice;

                do
                {
                    input = Console.ReadLine() ?? "";
                    

                    if(!int.TryParse(input, out choice) || choice < 1 || choice > player.Abilities.Count)
                    {
                        Console.WriteLine("\nInvalid Input");
                        Console.WriteLine("Choose your action:");

                        for(int i = 0; i <player.Abilities.Count; i++)
                        {
                            Console.WriteLine($"({i + 1}) {player.Abilities[i]}");
                        }
                    }
                } while(!int.TryParse(input, out choice) || choice < 1 || choice > player.Abilities.Count);

            }

            // Setting conditional to add Speed stat control
            int playerSpeed = GameSettings.RNG.Next(1, player.Speed + 1);
            int enemySpeed = GameSettings.RNG.Next(1, enemy.Speed + 1);


            if(playerSpeed >= enemySpeed)
            {
                player.PlayerAttack(input, enemy);

                if(enemy.IsAlive())
                {
                    enemy.EnemyAttack(enemy, player);
                }

            } else
            {
                enemy.EnemyAttack(enemy, player);

                if(player.IsAlive())
                {
                    player.PlayerAttack(input, enemy);
                }

            }

        }




        // Check for end of battle conditions-------------------------------------------
        if (player.IsAlive())
        {
            Console.WriteLine($"\nYou have defeated {enemy.Name}!");

            // End of battle Summary and rewards
            player.Gold += enemy.GoldRewards;
            player.Exp += enemy.EXPRewards;

                        
            player.CheckLevelUp();

            
            Console.WriteLine($"\nYou received {enemy.GoldRewards} gold! Current Gold: {player.Gold}");
            Console.WriteLine($"You received {enemy.EXPRewards} exp! | Current Exp {player.Exp} / {player.ExpToLevel}");

            return true; // Player wins
        } else
        {
            Console.WriteLine("\nYou have been defeated...");
            return false; // Player loses
        }
    }
}