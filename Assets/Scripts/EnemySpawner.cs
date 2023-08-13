using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    //////////////////////////////////
    ///////////// FIELDS /////////////
    //////////////////////////////////    

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;          
    [SerializeField] bool looping = false;         
    [SerializeField] TMP_Text waveInfo;
    int spawnedWaveIndex;


    //////////////////////////////////
    ///////// START & UPDATE /////////
    //////////////////////////////////


    IEnumerator Start(){
        // Keep spawning waves.
        do{
            yield return StartCoroutine(SpawnAllWaves());
        }
        while(looping);                                                                              
    }


    //////////////////////////////////
    ///////////// METHODS ////////////
    //////////////////////////////////   

    // This coroutine is to spawn all the waves.
    private IEnumerator SpawnAllWaves(){
        // Spawn all the (mini) waves.
        for(int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++){
            var currentWave = waveConfigs[waveIndex];
            StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            yield return new WaitForSeconds(10f);
        }
        // Increment the wave number.
        StartCoroutine("ShowWaveInfo");
        spawnedWaveIndex++;
        // Upgrade all the enemies.
        for(int i = 0; i < waveConfigs.Count; i++){
            var currentWave = waveConfigs[i];
            currentWave.UpgradeEnemies();
        }
        // Wait before the next wave.
        yield return new WaitForSeconds(5f);
    }

    // This coroutine is to spawn all the enemies inside a wave.
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig){
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++){
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity); 
            newEnemy.GetComponent<Enemy>().SetWaveConfig(waveConfig);
            // If the first wave is done, upgrade the enemies    
            if(spawnedWaveIndex != 0){
                newEnemy.GetComponent<Enemy>().UpgradeEnemy();
            }
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }

    // This coroutine is to show the wave information.
    public IEnumerator ShowWaveInfo(){
        waveInfo.text = ("WAVE " + (spawnedWaveIndex + 1).ToString());
        waveInfo.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        waveInfo.gameObject.SetActive(false);
    }

    // This method is to upgrade the enemies inside all the waves,
    // according to the current wave index. (It is for loading the game).
    public void UpgradeEnemiesOnLoad(int spawnedWaveIndex){
        for(int i = 0; i < waveConfigs.Count; i++){
            var currentWave = waveConfigs[i];
            currentWave.UpgradeEnemiesOnLoad(spawnedWaveIndex);
        }
    }

    // Getter for spawnedWaveIndex.
    public int GetWaveIndex(){
        return spawnedWaveIndex;
    }

    // Setter for spawnedWaveIndex.
    public void SetWaveIndex(int spawnedWaveIndex){
        this.spawnedWaveIndex = spawnedWaveIndex;
    }
}
