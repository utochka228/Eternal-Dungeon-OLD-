using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPickAxeType", menuName = "CreateItem/PickAxe")]
public class PickAxe : Item
{
    [SerializeField] float power = 10;
    [SerializeField] float speed = 1f;
    [SerializeField] float distance = 1f;
    public override void UseItem(Transform user)
    {
        //Activate mining animation
        //Mining if block is selected by player
        PlayerAttackSystem attackSystem = user.GetComponent<PlayerAttackSystem>();
        attackSystem?.Attack(power, distance, "PickAxe");
    }
}
