using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;


public static class Battle
{
    //  This Class handles the battle loop process, Enemy generaiion, and end of battle process

    // Base enemy pool were the enemy will be randomly selected and pooled from this battle 
    private static List<Func<Enemy>> enemyPool = new List<Func<Enemy>>()
    {
        () => new Enemy("Goblin", 10, 3, 2, 6, 60, 5, 25, 20, EnemyAbilities.GoblinAbilities),            // High Speed / Ave Accuracy 
        () => new Enemy("Hob Goblin", 15, 5, 3, 12, 40, 8, 50, 20, EnemyAbilities.HobGoblinAbilities)       // Higher Speed / More Strength / Less Accuracy
    };

    // Boss pool for the boss battles that occur 
    private static List<Func<Enemy>> bossPool = new List<Func<Enemy>>()
    {
        () => new Enemy("Big Goblin", 20, 8, 5, 10, 60, 12, 100, 20, EnemyAbilities.GoblinChampionAbilities)   // Strength Boss / High Attack / Defend / Abilities
    };


    // Function for random generating enemies
    private static Enemy GenerateEnemy()
    {
        return enemyPool[GameSettings.RNG.Next(enemyPool.Count)]();
    }

    // Function for random Boss Generation
    private static Enemy GenerateBoss()
    {
        return bossPool[GameSettings.RNG.Next(bossPool.Count)]();
    }


// -------------------- PREBATTLE Start ---------------------------------
// This stage initiates the battle and Generates the enemy, The battle intro 

    public static bool Run(Player player, bool bossBattle)
    {
        // Base Varible reference for enemy
        Enemy enemy;

        if(bossBattle)
        {
          enemy = GenerateBoss();
        } else
        {

          enemy = GenerateEnemy();
        }

        // Inform player of enemy, this will be how the player draws thier initial strategy going into the battle if they have faced the enemy before 
        Console.WriteLine($"A wild {enemy.Name} appears!\n");
        
// -------------------- PREBATTLE END ---------------------------------



// ------------------------ START TURN ---------------------------------
// This stage is the begining of the Turn - This stage Checks the battle condition to see if it will continue, process any status effects, reduce the cooldown of enemy abilities, and display the current HP/MP values 

        // Battle loop, will continue to run until either the player or the enemy is defeated
        while (player.IsAlive() && enemy.IsAlive())
        {

            // Running Check for status effect, and processing the effect action
            player.ProcessStatusEffect();
            enemy.ProcessStatusEffect();

            Console.WriteLine("\n---------------------------------");

            // Process and display current enemy cooldowns
            foreach(Ability ability in enemy.EnemyAbilities)
            {
                ability.ReduceCooldown();

                Console.WriteLine($"{ability.Name} Cooldown: {ability.CurrentCooldown}");
            }


            if(!player.IsAlive() || !enemy.IsAlive())
            {
                break;
            }


            // Display current HP of both the player and the enemy
            Console.WriteLine($"\n\nPlayer Lvl({player.Level}) HP: {player.CurrentHP}/{player.MaxHP} MP: {player.CurrentMP}/{player.MaxMP} | {enemy.Name} HP: {enemy.CurrentHP} / {enemy.MaxHP}\n");

// ------------------------ START TURN ---------------------------------



// ------------------------ CHOICE STAGE -------------------------------
// This stage will store the choices made from the player and Enemy

            // Prompt the player for their action
            Console.WriteLine("Choose your action:");

            // Displays the current abilities that the player has
            for(int i = 0; i < player.abilities.Count; i++)
            {
                Console.WriteLine($"({i + 1}) {player.abilities[i].Name}");
            }

            string input;

            // Adding conditional options to run simulation without player input
            if(GameSettings.SimulationMode)
            {
                // Random setting for simulation mode
                 input = GameSettings.RNG.Next(1, player.abilities.Count + 1).ToString();
                 Console.WriteLine($"\nInput: {input}");
            } else
            {
                int choice;

                // The console will repeat this prompt until a choice is made by the player, invalid inputs will cause the sonsole to repost the prompt 
                do
                {
                    input = Console.ReadLine() ?? "";
                    

                    if(!int.TryParse(input, out choice) || choice < 1 || choice > player.abilities.Count)
                    {
                        Console.WriteLine("\nInvalid Input");
                        Console.WriteLine("Choose your action:");

                        for(int i = 0; i <player.abilities.Count; i++)
                        {
                            Console.WriteLine($"({i + 1}) {player.abilities[i]}");
                        }
                    }
                } while(!int.TryParse(input, out choice) || choice < 1 || choice > player.abilities.Count);

            }
// ------------------------ CHOICE STAGE -------------------------------



// ------------------------ PREACTION STAGE ------------------------------
// This stage will perform and Attribute affecting abilities that will need to be considered in the Action stage


            // Storing the character choices
            Ability playerChoice = player.Choice(input); 
            Ability enemyChoice = enemy.ChoooseEnemyAbility();

            // Setting conditional to add Speed stat control
            int playerSpeed = GameSettings.RNG.Next(1, player.Speed + 1);
            int enemySpeed = GameSettings.RNG.Next(1, enemy.Speed + 1);

            // Set is defending is chosen by character 
            if(playerChoice.Effect == StatusEffectType.Defend)
            {
              Console.WriteLine($"{player.Name} is bracing for the attack!");
              player.ApplyEffect(playerChoice.Effect, playerChoice.EffectCount);
            }

            if(enemyChoice.Effect == StatusEffectType.Defend)
            {
              Console.WriteLine($"{enemy.Name} is bracing for the attack!");
              enemy.ApplyEffect(enemyChoice.Effect, enemyChoice.EffectCount);
            }
           

// ------------------------ PREACTION STAGE ------------------------------



// ------------------------ ACTION STAGE ------------------------------
// Battle calculation and abilities are performed here 

            if(playerSpeed >= enemySpeed)
            {
                player.PlayerAttack(enemy, playerChoice);

                if(enemy.IsAlive())
                {
                    enemy.EnemyAttack(player, enemyChoice);
                }

            } else
            {
                enemy.EnemyAttack(player, enemyChoice);

                if(player.IsAlive())
                {
                    player.PlayerAttack(enemy, playerChoice);
                }

            }
        }
// ------------------------ PREACTION STAGE ------------------------------



// ------------------------ ENDTURN STAGE ------------------------------
// Checks the battle condition to determine whether to break the loop or continue to the next turn

        // Check for end of battle conditions---------------------
        if (player.IsAlive())
        {
            Console.WriteLine($"\n<<<<<<<<<< You have defeated {enemy.Name}! >>>>>>>>>>");

            player.ActiveEffects.Clear();

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
        
// ------------------------ ENDTURN STAGE ------------------------------



    }
}