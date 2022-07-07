using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int Damage;
    [SerializeField] int Speed;
    [SerializeField] Rigidbody Body;
    [SerializeField] int DestroyTime;
    [SerializeField] GameObject HitEffect;

    // Start is called before the first frame update
    void Start()
    {
        // get direction by subtracting the bullets vector from the players vector, then mult by speed to apply speed to bullet
        Body.velocity = (GameManager.Instance.Player.transform.position - transform.position).normalized * Speed;

        // after some time, destroy bullet if it has not hit anything
        Destroy(gameObject, DestroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the target is damageable, it takes damage
        if (other.GetComponent<IDamagable>() != null)
        {
            // get target
            IDamagable isDamageable = other.GetComponent<IDamagable>();

            // apply damage
            isDamageable.TakeDamage(Damage);
        }

        // play hitEffect particles at hit location
        Instantiate(HitEffect, transform.position, HitEffect.transform.rotation);
        Destroy(gameObject); // destroy bullet after hit
    }
}
