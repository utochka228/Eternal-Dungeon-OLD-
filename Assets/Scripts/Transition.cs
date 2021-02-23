using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI levelDesc;
    [SerializeField] Animator animator;
    public void Create(int caveLevel, string caveDesc){
        Setup(caveLevel, caveDesc);
        animator.SetTrigger("Start");
        StartCoroutine(WaitTransition());
    }
    public void Setup(int caveLevel, string caveDesc){
        levelName.text = "Level " + caveLevel;
        levelDesc.text = caveDesc;
    }

    IEnumerator WaitTransition(){
        yield return new WaitForSeconds(3f);
        animator.SetTrigger("End");
        yield break;
    }
    public void SetAstar(){
        GameMap.GM.SetAstarGrid();
    }
    
    void Update()
    {
        
    }
}
