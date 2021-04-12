using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerStyle { SOWRDSHIELD, TWOHANDSSWORD }
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Collider colli;
    private Animator anim;
    private Rigidbody rb;
    private GameObject attackTarget;
    private float lastAttackTime;
    private CharacterStates characterStates;
    private bool isDead;
    public bool isDefense;
    public int attackCount = 0;

    [Header("Player Style")]
    public PlayerStyle playerStyle;

    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        colli = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
    }

    //TODO: Some errors here. Please enable player when starting playing. Fix latter.
    private void OnEnable()
    {
        MouseManager.Instance.onMouseClicked += MoveToTarget;
        MouseManager.Instance.onEnemyClicked += EventAttack;
        GameManager.Instance.RegisterPlayer(characterStates);
        ResetPositionRotation();
    }
    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
    }

    // quit event after going to new scene. Otherwise, errors would happen.
    private void OnDisable()
    {
        if (!MouseManager.IsInitialized) return;
        MouseManager.Instance.onMouseClicked -= MoveToTarget;   
        MouseManager.Instance.onEnemyClicked -= EventAttack;
    }


    private void Update()
    {
        Braking();
        Dead();
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
        Defense();
        VictoryAnim();
        PlayerSetManager.Instance.SetPositionRotation(transform);
    }
    private void LateUpdate()
    {
        ResetAttack();
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;
        agent.isStopped = false;
        agent.destination = target;
    }

    private void VictoryAnim()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetTrigger("Victory");
        }
    }
    public void Braking()
    {
        if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)
        {
            agent.isStopped = true;
        }
    }

    void Defense()
    {
        if (Input.GetKey(KeyCode.J) && playerStyle == PlayerStyle.SOWRDSHIELD)
        {
            isDefense = true;
            agent.isStopped = true;
        } else
        {
            isDefense = false;
        }

    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Dead", isDead);
        anim.SetBool("Defense", isDefense);
    }
    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target != null)
        {
            attackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    private void Dead()
    {
        isDead = characterStates.CurrentHealth == 0;
        if (isDead)
        {
            agent.enabled = false;
            rb.useGravity = false;
            colli.enabled = false;
        }
    }

    public void AttackCombo()
    {
        Debug.Log(attackCount);
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("LocoMotion"));
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Debug.Log(anim.GetCurrentAnimatorStateInfo(1).normalizedTime);
        Debug.Log(attackCount);
        if (attackCount == 0 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f && anim.GetCurrentAnimatorStateInfo(0).IsName("LocoMotion"))
        {
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("Attack");
            //anim.Play("Attack01");
            attackCount = 1;
        } else if (attackCount == 1 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            //anim.Play("Attack02");
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("Attack");
            attackCount = 2;
        } else if (attackCount == 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            //anim.Play("Attack03");
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("Attack");
            attackCount = 3;
        } else if (attackCount == 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            //anim.Play("Attack03");
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("Attack");
            attackCount = 4;
        
        } else if (attackCount == 4 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack04"))
        {
            //anim.Play("Attack03");
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("Attack");
            attackCount = 0;
        }
    }

    private void ResetAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f && anim.GetCurrentAnimatorStateInfo(0).IsName("LocoMotion"))
        {
            attackCount = 0;
            anim.SetInteger("AttackCount", attackCount);
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(agent.transform.position, attackTarget.transform.position) > characterStates.attackData.attackRange)
        {
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        AttackCombo();
        //if (lastAttackTime < 0)
        //{
        //    anim.SetTrigger("Attack");
        //    //anim.SetBool("DoubleAttack", UnityEngine.Random.value <= characterStates.attackData.doubleAttack);
        //    lastAttackTime = characterStates.attackData.coolDown;
        //}
    }

    void Hit()
    {
        if (attackTarget != null)
        {
            Debug.Log("Attack");
            characterStates.isCritical = UnityEngine.Random.value <= characterStates.attackData.criticalChance;
            characterStates.TakeDamage(attackTarget.GetComponent<CharacterStates>());
            attackTarget.GetComponent<EnemyController>().enemyState = EnemyStates.CHASE;
            if (!attackTarget.GetComponent<Animator>().GetBool("Hit"))
                attackTarget.GetComponent<Animator>().SetTrigger("Hit");
        }
    }

    void ResetPositionRotation()
    {
        transform.position = PlayerSetManager.Instance.lookAtPoint.position;
        transform.rotation = PlayerSetManager.Instance.lookAtPoint.rotation;
    }
}
