using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Configuration")]
public class WaveConfig : ScriptableObject
{
    //////////////////////////////////
    ///////////// FIELDS /////////////
    //////////////////////////////////

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;


    //////////////////////////////////
    //////////// METHODS /////////////
    //////////////////////////////////

    // Getter method for enemyPrefab.
    public GameObject GetEnemyPrefab() {
        return enemyPrefab;
    }

    // Getter method for waypoints' transform infos. 
    public List<Transform> GetWaypoints(){
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform){
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }

    // This method is to upgrade all the enemies inside a wave.
    public void UpgradeEnemies(){
        Enemy enemy = enemyPrefab.GetComponent<Enemy>();
        if(enemy){
            enemy.UpgradeEnemy();
        }
    }

    // This method is to upgrade the enemies inside the wave,
    // according to the current wave. (It is for load game state).
    public void UpgradeEnemiesOnLoad(int spawnedWaveIndex){
        Enemy enemy = enemyPrefab.GetComponent<Enemy>();
        if(enemy){
            for(int i = 0; i < spawnedWaveIndex; i++){
                enemy.UpgradeEnemy();
            }
        }
    }

    // Getter method for timeBetweenSpawns.
    public float GetTimeBetweenSpawns(){
        return timeBetweenSpawns;
    }

    // Getter method for numberOfEnemies.
    public int GetNumberOfEnemies(){
        return numberOfEnemies;
    }

    // Getter method for moveSpeed.
    public float GetMoveSpeed(){
        return moveSpeed;
    }
}
