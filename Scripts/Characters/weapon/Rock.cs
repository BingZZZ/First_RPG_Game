using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RockStates { HitPlayer, HitNothing }
public class Rock : MonoBehaviour
{
    public Rigidbody rb;
    public float throwForce;
    public GameObject target;
    public float damage;
    private RockStates rockState;
    public ParticleSystem breakEffect;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rockState = RockStates.HitPlayer;
        ThrowToTarget();
    }

    public void ThrowToTarget()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        direction = target.transform.position - transform.position + Vector3.up;
        rb.AddForce(direction.normalized * throwForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (rockState)
        {
            case RockStates.HitPlayer:
                if (collision.gameObject.CompareTag("Player"))
                {
                    GameObject player = collision.gameObject;
                    if (player.GetComponent<Animator>().GetBool("Defense") && player.transform.IsFacingAttackRange(-direction.normalized))
                    {
                        player.GetComponent<Animator>().SetTrigger("Hit");
                        rockState = RockStates.HitNothing;
                    } else
                    {
                        player.GetComponent<CharacterStates>().TakeDamage(damage);
                        player.GetComponent<Animator>().SetTrigger("Hit");
                        rockState = RockStates.HitNothing;
                    }

                }
                break;
        }
        Destroy(gameObject);
        Instantiate(breakEffect, transform.position, Quaternion.identity);
    }

}
