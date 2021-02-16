using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPickAxeType", menuName = "CreateItem/PickAxe")]
public class PickAxe : Weapon
{
    [SerializeField] int power = 10;
    [SerializeField] float distance = 1f;

    public override string GetItemType()
    {
        return this.GetType().Name;
    }

    public override void UseItem(Transform user)
    {
        //Activate mining animation
        //Mining if block is selected by player
        PlayerAttackSystem attackSystem = user.GetComponent<PlayerAttackSystem>();
        attackSystem?.Attack(power, distance, this);
    }
}
