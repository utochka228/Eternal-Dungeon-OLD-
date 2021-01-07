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

    public Vector2 currentPosition;
    public Vector2 oldPosition;
    public Block targetBlock;

    #endregion

    #region PrivateVariables

    [Header("My Player Stats(source)")]
    [SerializeField]
    PlayerStats myPlayerStats;

    [Header("My Player AttackSystem")]
    [SerializeField]
    PlayerAttackSystem attackSystem;

    [SerializeField] SpriteRenderer playerSpriteRend;
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

    void Start()
    {
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

        if(targetBlock != null){
            float distance = Vector3.Distance(transform.position, targetBlock.transform.position);
            if(distance >= 1.5f){
                TargetSelector.instance.NullTargetPosition();
                targetBlock = null;
            }
        }
    }
    [SerializeField] float viewOfDistance = 2f;
    void FixedUpdate()
    {
        MovePlayer();
    }
    
    [SerializeField] Rigidbody2D rb;
    void MovePlayer()
    {
        float xDir = joystick.Direction.normalized.x;
        Vector2 movement = new Vector2(speed * joystick.Direction.x, speed * joystick.Direction.y);
        movement *= Time.deltaTime;
        rb.MovePosition(rb.position + movement);
        FlipCharacter();
        FlipHandItem();
    }

    void FlipCharacter(){
        if(joystick.Direction.normalized.x == 0)
            return;
        bool oldFlip = playerSpriteRend.flipX;
        bool flipCharacterX = joystick.Direction.normalized.x > 0;

        playerSpriteRend.flipX = flipCharacterX;
        if(oldFlip != flipCharacterX && !Inventory.instance.HandsEmpty())
            Inventory.instance.FlipXHandItem();
    }
    public Vector2 attackPointPos;
    void FlipHandItem(){
        if(Inventory.instance.HandsEmpty())
            return;
        float xDir = joystick.Direction.normalized.x;
        float yDir = joystick.Direction.normalized.y;

        if(xDir == 0 && yDir == 0)
            return;

        if(Mathf.Abs(xDir) > Mathf.Abs(yDir))
            attackPointPos = new Vector2(xDir, 0f);
        else
            attackPointPos = new Vector2(0f, yDir);
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
