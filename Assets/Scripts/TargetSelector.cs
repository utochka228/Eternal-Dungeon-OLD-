using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetSelector : MonoBehaviour
{
    public static TargetSelector instance;
    [SerializeField] Camera myCamera;
    [SerializeField] Transform target;
    [SerializeField] RectTransform parent;
    [SerializeField] FixedJoystick joystick;
    [SerializeField] float speed = 1f;
    [SerializeField] float maxPlayerDistFromTarget = 1.5f;
    [SerializeField] float timeForUnsnapp = 1f;
    [SerializeField] bool targetSnapping = true;
    [SerializeField] LayerMask snapMask;
    bool targetSnapped;
    bool freezeTarget;

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    private void Awake() {
        instance = this;
    }

    Vector3 upperLeftScreen, upperRightScreen, lowerLeftScreen, lowerRightScreen;
    
    void Start()
    {
        joystick.OnPointerUpAction += SetPlayerTarget;
        joystick.OnPointerDownAction += NullTargetPosition;
        joystick.OnPointerDownAction += ActivateTarget;
        NullTargetPosition();

        upperLeftScreen = new Vector3(0, Screen.height, 0);
        upperRightScreen = new Vector3(Screen.width, Screen.height, 0);
        lowerLeftScreen = new Vector3(0, 0, 0);
        lowerRightScreen = new Vector3(Screen.width, 0, 0);
    }
    GameObject selectedObject;
    void Update()
    {
        if(joystick.Vertical != 0 || joystick.Horizontal != 0){

            if(freezeTarget == false){

                UpdateTargetPosition();

                Vector2 origin = new Vector2(target.position.x, target.position.y);
                Vector2 direction = Vector2.zero;
                int layerMask = snapMask.value; //~LayerMask.GetMask("Player");
                RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);
                if(hit.collider != null){
                    if(hit.transform.gameObject != selectedObject){
                        selectedObject = hit.transform.gameObject;
                        targetSnapped = false;
                    }

                    if(targetSnapping){
                        SnapTarget(hit.transform);
                    }

                    ShowSelectedInfo(hit.transform);
                } else{
                    PlayerController player = GameSession.instance.Player;
                    if(player.targetBlock != null)
                        player.targetBlock = null;
                        
                    selectedObject = null;
                    targetSnapped = false;
                }
            }
        }
    }

    IEnumerator WaitForUnsnapp(){
        freezeTarget = true;
        yield return new WaitForSeconds(timeForUnsnapp);
        freezeTarget = false;
    }

    void SnapTarget(Transform snapTarget){
        if(targetSnapped == false){
            target.position = snapTarget.position;
            StartCoroutine(WaitForUnsnapp());
            targetSnapped = true;
        }
    }

    void ShowSelectedInfo(Transform hitted){

    }

    void UpdateTargetPosition(){
        target.position += new Vector3(joystick.Horizontal, joystick.Vertical, 0f) * Time.deltaTime * speed;
            var upperLeft = myCamera.ScreenToWorldPoint(upperLeftScreen);
            var lowerLeft = myCamera.ScreenToWorldPoint(lowerLeftScreen);
            var lowerRight = myCamera.ScreenToWorldPoint(lowerRightScreen);
            target.position = new Vector3(Mathf.Clamp(target.position.x, lowerLeft.x, lowerRight.x),
            Mathf.Clamp(target.position.y, lowerLeft.y, upperLeft.y), target.position.z);
    }

    public void NullTargetPosition(){
        Vector3 point = myCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0f));
        target.position = new Vector3(point.x, point.y, target.position.z);
        target.gameObject.SetActive(false);
    }
    void ActivateTarget(){
        target.gameObject.SetActive(true);
        Debug.Log("ACtivated!");
    }

    void SetPlayerTarget(){
        if(selectedObject == null){
            target.gameObject.SetActive(false);
            return;
        }

        Block block = selectedObject.GetComponent<Block>();
        PlayerController player = GameSession.instance.Player;
        Vector2 playerPos = new Vector2(player.transform.position.x, 
        player.transform.position.y);
        Vector2 selectedPos = new Vector2(selectedObject.transform.position.x, 
        selectedObject.transform.position.y);
        float distance = Vector2.Distance(playerPos, selectedPos);
        if(block != null && distance <= maxPlayerDistFromTarget){
            //Set Player Target
            player.targetBlock = block;
            target.transform.position = selectedObject.transform.position;
            target.gameObject.SetActive(true);
        }
        else
            target.gameObject.SetActive(false);
        selectedObject = null;
    }
}
