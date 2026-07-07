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




    // Take damage function to be used by both player and enemy------------------------------------------
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

        // if a character is defending damage is halved
        if(IsDefending)
        {
          damage /= 2;
        }

        // Reduce character HP by total damage amount
        CurrentHP-= damage;

        // Sets Curent hp to 0 if it goes below
        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        return damage;
    }


    // Functions for applying and processing status effects 
    public void ApplyEffect(StatusEffectType type, int duration)
    {
        // Addes status effect to character list
        ActiveEffects.Add(new StatusEffect(type, duration));
        
        // Will set is defending on for character to true, else display the current status effect applied
        if(type == StatusEffectType.Defend)
        {
          IsDefending = true;
        } else
        {
          Console.WriteLine($"\n{Name} is affected by {type}");
        }
        
    }


    // This function is for processing the status effects within the characters activaeffects list
    public void ProcessStatusEffect()
    {
        // Will loop through each element to process status effect
        for(int i = ActiveEffects.Count - 1; i >= 0; i--)
        {

            // Setting refernce for the Status effect type
            var effect = ActiveEffects[i];

            // Conditional to trigger actions taken for effect process based on the matching status effect type
            switch(effect.Type)
            {
                  // Actions for Poison status effect, once triggered it will decrease currentHP of the character until it wares off 
                  case StatusEffectType.Poison:
                    int poisonDamage = 2;
                    CurrentHP -= poisonDamage;
                    Console.WriteLine($"{Name} takes {poisonDamage} poison damage!");
                    break;
                  default:
                    break;
            }

            // Reduce the effct duration by 1
            effect.Duration --;

            // Action for Status effect when the duration hits 0, Remove the element from the list, Display that the status effect is no longer active 
            if(effect.Duration <= 0)
            {
                if(effect.Type != StatusEffectType.Defend)
                {
                  Console.WriteLine($"\n{effect.Type} has worn off");
                } else
                {
                  IsDefending = false;  
                  Console.WriteLine($"\n{Name} is no longer defending!");
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