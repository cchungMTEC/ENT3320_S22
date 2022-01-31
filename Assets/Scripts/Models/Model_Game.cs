using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Game : MonoBehaviour
{
    [Header("Game State Transition Timings")]
    public float introDuration;
    public float dialogueDuration;
    public float spawnDuration;
    [Header("Enemies")]
    public GameObject enemyPrefab1;
    public GameObject enemyBulletPrefab1;
    public List<int> level1Waves;
    public float enemySpeed1;
    public float enemyBulletSpeed1;
    [Header("Effects")]
    public GameObject explosionPrefab1;
}
