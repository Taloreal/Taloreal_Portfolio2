using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamagable
{

    [Header("Components")]
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] Renderer Render;

    [Header("--------------------------------")]
    [Header("Enemy Attributes")]
    [SerializeField] int HP;

    [Header("--------------------------------")]
    [Header("Weapon Stats")]
    [SerializeField] float ShootRate;
    [SerializeField] GameObject Bullet;

    [SerializeField] bool CanShoot = true;
    //bool PlayerInRange = false;

    void Start()
    {
    }


    void Update()
    {
        Agent.SetDestination(GameManager.Instance.Player.transform.position);

        if (Agent.remainingDistance <= Agent.stoppingDistance && CanShoot)
            StartCoroutine(Shoot());
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(FlashColor());

        if (HP <= 0)
            Destroy(gameObject);
    }

    IEnumerator FlashColor()
    {
        Render.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Render.material.color = Color.white;
    }

    IEnumerator Shoot()
    {
        CanShoot = false;
        Instantiate(Bullet, transform.position, Bullet.transform.rotation);
        yield return new WaitForSeconds(ShootRate);
        CanShoot = true;
    }
}
