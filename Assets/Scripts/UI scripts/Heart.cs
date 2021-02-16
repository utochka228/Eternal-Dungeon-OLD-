using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] Image heartFill;
    
    public void SetFillLevel(float level){
        heartFill.fillAmount = level;
    }
}
