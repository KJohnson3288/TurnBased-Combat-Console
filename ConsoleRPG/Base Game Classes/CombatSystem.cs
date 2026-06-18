using System;
using System.Collections.Generic;

public static class CombatSystem
{
  public static void ExecuteDamageAbility(Character attacker, Character defender, Ability damageAbility)
  {
      if(attacker.IsAlive())
      {

        for(int i = 0; i < damageAbility.HitCount; i++)
        {
          // Adding battle calc for hit success
          int attackRoll = GameSettings.RNG.Next(0, 100); 
          int critHit = GameSettings.RNG.Next(0, 100);
          int accuracy = attacker.Accuracy + damageAbility.AccuracyModifier;
          int damage = attacker.Attack * damageAbility.Modifier;
          int result;

          if(damageAbility.Effect == StatusEffectType.Defend)
          {
            attacker.ApplyEffect(damageAbility.Effect, damageAbility.EffectCount);
            break;
          }


          if(attackRoll < accuracy)
          {
              if(critHit < attacker.CritChance)
              {
                  // If crit is triggered we set the ceic param to true to properly calc damage
                  Console.WriteLine($"{attacker.Name} rolled ({attackRoll}) / Acc: {accuracy}!");
                  Console.WriteLine($"\n{attacker.Name} landed a critical hit!");    
                  result = defender.TakeDamage(damage, defender.Defense, true);
                  Console.WriteLine($"{attacker.Name} used ({damageAbility.Name}) attacked {defender.Name} for {result} damage!");
              } else
              {
                  // Normal damage calc if crit is not triggered
                  Console.WriteLine($"\n{attacker.Name} rolled ({attackRoll}) / Acc: {accuracy}!");
                  result = defender.TakeDamage(damage, defender.Defense, false);
                  Console.WriteLine($"{attacker.Name} used ({damageAbility.Name}) attacked {defender.Name} for {result} damage!");
              }
                
              if(damageAbility.Effect != StatusEffectType.None)
              {
                  int statusRoll = GameSettings.RNG.Next(0, 100);

                  if(statusRoll < damageAbility.EffectChance)
                  {
                    defender.ApplyEffect(damageAbility.Effect, damageAbility.EffectCount);
                  }
              }
          } else
          {
              Console.WriteLine($"\n{attacker.Name} rolled ({attackRoll}) / Acc: {accuracy}!");
              Console.WriteLine($"{attacker.Name} used ({damageAbility.Name}) attack but the {defender.Name} dodges!\n");
          }
        }
      }
  }

}