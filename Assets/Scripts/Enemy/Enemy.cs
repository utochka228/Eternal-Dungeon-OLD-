using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class Enemy : MonoBehaviour
{
    [Range(0f, 1f)] public float spawnChance;
    public Vector2 currentPosition { get; private set; }
    public Vector2 oldPosition { get; private set; }
    [SerializeField] AIDestinationSetter targetSetter;
    public event Action OnChangedPosition;
    [Header("Start Enemy State")] public State startState;
    [Header("Actual state")] public State currentState;
    public Animator animator;
    [HideInInspector] public float distanceToPlayer;
    public List<GameObject> TargetsInMyViewZone{ get; private set;}
    public GameObject GetClosestVisibleTarget(){
        if(TargetsInMyViewZone.Count == 0)
            return null;
        GameObject closestTarget = TargetsInMyViewZone[0];
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        foreach (var target in TargetsInMyViewZone)
        {
            Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.y);
            float dist = Vector2.Distance(targetPos, myPos);
            Vector2 prevTargetPos = new Vector2(closestTarget.transform.position.x, closestTarget.transform.position.y);
            float prevTargetDist = Vector2.Distance(myPos, prevTargetPos);
            if(dist > prevTargetDist)
                closestTarget = target;            
        }
        return closestTarget;
    }

    [Header("Enemy Stats(source)")]
    protected EnemyStats myEnemyStats;
    [SerializeField]
    int lowHpValue = 1;
    Transform player;

    void PositionHasChanged()
    {
        if (OnChangedPosition != null)
            OnChangedPosition();
    }
    AIPath aIPath;
    [SerializeField] Transform gfx;
    protected void Start()
    {
        TargetsInMyViewZone = new List<GameObject>();
        myEnemyStats = GetComponent<EnemyStats>();
        oldPosition = currentPosition;
        player = GameSession.instance.Player.transform;
        aIPath = GetComponent<AIPath>();
        SetState(startState);
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        GameActions.instance.MatchEnded -= DestroyGameObject;
    }

    void Update()
    {
        //Flipping enemy
        if(aIPath != null){
            if(aIPath.desiredVelocity.x >= 0.01f){
                gfx.eulerAngles = new Vector3(0, 0, 0);
            }else if(aIPath.desiredVelocity.x <= -0.01f)
            {
                gfx.eulerAngles = new Vector3(0, 180f, 0);
            }
        }
        
        #region TempRegionForPosition
        currentPosition = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        Vector2 plPos = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.y);
        distanceToPlayer = Vector2.Distance(plPos, thisPos);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        //Adding entered collider to visible targets
        Stats creature = other.GetComponent<Stats>();
        Enemy enemy = other.GetComponent<Enemy>();
        if(creature != null && enemy == null)
            if(!TargetsInMyViewZone.Contains(other.gameObject))
                TargetsInMyViewZone.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other) {
        //Remove from visible targets
        if(TargetsInMyViewZone.Contains(other.gameObject)){
            TargetsInMyViewZone.Remove(other.gameObject);
        }
    }
    public virtual void DoStatementWork(){}
    
    public void SetState(State state)
    {
        currentState?.Exit();

        StopAllCoroutines();
        currentState = Instantiate(state);
        currentState.enemy = this;
        currentState.Init();
    }

    public void SetTarget(Transform target){
        targetSetter.target = target;
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
