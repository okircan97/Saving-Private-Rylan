using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour
{
    //////////////////////////////////
    ///////////// FIELDS /////////////
    //////////////////////////////////  

    [SerializeField] TMP_Text healthDisplay;
    [SerializeField] TMP_Text moneyDisplay;
    [SerializeField] TMP_Text scoreDisplay;
    [SerializeField] TMP_Text saveDisplay;
    private int playerScore = 0;
    private int playerHp    = 10;
    private int playerMoney = 150;
    SceneHandler sceneHandler;
    EnemySpawner enemySpawner;
    TurretHandler turretHandler;
    int highScore;

    // Lists to save turret and rocket launcher indexes.
    public List<int> turretIndexes;
    public List<int> rLauncherIndexes;


    //////////////////////////////////
    ///////// START & UPDATE /////////
    //////////////////////////////////

    void Start()
    {
        DisplayTexts();
        sceneHandler  = FindObjectOfType<SceneHandler>();
        enemySpawner  = FindObjectOfType<EnemySpawner>();
        turretHandler = FindObjectOfType<TurretHandler>();
        highScore = PlayerPrefs.GetInt(ScoreHandler.highScoreKey, 0);

        // If the game is loaded, get the necessary stuff from 
        // playerprefs.
        if(PlayerPrefs.GetInt("ShouldLoad", 1) == 1){
            LoadGameState();
            PlayerPrefs.SetInt("ShouldLoad", 0);
        }
    }


    //////////////////////////////////
    ///////////// METHODS ////////////
    //////////////////////////////////   

    // This method is to decrease the player health when
    // an enemy reaches to the end of its path.
    public void DecreaseHealth(){
        // If the HP is higher than 1, decrease it.
        if(playerHp > 1){
            playerHp--;
            healthDisplay.text = playerHp.ToString();
        }
        // If not, load the main menu.
        else{
            sceneHandler.OpenMainMenu();
            if(highScore < playerScore){
                PlayerPrefs.SetInt(ScoreHandler.highScoreKey, playerScore);
            }
        }
    }

    // This method is to update the playerMoney.
    public void UpdateMoney(int num){
        playerMoney += num;
        moneyDisplay.text = playerMoney.ToString();
    }

    // This method is to update the playerScore.
    public void UpdateScore(){
        playerScore ++;
        scoreDisplay.text = "SCORE: " + playerScore.ToString();
    }

    // Getter method for playerMoney.
    public int GetPlayerMoney(){
        return playerMoney;
    }

    // This method is to display the necessary texts.
    void DisplayTexts(){
        healthDisplay.text = playerHp.ToString();
        moneyDisplay.text  = playerMoney.ToString();
        scoreDisplay.text  = "SCORE: " + playerScore.ToString();        
    }


    // This method is to save the game state.
    public void SaveGameState(){
        // Get the highest score.
        int highScore = PlayerPrefs.GetInt(ScoreHandler.highScoreKey, 0);
        // Clean playerprefs (for turrets).
        PlayerPrefs.DeleteAll();
        // Set new values.
        PlayerPrefs.SetInt(ScoreHandler.highScoreKey, highScore);
        PlayerPrefs.SetInt("Score", playerScore);
        PlayerPrefs.SetInt("Health", playerHp);
        PlayerPrefs.SetInt("Money", playerMoney);
        PlayerPrefs.SetInt("Wave", enemySpawner.GetWaveIndex());
        SaveTurretIndexes();
        StartCoroutine("ShowSaveDisplay");
    }

    // This method is to load the game state.
    void LoadGameState(){
        // Update the text displays.
        playerScore = PlayerPrefs.GetInt("Score", 0);
        playerHp    = PlayerPrefs.GetInt("Health", 10);
        playerMoney = PlayerPrefs.GetInt("Money", 150);
        // Update the current wave and enemies.
        enemySpawner.SetWaveIndex(PlayerPrefs.GetInt("Wave", 0));
        enemySpawner.UpgradeEnemiesOnLoad(PlayerPrefs.GetInt("Wave", 0));
        DisplayTexts();
        // Spawn the turrets.
        LoadTurretIndexes();
        turretHandler.SpawnOnLoad(turretIndexes, rLauncherIndexes);
    }

    // This coroutine is to show the save display.
    private IEnumerator ShowSaveDisplay(){
        saveDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        saveDisplay.gameObject.SetActive(false);
    }

    // This method is to save the turret and r. launcher indexes.
    void SaveTurretIndexes(){
        turretIndexes    = turretHandler.GetTurretIndexes();
        rLauncherIndexes = turretHandler.GetRLauncherIndexes();

        // Save the turret indexes.
        for(int i = 0; i < turretIndexes.Count; i++){
            PlayerPrefs.SetInt("turret_" + i, turretIndexes[i]);
        }

        // Save the r. launcher indexes.
        for(int i = 0; i < rLauncherIndexes.Count; i++){
            PlayerPrefs.SetInt("rlaunch_" + i, rLauncherIndexes[i]);
        }
    }

    // This method is to get the turret and r. launcher indexes.
    void LoadTurretIndexes(){
        for(int i = 0; i < 8; i++){
            // Get the turrets.
            int turretIndex = PlayerPrefs.GetInt("turret_" + i, 999);
            if(turretIndex != 999){
                turretIndexes.Add(turretIndex);
            }
            // Get the r. launchers.
            int rLauncherIndex = PlayerPrefs.GetInt("rlaunch_" + i, 999);
            if(rLauncherIndex != 999){
                rLauncherIndexes.Add(rLauncherIndex);
            }
        }
    }
}
