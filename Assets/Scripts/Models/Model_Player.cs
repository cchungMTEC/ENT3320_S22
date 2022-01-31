using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Player : MonoBehaviour
{
    public GameObject shipPrefab;
    [HideInInspector]public GameObject ship;
    public GameObject shieldPrefab;
    [HideInInspector] public GameObject shield;
    public GameObject bulletPrefab;

    [Header("Starting Values")]
    public Vector3 positionSpawnStart;
    public Vector3 positionSpawnFinish;
    public int shieldpointsBase;
    public int hitpointsBase;
    public float damageBase;
    public int livesBase;
    public float shieldedRadius;
    public float unshieldedRadius;

    [Header("Tuning Values")]
    [Range(0.01f, 30.0f)] public float smoothingFactor;
    public float shipSpeed = 20;
    public float bulletSpeed = 35;
    public float fireRate = .15f;
    public float limitHorz = 10;
    public float limitVert = 5;
    public float shieldRegenIntervalBase = 1.25f;

    [Header("Controlled Variables")]
    public Vector3 positionTarget;
    public Vector3 positionCurrent;
    public int shieldPointsMax;
    public int shielddPointsCurrent;
    public int hitpointsCurrent;
    public float damageCurrent;
    public int livesCurrent;
    public bool shieldActive;
    public float shieldRegenIntervalCurrent;
}
