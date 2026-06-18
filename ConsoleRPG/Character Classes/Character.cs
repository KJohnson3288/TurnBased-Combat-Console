using System;
using System.Collections.Generic;

public class Character
{
    // Shared properties for all charter classes
    public string Name {get; set;} 
    public int MaxHP {get; set;}
    public int CurrentHP {get; set;}
    public int MaxMP {get; set;}
    public int CurrentMP {get; set;}
    public int Attack { get; set; }
    public int Defense {get; set;}
    public int Speed {get; set;}
    public int Accuracy {get; set;}
    public int CritChance {get; set;}

    // Defending Trigger
    public bool IsDefending { get; set; }

    // Status effect Property
    public List<StatusEffect> ActiveEffects = new List<StatusEffect>();




    // Take damage function to be used by both player and enemy----------------------------------------------
    public int TakeDamage(int attack, int defense, bool crit)
    {
        int damage;


        // Will trigger damage calc result based on crit 
        if(crit == true)
        {
            damage = Math.Max(1, (attack - (defense / 2)) * 2);
        } else
        {
            damage = Math.Max(1, attack - (defense / 2));
        }

        if(IsDefending)
        {
          damage /= 2;
        }

        CurrentHP-= damage;

        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        return damage;
    }

    // Functions for applying and processing status effects 
    public void ApplyEffect(StatusEffectType type, int duration)
    {
        ActiveEffects.Add(new StatusEffect(type, duration));

        if(type == StatusEffectType.Defend)
        {
          Console.WriteLine($"\n{Name} Braces for the attack"); 
          IsDefending = true;
        } else
        {
          Console.WriteLine($"\n{Name} is affected by {type}");
        }
    }


    public void ProcessStatusEffect()
    {
        for(int i = ActiveEffects.Count - 1; i >= 0; i--)
        {

            var effect = ActiveEffects[i];

            switch(effect.Type)
            {
                  case StatusEffectType.Poison:
                    int poisonDamage = 2;
                    CurrentHP -= poisonDamage;
                    Console.WriteLine($"{Name} takes {poisonDamage} poison damage!");
                    break;
            }

            effect.Duration --;

            if(effect.Duration <= 0)
            {
                if(effect.Type != StatusEffectType.Defend)
                {
                  IsDefending = false;  
                  Console.WriteLine($"{effect.Type} has worn off");
                } else
                {
                  Console.WriteLine($"{Name} is no longer defending!");
                }
                
                ActiveEffects.RemoveAt(i);
            }
        }
    }



    // Setting bool for checking if character is alive-------------------------------------------------------
    public bool IsAlive()
    {
        return CurrentHP > 0;
    }



}