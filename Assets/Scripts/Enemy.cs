using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    //////////////////////////////////
    ///////////// FIELDS /////////////
    //////////////////////////////////

    [SerializeField] float health;
    [SerializeField] WaveConfig waveConfig;            
    List<Transform> waypoints;        
    int waypointIndex = 0;            
    GameHandler gameHandler;


    //////////////////////////////////
    ///////// START & UPDATE /////////
    //////////////////////////////////

    void Start()
    {
        // Initialize the waypoints and the game handler.
        waypoints = waveConfig.GetWaypoints();   
        gameHandler = FindObjectOfType<GameHandler>();
        // Get the initial position.
        transform.position = waypoints[waypointIndex].position; 
        
    }

    void Update()
    {
        Move();
    }


    //////////////////////////////////
    /////////// COLLUSION ////////////
    //////////////////////////////////

    private void OnTriggerEnter2D(Collider2D other){
        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if(!projectile){
            return;
        }
        ProcessHit(projectile);
    }


    //////////////////////////////////
    ///////////// METHODS ////////////
    //////////////////////////////////   
    
    // This method is to destroy the enemies.
    private void Die()
    {
        gameHandler.UpdateMoney(25);
        gameHandler.UpdateScore();
        Destroy(gameObject);
    }

    // This method is to process the hits on the enemies.
    private void ProcessHit(Projectile projectile){
        health -= projectile.GetDamage();
        projectile.Hit();
        if (health <= 0){
            Die();
        }
    }

    // This method is to increase the enemy hp.
    public void UpgradeEnemy(){
        health = health + 25;
    }

    // Setter method for wave config.
    public void SetWaveConfig(WaveConfig waveConfig){
        this.waveConfig = waveConfig;
    } 

    // This method is to make the enemies move.
    private void Move(){
        if(waypointIndex <= waypoints.Count - 1){
            var targetPosition = waypoints[waypointIndex].position;  
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;  
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);
            transform.up = targetPosition - transform.position;
            if (transform.position == targetPosition){                                                        
                waypointIndex++;                                     
            }
        }
        else{
            Destroy(gameObject);
            gameHandler.DecreaseHealth();
        }
    }
}
