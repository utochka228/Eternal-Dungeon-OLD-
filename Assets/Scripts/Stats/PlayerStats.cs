using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    public PlayerController myController;
    UIPlayerUpdater playerUpdater;
    public override float Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value >= 0)
            {
                health = value;
                playerUpdater.healthText.text = health.ToString();
            }
        }
    }
    void Start()
    {
        //Variables init
        health = PlayerInfo.PI.Health;


        //Other init
        myController = GetComponent<PlayerController>();
        playerUpdater = GetComponent<UIPlayerUpdater>();
    }

    protected override void Dying(GameObject murderer)
    {
        Debug.Log(myController.transform.name + " was killed by " + murderer.name);
        GameTypeBase.instance.CallPlayerDieEvent();
        Destroy(myController.gameObject);
    }

    protected override void TakingDamage(GameObject hitter)
    {
        health--;
            if (health == 0)
                Die(hitter);
    }
    
}
