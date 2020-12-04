using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Item
{
    private bool pointIsNotFound = true;
    public override void UseItem(Transform user)
    {
        //Vector2 telPos = GameMap.GM.RandomizePosionOnMap();

        //player.SetStopPoint(new Vector2(telPos.x, telPos.y));
        //player.disableInput = true;
        Destroy(gameObject);

    }
}

