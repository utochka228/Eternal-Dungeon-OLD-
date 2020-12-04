using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStat : Stat
{
    public override void UpgradeStat()
    {
        PlayerInfo.PI.Health++;
    }
}
