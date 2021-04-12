using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntController : EnemyController
{
    [Header("Skill")]
    public float kickForce = 5;
    private float kickOffChance = 0.4f;

    void KickOff()
    {
        if (attackTarget != null)
        {
            transform.LookAt(attackTarget.transform);
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            Debug.Log(direction * kickForce);
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            //attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            //Debug.Log(attackTarget.GetComponent<NavMeshAgent>().velocity);
            if(attackTarget.GetComponent<Animator>().GetBool("Defense") && attackTarget.transform.IsFacingAttackRange(transform))
            {
                attackTarget.GetComponent<Rigidbody>().AddForce(direction * kickForce * 0.5f, ForceMode.Impulse);
            } else
            {
                attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
                attackTarget.GetComponent<Rigidbody>().AddForce(direction * kickForce, ForceMode.Impulse);
            }
        }
    }

    protected override void Attack()
    {
        bool isKickOff = UnityEngine.Random.value <= kickOffChance;
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStates.attackData.skillRange && isKickOff)
        {
            Skill1();
        } else
        {
            base.Attack();
        }
    }

    void Skill1()
    {
        characterStates.isCritical = UnityEngine.Random.value <= characterStates.attackData.criticalChance;
        transform.LookAt(attackTarget.transform.position);
        remainCoolDown = characterStates.attackData.coolDown;
        anim.SetTrigger("Skill");
    }
}
