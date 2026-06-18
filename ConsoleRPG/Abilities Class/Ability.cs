using System;
using System.Collections.Generic;


public class Ability
{
  public string Name {get; set;}
  public int HitCount {get; set;}
  public int Modifier {get; set;}
  public int MPCost {get; set;}
  public int Cooldown {get; set;}
  public int CurrentCooldown {get; set;}
  public string Description {get; set;}
  public StatusEffectType Effect {get; set;}
  public int EffectChance {get; set;}
  public int EffectCount {get; set;}
  public int AccuracyModifier {get; set;}

  public Ability(string name, int hitCount, int modifier, int mpCost, int cooldown, string description, StatusEffectType effect, int effectChance, int effectCount, int accuracyModifier)
  {
    Name = name;
    HitCount = hitCount;
    Modifier = modifier;
    MPCost = mpCost;
    Cooldown = cooldown;
    CurrentCooldown = 0;
    Description = description;
    Effect = effect;
    EffectChance = effectChance;
    EffectCount = effectCount;
    AccuracyModifier = accuracyModifier;
  }

  // Functions for cooldown process for enemy abilities
  public bool IsReady()
  {
    return CurrentCooldown <= 0;
  }

  public void StartCooldown()
  {
    CurrentCooldown = Cooldown;
  }

  public void ReduceCooldown()
  {
    if(CurrentCooldown > 0)
    {
      CurrentCooldown--;
    }
  }
}