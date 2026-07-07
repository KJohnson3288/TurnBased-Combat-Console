using System;
using System.Collections.Generic;

public class StatusEffect
{
    public StatusEffectType Type {get; set;}
    public int Duration {get; set;}

    // FOr each status effect we will use the type as the condition to trigger the effects, and  the duration will be set or reset based on the type that is applied 

    public StatusEffect(StatusEffectType type, int duration)
    {
        Type = type;
        Duration = duration;
    }
}