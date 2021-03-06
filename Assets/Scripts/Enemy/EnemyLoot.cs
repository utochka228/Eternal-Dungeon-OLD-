using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyLoot", menuName = "EnemyLoot/DropLoot")]
public class EnemyLoot : ScriptableObject
{
    public Item[] possibleDropLoot;
}
