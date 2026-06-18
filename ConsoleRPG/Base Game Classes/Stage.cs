using System;
using System.Collections.Generic;

public class Stage
{
    public bool BattleEncounter { get; set; }

    public Stage(bool battleEncounter)
    {
        BattleEncounter = battleEncounter;
    }
}