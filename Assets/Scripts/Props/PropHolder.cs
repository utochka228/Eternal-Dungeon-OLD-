using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    void Interact();
    string GetInteractName();
}
public class PropHolder : MonoBehaviour, IDamageble, IInteractable
{
    [SerializeField] Prop propBase;
    Prop myProp;
    [SerializeField] SpriteRenderer sprite;

    private void Start() {
        if(propBase != null)
            SetMyProp(propBase);
    }

    public void SetMyProp(Prop prop){
        myProp = Instantiate(prop);
        sprite.sprite = myProp.sprite;
    }

    public void Interact()
    {
        myProp.UseProp();
    }

    public void TakeDamage(GameObject hitter, int damage, bool isCritical)
    {
        if(myProp.destroyable == false)
            return;

        myProp.Health -= damage;
        if(myProp.Health <= 0)
            Die(hitter);
    }

    public void Die(GameObject murderer)
    {
        //Do something

        Destroy(gameObject);
    }

    public string GetInteractName()
    {
        return myProp.propName;
    }
}
