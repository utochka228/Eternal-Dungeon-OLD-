using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshPro textMesh;
    float disapearTimer;
    Color textColor;
    public static DamagePopup Create(Vector3 position, int damage){
        GameObject damagePopupObj = Instantiate(GameSession.instance.damagePopupPrefab, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupObj.GetComponent<DamagePopup>();
        damagePopup.Setup(damage);
        return damagePopup;
    }
    public void Setup(int damageCount){
        textMesh.text = damageCount.ToString();
        textColor = textMesh.color;
        disapearTimer = 1f;
    }
    private void Update() {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed)*Time.deltaTime;
        disapearTimer -= Time.deltaTime;
        if(disapearTimer <= 0){
            float disapearSpeed = 1f;
            textColor.a -= disapearSpeed * Time.deltaTime;
            textMesh.color = textColor; 
            if(textColor.a <= 0)
                Destroy(gameObject);
        }
    }
}
