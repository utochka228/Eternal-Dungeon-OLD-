using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshPro textMesh;
    float disapearTimer;
    Color textColor;
    Vector3 moveVector;
    public static DamagePopup Create(Vector3 position, int damage, bool isCritical){
        GameObject damagePopupObj = Instantiate(GameSession.instance.damagePopupPrefab, position, Quaternion.identity);
        float angle = Random.Range(-5f, 5f);
        damagePopupObj.transform.eulerAngles = new Vector3(0, 0, angle);
        DamagePopup damagePopup = damagePopupObj.GetComponent<DamagePopup>();
        Vector3 dirToMe =  position - GameSession.instance.Player.transform.position;
        damagePopup.moveVector = dirToMe.normalized;
        damagePopup.Setup(damage, isCritical);
        return damagePopup;
    }
    public void Setup(int damageCount, bool isCrititicalHit){
        textMesh.text = damageCount.ToString();
        textColor = textMesh.color; 
        if(isCrititicalHit){
            textMesh.fontSize = 8;
            textColor = Color.red;
            Debug.Log("CRIT");
        }else{
            textMesh.fontSize = 5;
            textColor = Color.yellow;
            Debug.Log("Not CRIT");
        }
        textMesh.color = textColor;
        disapearTimer = DISAPEAR_TIMER_MAX;
    }
    const float DISAPEAR_TIMER_MAX = 1f; 
    [SerializeField] float increaseScaleAmount = 1f;
    [SerializeField] float decreaseScaleAmount = 1f;
    [SerializeField] float moveYSpeed = 1f;
    [SerializeField] float disapearSpeed = 1f;
    private void Update() {
        transform.position += moveVector*moveYSpeed*Time.deltaTime;
        moveVector -= moveVector*8f*Time.deltaTime;
        if(disapearTimer > DISAPEAR_TIMER_MAX * 0.5f){
            //First half of the popup lifetime
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }else{
            //Second half
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        disapearTimer -= Time.deltaTime;
        if(disapearTimer <= 0){
            textColor.a -= disapearSpeed * Time.deltaTime;
            textMesh.color = textColor; 
            if(textColor.a <= 0)
                Destroy(gameObject);
        }
    }
}
