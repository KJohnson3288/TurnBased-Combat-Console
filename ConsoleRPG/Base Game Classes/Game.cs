using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;

public class Game
{
    private Player player;
    private List<Stage> stages;

    public Game()
    {
        // Create player instance on game start
        player = new Player("Hero", 20, 10, 3, 3, 10, 70, 10);
        stages = new List<Stage>();


        // Will Create stages and randomly assign encounters
        for(int i = 0; i < 50; i++)
        {
            bool battleEncounter = GameSettings.RNG.Next(0, 2) == 0;
            stages.Add(new Stage(battleEncounter));
        }

    }


    public void Start()
    {
        // Prompt informs player the game has started
        Console.WriteLine("**Welcome to the Dungeon");

        // Create game loop for stage progression
        for(int i = 0; i < stages.Count; i++)
        {
            Stage current = stages[i];

            // Prompt player to choose a path
            Console.WriteLine($"\nStage {i + 1}: Choose a path: Left or Right?");

            // Read player input to determine path
            string choice;

            if(GameSettings.SimulationMode)
            {
                choice = GameSettings.RNG.Next(0, 2) == 0 ? "left" : "right";
                Console.WriteLine($"[AUTO] Choosing {choice}");
            } else
            {

                do
                {
                    choice = (Console.ReadLine() ?? string.Empty).ToLower();
                    
                    if(choice != "left" && choice != "right")
                    {
                        Console.WriteLine("\nInvalid input");

                    Console.WriteLine($"\nStage {i + 1}: Choose a path: Left or Right?");
                    }


                } while(choice != "left" && choice != "right");

                Console.WriteLine($"\nYou Choose {choice}");
            }

            // Confirm if the chosen path will trigger a battle encounter / Added bool to use to check if a boss battle occurs
            bool battleChosen = false;
            bool bossBattle = false;

            //  Sets condition to trigger boss battle 
            if((i + 1) % 10 == 0)
            {
              Console.WriteLine("\n----------Boss Battle!-------------");

              battleChosen = true; 
              bossBattle = true;

            } else 
            {

              battleChosen = choice == "left" && current.BattleEncounter || choice == "right" && current.BattleEncounter;

            }
            
          

            // Determine path action based on the choice
            if(battleChosen)
            {
                // Enemy will now be created when battle has chosen
                List<Enemy> enemies = new List<Enemy>();
                

                // This checks to see if the player continues after the battle
                bool won = Battle.Run(player, bossBattle);
                if(!won)
                {
                    return; // Player had lost the battle - Game over
                } 
            } else
            {
                // Player chose healing path 
                Console.WriteLine("You take a moment to rest and heal 3 HP and MP.");
                player.Heal(3);
            }
        }

        // If the player completes all stages
        Console.WriteLine($"\nCongratulations! You survived all {stages.Count} stages!");

    }
}