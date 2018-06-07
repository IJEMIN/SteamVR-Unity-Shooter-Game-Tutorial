using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour, IDamageable {

    public float health = 100;


    public void OnDamage(float damageAmout)
    {
        health -= damageAmout;

        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
