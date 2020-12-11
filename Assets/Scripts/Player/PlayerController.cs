using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    #region PublicVariables

    public new string name;

    public float speed = 10f; //Скорость передвижения игрока

    public int bombCount = 0;

    public Text deathTimerText;

    public PlayerController enemy;

    public event Action OnChangedPlayerPosition;
    public event Action OnStartOfMoving;

    public MeshRenderer meshRenderer;

    public Vector2 currentPosition;
    public Vector2 oldPosition;

    #endregion

    #region PrivateVariables

    [Header("Player Attack System")]
    [SerializeField] PlayerAttackSystem attackSystem;

    [Header("My Player Stats(source)")]
    [SerializeField]
    PlayerStats myPlayerStats;

    private Vector2 stopPoint; //Позиция к которой стремится игрок

    [Header("Player Skins")]
    [SerializeField]
    private GameObject[] skins;

    FixedJoystick joystick;
    #endregion

    void UpdatePlayerSkin()
    {
        GameObject skin = skins[SkinChanger.skinChanger.currentActiveSkinIndex];
        skin.SetActive(true);
    }


    void PlayerPositionHasChanged()
    {
        if (OnChangedPlayerPosition != null)
            OnChangedPlayerPosition();
    }

    public void Attack()
    {
        attackSystem.Attack();
    }

    void Start()
    {
        stopPoint = new Vector2(transform.position.x, transform.position.y);
        oldPosition = currentPosition;

        joystick = PlayerUI.instance.joystick;
        PlayerUI.instance.interactButton.onClick.AddListener(OnInteractButton);

        UpdatePlayerSkin();
    }

    void Update()
    {
        int xPos = Mathf.RoundToInt(transform.position.x);
        int yPos = Mathf.RoundToInt(transform.position.y);
        currentPosition = new Vector2(xPos, yPos);

        if (currentPosition != oldPosition)
        {
            PlayerPositionHasChanged();
            
            oldPosition = currentPosition;
        }
    }
    [SerializeField] float viewOfDistance = 2f;
    public void SetStopPoint(Vector2 position)
    {
        stopPoint = position;
    }
    void FixedUpdate()
    {
        MovePlayer();
    }
    
    [SerializeField] Rigidbody2D rb;
    void MovePlayer()
    {
        //speed = movingSpeedCurve.Evaluate(currentTime);
        Vector2 movement = new Vector2(speed * joystick.Direction.x, speed * joystick.Direction.y);
        movement *= Time.deltaTime;

        rb.MovePosition(rb.position + movement);
        
    }


    Queue<IInteractable> interactItems = new Queue<IInteractable>();
    private IInteractable InteractItem {
        get{

            IInteractable item = null;
            try {
                item = interactItems.Dequeue();
                return item;

            } 
            catch{
                Debug.Log("InteractItems is empty.");
                return null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        IInteractable item = other.GetComponent<IInteractable>();
        if(item != null){
            interactItems.Enqueue(item);
        }
        if(interactItems.Count > 0 && Inventory.instance.itHasFreeSpace())
            PlayerUI.instance.interactButton.interactable = true;

    }
    private void OnTriggerExit2D(Collider2D other) {
        IInteractable item = other.GetComponent<IInteractable>();
        if(item != null){
            IInteractable[] items = interactItems.ToArray();
            interactItems.Clear();
            foreach (var it in items)
            {
                if(it != item)
                    interactItems.Enqueue(it);
            }
            if(interactItems.Count <= 0)
                PlayerUI.instance.interactButton.interactable = false;
        }
    }
    //Method for interact Button
    public void OnInteractButton(){
        IInteractable item = InteractItem;
        if(item != null && Inventory.instance.itHasFreeSpace()){
            item.Interact();
            if(interactItems.Count <= 0 || !Inventory.instance.itHasFreeSpace())
                PlayerUI.instance.interactButton.interactable = false;
        }
        else
            PlayerUI.instance.interactButton.interactable = false;
    }
}
