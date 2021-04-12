using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD}

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private NavMeshAgent agent;
    public EnemyStates enemyState;
    public EnemyStates enemyType;
    protected Animator anim;

    #region animation
    protected bool isWalk;
    private bool isChase;
    private bool isFollow;
    private bool isCritical;
    private bool isDead;
    #endregion

    private float attackRange;
    public float lookAtTime;

    private float remainLookAtTime;
    protected float remainCoolDown;

    public CharacterStates characterStates;

    private Vector3 patrolPos;
    private Vector3 guardPos;
    private Quaternion guardRotation;
    private Collider coli;
        
    private Vector3 wayPoint;

    [Header("Basic Settings")]
    public float sightRadius;

    [Header("Patrol State")]
    public float patrolRange;

    protected GameObject attackTarget;
    public float speed = 2.5f;
    // Start is called before the first frame update

    private void Awake()
    {
        patrolRange = 5.0f;
        lookAtTime = 4.0f;
        remainCoolDown = 0.0f;

        patrolPos = transform.position;
        guardPos = transform.position;
        guardRotation = transform.rotation;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        coli = GetComponent<Collider>();
        characterStates = GetComponent<CharacterStates>();
        enemyState = enemyType;
    }

    private void Start()
    {
        GetNewWayPoint();

        //FIXME: fix when scene changing.
        GameManager.Instance.AddEndGameObserver(this);
    }
    // Update is called once per frame
    //private void OnEnable()
    //{
    //    GameManager.Instance.AddEndGameObserver(this);
    //}

    private void OnDisable()
    {
        if (GameManager.IsInitialized)
            GameManager.Instance.RemoveEndGameObserver(this);
    }

    void Update()
    {
        isDead = characterStates.CurrentHealth == 0;
        GlobalRemainTime();
        SwitchStates();
        SwitchAnimation();
    }

    private void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStates.isCritical);
        anim.SetBool("Dead", isDead);
    }

    private void SwitchStates()
    {
        if (isDead)
        {
            enemyState = EnemyStates.DEAD;
        } else if (FoundPlayer())
        {
            enemyState = EnemyStates.CHASE;
        } else
        {
            enemyState = enemyType;
        }
        // if find player, change to CHASE
        switch (enemyState)
        {
            case EnemyStates.GUARD:
                Guard();
                break;
            case EnemyStates.PATROL:
                Patrol();
                break;
            case EnemyStates.CHASE:
                Chase();
                break;
            case EnemyStates.DEAD:
                Dead();
                break;
        }
    }

    private void GlobalRemainTime()
    {
        remainCoolDown -= Time.deltaTime;
    }

    void Dead()
    {
        coli.enabled = false;
        agent.radius = 0;
        Destroy(gameObject, 2.0f);
    }

    void Guard()
    {
        isFollow = false;
        isChase = false;
        if (Vector3.Distance(transform.position, guardPos) > agent.stoppingDistance)
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;
        } else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.05f);
            isWalk = false;
            agent.isStopped = true;
        }
    }

    private void Patrol()
    {
        agent.isStopped = false;
        agent.speed = speed * 0.5f;
        isFollow = false;
        isChase = false;
        isWalk = true;
        if (Vector3.Distance(transform.position, wayPoint) < agent.stoppingDistance)
        {
            isWalk = false;
            agent.destination = transform.position;
            if (remainLookAtTime < 0)
            {
                GetNewWayPoint();
            }
            remainLookAtTime -= Time.deltaTime;
        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }

    protected virtual void Attack()
    {
        characterStates.isCritical = UnityEngine.Random.value <= characterStates.attackData.criticalChance;
        transform.LookAt(attackTarget.transform.position);
        remainCoolDown = characterStates.attackData.coolDown;
        anim.SetTrigger("Attack");
    }

    private void Chase()
    {
        isWalk = false;
        isChase = true;
        agent.speed = speed;
        if (Vector3.Distance(transform.position, attackTarget.transform.position) > Mathf.Max(characterStates.attackData.attackRange, characterStates.attackData.skillRange))
        {
            agent.isStopped = false;
            isFollow = true;
            agent.destination = attackTarget.transform.position;
        }
        else
        {
            agent.isStopped = true;
            isFollow = false;
            if (remainCoolDown < 0)
            {
                Attack();
            }
        }
    }

    private bool FoundPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var hitCollider in hitColliders)
        {
            //if (hitCollider.CompareTag("Player") && (transform.IsFacingTarget(hitCollider.gameObject.transform) || enemyState == EnemyStates.CHASE))
            if (hitCollider.CompareTag("Player"))
            {
                attackTarget = hitCollider.gameObject;               
                return true;
            }
        }
        return false;
    }

    void GetNewWayPoint()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(patrolPos.x + randomX, transform.position.y, patrolPos.z + randomZ);
        NavMeshHit hit;
        // avoid the situation when the randomPoint is an unwalkabel point.
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, 1.0f, 1) ? hit.position : transform.position;
        remainLookAtTime = lookAtTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    void Hit()
    {
        if (attackTarget != null && transform.IsFacingAttackRange(attackTarget.transform))
        {
            if (!attackTarget.GetComponent<Animator>().GetBool("Defense"))
            {
                characterStates.TakeDamage(attackTarget.GetComponent<CharacterStates>());
                //if (!attackTarget.GetComponent<Animator>().GetBool("Hit"))
            } else
            {
                attackTarget.GetComponent<Animator>().SetTrigger("Hit");
            }
        }
    }

    public void EndNotify()
    {
        Debug.Log("Player Defeated");
    }
}
