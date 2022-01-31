using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_ShieldAndHealth : MonoBehaviour
{
    public Model_Player player;
    public Controller_EnemyBullets bullets;

    private float shieldRegenTimer;

    void Start()
    {
        Debug.Assert(player != null, "Controller_ShieldAndHealth is looking for a reference to Model_Player, but none has been added in the Inspector!");
    }

    public void OnSpawn()
    {
        Debug.Log("OnSpawn Called");
        player.hitpointsCurrent = player.hitpointsBase;
        player.shieldPointsMax = player.shielddPointsCurrent = player.shieldpointsBase;
        player.livesCurrent = player.livesBase;
        player.shieldRegenIntervalCurrent = player.shieldRegenIntervalBase;
    }

    public void ShieldAndHealthUpdate()
    {
        // Inputs
        if (Input.GetKey(KeyCode.LeftShift) && player.shielddPointsCurrent > 0)
            player.shieldActive = true;
        else
            player.shieldActive = false;

        // Update Model
        _ShieldOnOff();

        // Collision Detection
        float radius = 0;
        if (player.shieldActive)
            radius = player.shieldedRadius;
        else
            radius = player.unshieldedRadius;

        var colliders = Physics.OverlapSphere(player.ship.transform.position, radius);

        foreach (var c in colliders)
        {
            if (c.gameObject.tag == "Enemy")
            {
                if (player.shieldActive)
                {
                    player.shielddPointsCurrent -= 3;
                    player.shielddPointsCurrent = (int)Mathf.Max(player.shielddPointsCurrent, 0);
                    shieldRegenTimer = 0;
                }
                else           
                    player.hitpointsCurrent--;
                
                Behavior_Enemy1 e = c.GetComponent<Behavior_Enemy1>();
                e.KillThisEnemy();
            }
            else if (c.gameObject.tag == "EnemyBullet")
            {
                if (player.shieldActive)
                {
                    player.shielddPointsCurrent--;
                    player.shielddPointsCurrent = (int)Mathf.Max(player.shielddPointsCurrent, 0);
                    shieldRegenTimer = 0;
                }
                else
                    player.hitpointsCurrent--;

                bullets.KillBullet(c.gameObject);
            }
        }

        if (player.shielddPointsCurrent < player.shieldPointsMax)
        {
            shieldRegenTimer += Time.deltaTime;
            if (shieldRegenTimer >= player.shieldRegenIntervalCurrent)
            {
                shieldRegenTimer = 0;
                player.shielddPointsCurrent++;
            }
        }
    }

    private void _ShieldOnOff()
    {
        if (player.shieldActive)
            player.shield.SetActive(true);
        else
            player.shield.SetActive(false);
    }
}
