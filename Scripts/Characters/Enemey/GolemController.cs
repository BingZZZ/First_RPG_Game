using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemController : EnemyController
{
    [Header("KickOff")]
    public float kickForce;
    public GameObject rockPrefab;
    public Transform handPos;

    protected override void Attack()
    {
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStates.attackData.attackRange)
        {
            base.Attack();
        } else
        {
            Skill1();
        }
    }

    void Skill1()
    {
        characterStates.isCritical = UnityEngine.Random.value > characterStates.attackData.criticalChance;
        transform.LookAt(attackTarget.transform);
        remainCoolDown = characterStates.attackData.coolDown;
        anim.SetTrigger("Skill");
    }
    void ThrowRock()
    {
        if (attackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
    void KickOff()
    {
        if (attackTarget != null && transform.IsFacingAttackRange(attackTarget.transform))
        {
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            if (attackTarget.GetComponent<Animator>().GetBool("Defense") && attackTarget.transform.IsFacingAttackRange(transform))
            {
                attackTarget.GetComponent<Rigidbody>().AddForce(direction * kickForce * 0.5f, ForceMode.Impulse);
                attackTarget.GetComponent<Animator>().SetTrigger("Hit");
            } else
            {
                characterStates.TakeDamage(attackTarget.GetComponent<CharacterStates>());
                attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
                attackTarget.GetComponent<Rigidbody>().AddForce(direction * kickForce, ForceMode.Impulse);
            }
            //attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
        }
    }
}
