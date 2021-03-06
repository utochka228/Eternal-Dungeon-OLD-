using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotThorn : MonoBehaviour
{
    Animator animator;
    [SerializeField] float visibleTime;
    float timer;
    void Start()
    {
        animator = GetComponent<Animator>();
        timer = visibleTime;
    }
    bool breakMethod;
    // Update is called once per frame
    void Update()
    {
        if(breakMethod)
            return;
        timer -= Time.deltaTime;
        if(timer <= 0f){
            animator.SetTrigger("Hide");
            breakMethod = true;
        }
    }
    public void DestroyThorn(){
        Destroy(gameObject);
    }
}
