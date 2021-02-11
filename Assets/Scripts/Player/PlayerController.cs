using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] PlayerStats myPlayerStats;
    [Header("My Player AttackSystem")]
    [SerializeField] PlayerAttackSystem attackSystem;
    [SerializeField] SpriteRenderer playerSpriteRend;
    [Header("Player Skins")]
    [SerializeField] private GameObject[] skins;
    FixedJoystick joystick;
    Animator playerAnimator;
    #endregion
    
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        oldPosition = currentPosition;
        interactText = PlayerUI.instance.interactName;
        joystick = PlayerUI.instance.joystick;
        PlayerUI.instance.interactButton.onClick.AddListener(OnInteractButton);
    }

    void Update()
    {
        int xPos = Mathf.RoundToInt(transform.position.x);
        int yPos = Mathf.RoundToInt(transform.position.y);
        currentPosition = new Vector2(xPos, yPos);

        if (currentPosition != oldPosition)
        {
            OnChangedPlayerPosition?.Invoke();
            
            oldPosition = currentPosition;
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
        //FlipCharacter();
        //FlipHandItem();
        float X = Mathf.Abs(joystick.Direction.normalized.x);
        float Y = Mathf.Abs(joystick.Direction.normalized.y);
        playerAnimator.SetFloat("X", X);
        playerAnimator.SetFloat("Y", Y);
        if(Input.GetKeyDown(KeyCode.Space))
            playerAnimator.SetTrigger("Attack_Swing");
    }

    void FlipCharacter(){
        
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
    TextMeshProUGUI interactText;
    private void OnTriggerEnter2D(Collider2D other) {
        IInteractable item = other.GetComponent<IInteractable>();
        if(item != null){
            interactItems.Enqueue(item);
            interactText.text = item.GetInteractName();
            interactText.gameObject.SetActive(true);
        }
        Button interactButton = PlayerUI.instance.interactButton;
        if(interactItems.Count > 0 && Inventory.instance.itHasFreeSpace())
            interactButton.interactable = true;
        
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
            if(interactItems.Count <= 0){
                PlayerUI.instance.interactButton.interactable = false;
                interactText.gameObject.SetActive(false);
            }else
                interactText.text = interactItems.Peek().GetInteractName();
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
