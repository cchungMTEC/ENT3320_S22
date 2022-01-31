using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_PlayerGuns : MonoBehaviour
{
    public Model_Player playerModel;

    List<GameObject> _inactiveBullets;
    List<GameObject> _activeBullets;

    Transform leftGun;
    Transform rightGun;

    bool leftGunFire;

    void Start()
    {
        Debug.Assert(playerModel != null, "Controller_PlayerGuns is looking for a reference to Model_Player, but none has been added in the Inspector!");

        _inactiveBullets = new List<GameObject>();
        _activeBullets = new List<GameObject>();

        var guns = GameObject.FindGameObjectsWithTag("GunPos");
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i].gameObject.name == "GunLFirePos") 
                leftGun = guns[i].transform;
            else 
                rightGun = guns[i].transform;
        }
    }

    void Update()
    {
        _BulletsUpdate();
    }

    float fireTimer;
    public void GunsUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fireTimer = 0;
            leftGunFire = !leftGunFire;
            _FireBullet(leftGunFire);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            fireTimer += Time.deltaTime / playerModel.fireRate;
            if (fireTimer >= 1)
            {
                fireTimer -=1;
                leftGunFire = !leftGunFire;
                _FireBullet(leftGunFire);
            }
        }
    }

    private void _FireBullet(bool leftGunFire)
    {
        // Cannot fire while shields are up
        if (playerModel.shieldActive) return;

        // Grab a bullet that's already been made, or make a new one if there isn't one
        GameObject bullet;
        if (_inactiveBullets.Count > 0)
        {
            bullet = _inactiveBullets[0];
            bullet.SetActive(true);
            _inactiveBullets.Remove(bullet);
        }
        else
        {
            bullet = Instantiate(playerModel.bulletPrefab);
        }
        // Add bullet to _activeBullets to make it fly
        _activeBullets.Add(bullet);

        // Set starting position based on which gun should fire
        if (leftGunFire)
            bullet.transform.position = leftGun.position;
        else
            bullet.transform.position = rightGun.position;
    }

    private void _BulletsUpdate()
    {
        foreach (var bullet in _activeBullets)
        {
            bullet.transform.position += Vector3.forward * Time.deltaTime * playerModel.bulletSpeed;
        }

        // Goes backwards because removing element from collection
        if (_activeBullets.Count > 0)
        {
            for (int i = _activeBullets.Count - 1; i>= 0; i--)
            {
                if (_activeBullets[i].transform.position.z > playerModel.limitVert * 1.5f)
                {
                    GameObject bulletToMove = _activeBullets[i];
                    _activeBullets.Remove(bulletToMove);
                    _inactiveBullets.Add(bulletToMove);
                    bulletToMove.gameObject.SetActive(false);
                }
            }
        }
    }
}
