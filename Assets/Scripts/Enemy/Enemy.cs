using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region PublicVariables

    public Vector2 currentPosition { get; private set; }
    public Vector2 oldPosition { get; private set; }

    public Unit movementScript;

    public GameObject dodgeTrail { get; private set; }

    public Transform target;

    private bool canAttackPlayer = true;

    public event Action OnChangedPosition;

    

    [Header("Actual state")]
    public State currentState;

    [HideInInspector]
    public float distanceToPlayer;
    public float distanceToDetecting = 4f;
    #endregion

    #region PrivateVariables

    [Header("Enemy Stats(source)")]
    [SerializeField]
    protected EnemyStats myEnemyStats;

    [SerializeField]
    int lowHpValue = 1;

    #endregion

    void PositionHasChanged()
    {
        if (OnChangedPosition != null)
            OnChangedPosition();
    }

    protected void Start()
    {
        MenuManager.MG.MatchEnded += DestroyGameObject;

        oldPosition = currentPosition;
        target = GameTypeBase.instance.Player.transform;
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        MenuManager.MG.MatchEnded -= DestroyGameObject;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            myEnemyStats.Die(target.gameObject);

        #region TempRegionForPosition

        currentPosition = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        distanceToPlayer = Vector3.Distance(target.position, transform.position);


        if (currentPosition != oldPosition)
        {
            PositionHasChanged();

            oldPosition = currentPosition;
        }
        #endregion

        //All states checks and changes
        #region StatesWork

        DoStatementWork();

        #endregion
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(player != null)
            OnTouchedByPlayer(player);
    }

    public virtual void OnTouchedByPlayer(PlayerController player)
    {
        Debug.Log($"{transform.name} was touched by {player.transform.name}");
    }

    public virtual void DoStatementWork()
    {
        
    }

    public bool TargetIsLost() { return distanceToPlayer > distanceToDetecting ? true : false; }
    public bool TargetIsLost(float multiplier) { return distanceToPlayer > distanceToDetecting * multiplier ? true : false; }
    protected bool ThisStateWasPrevious(State.EnemyStatement state) { return currentState.statement == state ? true : false; }
    
    public void SetState(State state)
    {
        currentState?.Exit();

        StopAllCoroutines();
        //Do copy (Instantiate(state) because it is ScriptableObject. 
        //When parameters will be changed it change parameters in asset menu!
        currentState = Instantiate(state);
        currentState.enemy = this;
        currentState.Init();
    }


    public void MoveToTarget(Vector2 myPos, Vector2 target)
    {
        movementScript.MoveToTarget(myPos, target);
    }

    public bool isLowHp()
    {
        return myEnemyStats.Health <= lowHpValue ? true : false;
    }

    public void WaitAndDoAction(float timeToWait, Action action)
    {
        Action stopAction = () => { StopCoroutine("Coroutine_WaitAndDoAction"); };
        StartCoroutine(Coroutine_WaitAndDoAction(timeToWait, action));
    }

    IEnumerator Coroutine_WaitAndDoAction(float timeToWait, Action action)
    {
        yield return new WaitForSeconds(timeToWait);

        action();

        yield break;
    }
}
