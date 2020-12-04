using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    #region PublicVariables

    public new string name;

    [SerializeField]
    AnimationCurve movingSpeedCurve;
    public float speed = 10f; //Скорость передвижения игрока

    public int bombCount = 0;

    public Text deathTimerText;

    public PlayerController enemy;

    public event Action OnChangedPlayerPosition;
    public event Action OnStartOfMoving;

    public MeshRenderer meshRenderer;

    public Vector2 currentPosition;
    public Vector2 oldPosition;

    public FieldOfView fov;
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
        totalTime = movingSpeedCurve[movingSpeedCurve.keys.Length - 1].time;

        stopPoint = new Vector2(transform.position.x, transform.position.y);
        oldPosition = currentPosition;
        GameObject _fov = GameObject.FindGameObjectWithTag("FOV");
        fov = _fov.GetComponent<FieldOfView>();

        joystick = PlayerUI.instance.joystick;
        PlayerUI.instance.attackButton.onClick.AddListener(Attack);

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
        fov.SetOrigin(transform.position);
        fov.SetViewDistance(viewOfDistance);
        fov.SetAimDirection(Vector3.right);
        fov.SetFoV(360);
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
    private bool canCountDown;
    float currentTime, totalTime;
    void MovePlayer()
    {
        //speed = movingSpeedCurve.Evaluate(currentTime);
        Vector2 movement = new Vector2(speed * joystick.Direction.normalized.x, speed * joystick.Direction.normalized.y);
        movement *= Time.deltaTime;

        rb.MovePosition(rb.position + movement);

        if (canCountDown)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= totalTime)
                canCountDown = false;
        }
    }



    void OnTriggerEnter(Collider other)
    {
        IItem item = other.GetComponent<IItem>();
        item?.Use(transform);
    }
}
