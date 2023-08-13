using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //////////////////////////////////
    ///////////// FIELDS /////////////
    //////////////////////////////////    

    // Turret sprite stuff.
    SpriteRenderer spriteRenderer;

    // Enemy stuff.
    GameObject[] enemies;
    GameObject closestEnemy;
    GameObject currentEnemy;

    // Projectile stuff.
    [SerializeField] GameObject projectilePrefab;      
    [SerializeField] GameObject backfire;     
    [SerializeField] float shotCounter;
    float shotCounterCopy;
    [SerializeField] float fireRange;
    [SerializeField] AudioClip shotSFX;

    //////////////////////////////////
    ///////// START & UPDATE /////////
    //////////////////////////////////

    void Start(){
        backfire = gameObject.transform.Find("Backfire").gameObject;
        shotCounterCopy = shotCounter;
    }

    void Update(){
        FindClosestEnemy();
    }


    //////////////////////////////////
    ///////////// METHODS ////////////
    //////////////////////////////////   

    // This method is to find the closest enemy and fire.
    void FindClosestEnemy(){
        // Get the closest enemy.
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distanceToClosestEnemy = Mathf.Infinity;
        foreach(GameObject enemy in enemies){
            currentEnemy = enemy;
            Vector3 difference = enemy.transform.position - transform.position;
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if(distanceToEnemy <= distanceToClosestEnemy){
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = enemy;
                // Face to the closest enemy.
                FaceToTheClosest(difference);
            }
        }
        // Fire.
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
        shotCounterCopy -= Time.deltaTime;  
        if(shotCounterCopy <= 0f && distanceToClosestEnemy <= fireRange){
            Fire(target);
            shotCounterCopy = shotCounter;
        }
    }

    // This method is to make the enemies fire.
    private void Fire(GameObject target){
        AudioSource.PlayClipAtPoint(shotSFX, transform.position, 0.5f);  
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        StartCoroutine("BackFire");
        if(projectile.GetComponent<Projectile>())
            projectile.GetComponent<Projectile>().SetTarget(target);
    }

    // This method is to show backfire while the turret is shooting.
    public IEnumerator BackFire(){
        backfire.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        backfire.SetActive(false);
    }
}
