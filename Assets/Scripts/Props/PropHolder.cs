using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    void Interact();
}
public class PropHolder : MonoBehaviour, IDamageble, IInteractable
{
    [SerializeField] Prop myProp;
    

    public void SetMyProp(Prop prop){
        myProp = prop;
    }

    public void Interact()
    {
        //myProp.UseProp();
    }

    public void TakeDamage(GameObject hitter, float damage)
    {
        myProp.Health -= damage;
        if(myProp.Health <= 0)
            Die(hitter);
    }

    public void Die(GameObject murderer)
    {
        //Do something

        Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
