using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHandler : MonoBehaviour
{
    //////////////////////////////////
    ///////////// FIELDS /////////////
    //////////////////////////////////   

    [SerializeField] List<Transform> turretPositions;
    [SerializeField] GameObject rLauncherPrefab;
    [SerializeField] GameObject turretPrefab;
    GameHandler gameHandler;

    // Used turret positins.
    List<int> turretIndexes = new List<int>();

    // Used rocket launcher positins.
    List<int> rLauncherIndexes = new List<int>();


    //////////////////////////////////
    ///////// START & UPDATE /////////
    //////////////////////////////////
    void Start(){
        gameHandler = GameObject.FindObjectOfType<GameHandler>();
    }


    //////////////////////////////////
    ///////////// METHODS ////////////
    //////////////////////////////////   

    // This method is to spawn a turret at a random pos.
    public void SpawnTurret(){
        if(gameHandler.GetPlayerMoney() >= 75 && turretPositions.Count != 0){
            // Get a random pos from the list to instantiate the turret.
            int rand = Random.Range(0, turretPositions.Count);
            while(rLauncherIndexes.Contains(rand) || turretIndexes.Contains(rand)){
                rand = Random.Range(0, turretPositions.Count);
            }
            Transform turretPos = turretPositions[rand];
            Vector3 spawnPos = new Vector3(turretPos.position.x, 
                                           turretPos.position.y, 
                                           turretPos.position.z-1);
            // Push the index to the used indexes list.
            turretIndexes.Add(rand);
            // Instantiate the turret.
            var newTurret = Instantiate(turretPrefab, spawnPos, Quaternion.identity);
            // Decrease the player money.
            gameHandler.UpdateMoney(-75);
        }
    }

    // This method is to spawn a rocket launcher at a random pos.
    public void SpawnRocketLauncher(){
        if(gameHandler.GetPlayerMoney() >= 200 && turretPositions.Count != 0){
            // Get a random pos from the list to instantiate the turret.
            int rand = Random.Range(0, turretPositions.Count);
            while(rLauncherIndexes.Contains(rand) || turretIndexes.Contains(rand)){
                rand = Random.Range(0, turretPositions.Count);
            }
            Transform turretPos = turretPositions[rand];
            Vector3 spawnPos = new Vector3(turretPos.position.x, 
                                           turretPos.position.y, 
                                           turretPos.position.z-1);
            // Push the index to the used indexes list.
            rLauncherIndexes.Add(rand);
            // Instantiate the rocket launcher.
            var newTurret = Instantiate(rLauncherPrefab, spawnPos, Quaternion.identity);
            // Decrease the player money.
            gameHandler.UpdateMoney(-200);
        }
    }

    // This method is to spawn turrets and r. launchers on load.
    public void SpawnOnLoad(List<int> turretIndexes, List<int> rLauncherIndexes){
        // Spawn turrets.
        for(int i = 0; i < turretIndexes.Count; i++){
            Vector3 spawnPos = new Vector3(turretPositions[turretIndexes[i]].position.x,
                                           turretPositions[turretIndexes[i]].position.y,    
                                           turretPositions[turretIndexes[i]].position.z-1);
            var newTurret = Instantiate(turretPrefab, spawnPos, Quaternion.identity);
            // Push the index to the used indexes list.
            this.turretIndexes.Add(turretIndexes[i]);
        }

        // Spawn rocket launchers.
        for(int i = 0; i < rLauncherIndexes.Count; i++){
            Vector3 spawnPos = new Vector3(turretPositions[rLauncherIndexes[i]].position.x,
                                           turretPositions[rLauncherIndexes[i]].position.y,    
                                           turretPositions[rLauncherIndexes[i]].position.z-1);
            var newTurret = Instantiate(rLauncherPrefab, spawnPos, Quaternion.identity);
            // Push the index to the used indexes list.
            this.rLauncherIndexes.Add(rLauncherIndexes[i]);
        } 
    }

    // Getter for turretIndexes.
    public List<int> GetTurretIndexes(){
        return turretIndexes;
    } 

    // Getter for rLauncherIndexes.
    public List<int> GetRLauncherIndexes(){
        return rLauncherIndexes;
    } 
}
