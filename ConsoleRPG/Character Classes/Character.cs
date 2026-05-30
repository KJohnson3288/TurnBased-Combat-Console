public class Character
{
    // Shared properties for all charter classes
    public string? Name {get; set;} 
    public int MaxHP {get; set;}
    public int CurrentHP {get; set;}
    public int MaxMP {get; set;}
    public int CurrentMP {get; set;}
    public int Attack { get; set; }
    public int Defense {get; set;}
    public int Speed {get; set;}
    public int Accuracy {get; set;}
    public int CritChance {get; set;}

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

        CurrentHP-= damage;

        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        return damage;
    }

    // Functions for applying and processing status effects 
    public void ApplyEffect(string type, int duration)
    {
        ActiveEffects.Add(new StatusEffect(type, duration));
        Console.WriteLine($"{Name} is affected by {type}");
    }

    public void ProcessStatusEffect()
    {
        for(int i = ActiveEffects.Count - 1; i >= 0; i--)
        {

            var effect = ActiveEffects[i];

            switch(effect.Type)
            {
                case "poison":
                    int poisonDamage = 2;
                    CurrentHP -= poisonDamage;
                    Console.WriteLine($"{Name} takes {poisonDamage} poison damage!");
                    break;
                case "burn":
                    int burnDamage = 1;
                    CurrentHP -= burnDamage;
                    Console.WriteLine($"{Name} takes {burnDamage} poison damage!");
                    break;
                case "blind":
                    Console.WriteLine($"{Name} is blinded and may miss!");
                    break;
                default:
                    break;
            }

            effect.Duration --;

            if(effect.Duration <= 0)
            {
                Console.WriteLine($"{effect.Type} has worn off");
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