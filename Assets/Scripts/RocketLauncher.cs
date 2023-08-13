using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    //////////////////////////////////
    ///////////// FIELDS /////////////
    //////////////////////////////////    

    // Turret sprite stuff.
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;

    // Enemy stuff.
    GameObject[] enemies;
    GameObject closestEnemy;
    GameObject currentEnemy;

    // Projectile stuff.
    [SerializeField] GameObject projectilePrefab;           
    [SerializeField] float shotCounter;
    float refillCounter;
    bool isFiring = true;
    [SerializeField] float fireRange;
    [SerializeField] AudioClip rocketSFX;


    //////////////////////////////////
    ///////// START & UPDATE /////////
    //////////////////////////////////

    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FindClosestEnemy();
        HandleSprite();
    }


    //////////////////////////////////
    ///////////// METHODS ////////////
    //////////////////////////////////   

    // This method is to find the closest enemy and face to it.
    void FindClosestEnemy(){
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distanceToClosestEnemy = Mathf.Infinity;
        foreach(GameObject enemy in enemies){
            currentEnemy = enemy;
            Vector3 difference = enemy.transform.position - transform.position;
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if(distanceToEnemy <= distanceToClosestEnemy){
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = enemy;
                FaceToTheClosest(difference);
            }
        }
        if(closestEnemy != null){
            // Debug.DrawLine(transform.position, closestEnemy.transform.position, Color.red);
            CountDownAndShot(closestEnemy, distanceToClosestEnemy);
        }

    }

    // This method is to make the turrets and the projectiles face to the
    // closest enemy.
    void FaceToTheClosest(Vector3 difference){
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // This method is to make the turrets count down and fire. 
    private void CountDownAndShot(GameObject target, float distanceToClosestEnemy){
        shotCounter -= Time.deltaTime;  
        if(shotCounter <= 0f && distanceToClosestEnemy <= fireRange){
            Fire(target);
            shotCounter = 3f;
        }
    }


    // This method is to make the enemies fire.
    private void Fire(GameObject target){
        AudioSource.PlayClipAtPoint(rocketSFX, transform.position, 0.5f);  
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        if(projectile.GetComponent<Projectile>())
            projectile.GetComponent<Projectile>().SetTarget(target);
        isFiring = true;
    }

    // This method is to handle the changes on turret sprite.
    private void HandleSprite(){
        if(isFiring){
            spriteRenderer.sprite = sprites[1];
            refillCounter -= Time.deltaTime;  
            if(refillCounter <= 0f){
                spriteRenderer.sprite = sprites[0];
                refillCounter = 1;
                isFiring = false;
            }
        }  
    }
}
