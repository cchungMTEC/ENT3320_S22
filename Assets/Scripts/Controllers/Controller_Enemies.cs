using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Enemies : MonoBehaviour
{
    public Model_Game gameModel;

    public List<Wave> waves;
    private float waveTimer = 1000;
    private int waveIndex;
    void Start()
    {
        Debug.Assert(gameModel != null, "Controller_Enemies is looking for a reference to Model_Game, but none has been added in the Inspector!");
        waves = new List<Wave>();
    }

    void Update()
    {
        // Enemies Movement
        for (int i = waves.Count -1; i >= 0; i--)
        {
            Wave wave = waves[i];
            bool anyLeft = false;
            foreach (var enemy in wave.enemies)
            {
                //checking wave to see if any are left
                //this allows us to simply set an enemy's gameObject to not be active to effectively kill it
                //to allow for effects, we handle this with the enemey's behavior
                //  this does two things: 
                //      - this allows us to manage resources in the controller more cleanly
                //      - this allows us stop the enemey's behavior for continuing to run after it is dead
                if (enemy.transform.gameObject.activeSelf && enemy.transform.position.z > -16)
                    anyLeft = true;

                if (enemy.waypointIndex < wave.waypoints.Count)
                {
                    enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, wave.waypoints[enemy.waypointIndex], gameModel.enemySpeed1 * Time.deltaTime);
                    if (Vector3.Distance(enemy.transform.position, wave.waypoints[enemy.waypointIndex]) < .03f)
                        enemy.waypointIndex++;
                }
                else
                {
                    enemy.transform.position -= Vector3.forward * gameModel.enemySpeed1 * Time.deltaTime;
                }
            }

            if (!anyLeft)
            {
                CleanUpWave(wave);
            }
        }
    }

    public void EnemyUpdate()
    {
        // Making waves for the level according to model specifications
        waveTimer += Time.deltaTime;
        float turnOverTime = 15;
        if (waveTimer >= turnOverTime && waveIndex < gameModel.level1Waves.Count)
        {
            Vector3 startPoint = new Vector3(Random.Range(-17f, 17f), 0, 20);
            Vector3 spawnStaggerDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(0, 1f)).normalized;
            int numberToSpawn = gameModel.level1Waves[waveIndex];

            Wave newWave = new Wave();

            for (int i = 0; i < numberToSpawn; i++)
            {
                EnemyOnPath EOP = new EnemyOnPath();
                EOP.transform = Instantiate(gameModel.enemyPrefab1).transform;
                EOP.transform.position = startPoint + spawnStaggerDir * 3 * i;
                newWave.enemies.Add(EOP);
            }

            for (int i = 0; i < numberToSpawn; i++)
            {
                newWave.waypoints.Add(new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-8f, 8f)));
            }

            waves.Add(newWave);

            waveTimer = 0;
            waveIndex++;
        }
    }

    private void CleanUpWave(Wave wave)
    {
        for (int j = wave.enemies.Count - 1; j >= 0; j--)
        {
            var EOP = wave.enemies[j];
            wave.enemies.Remove(EOP);
            Destroy(EOP.transform.gameObject);
        }
        waves.Remove(wave);
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyOnPath> enemies;
        public List<Vector3> waypoints;

        public Wave()
        {
            enemies = new List<EnemyOnPath>();
            waypoints = new List<Vector3>();
        }
    }

    [System.Serializable]
    public class EnemyOnPath
    {
        public Transform transform;
        public int waypointIndex;
    }
}
