using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Enemy1 : MonoBehaviour
{
    Model_Player playerModel;
    Controller_Effects effects;
    Controller_EnemyBullets bullets;
    public ParticleSystem ps;
    public float rate = 360;
    public float hitPoints = 20;

    public float shootInterval;

    private float shootTimer;

    private void Start()
    {
        playerModel = GameObject.Find("Model").GetComponent<Model_Player>();
        effects = GameObject.Find("Controller").GetComponent<Controller_Effects>();
        bullets = GameObject.Find("Controller").GetComponent<Controller_EnemyBullets>();
        shootTimer = Random.Range(0, shootInterval);
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        
        if (shootTimer >= shootInterval)
        {
            shootTimer -= shootInterval;
            if (Vector3.Distance(playerModel.ship.transform.position, transform.position) > 5)
                bullets.FireBullet(transform.position, (playerModel.ship.transform.position - transform.position).normalized);
        }

        transform.Rotate(Vector3.up * Time.deltaTime * rate);

        var around = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider c in around)
        {
            if (c.gameObject.tag == "PlayerBullet")
            {
                c.gameObject.transform.position += Vector3.forward * 1000;
                hitPoints -= playerModel.damageCurrent;
                ps.Emit(40);
            }
        }

        if (hitPoints <= 0)
        {
            KillThisEnemy();
        }
    }

    public void KillThisEnemy()
    {
        effects.MakeExplosion(transform.position);
        gameObject.SetActive(false);
    }
}
