using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //////////////////////////////////
    ///////////// FIELDS /////////////
    //////////////////////////////////

    [SerializeField] float damage;
    [SerializeField] GameObject target;
    [SerializeField] float speed = 10f;
    [SerializeField] string projectileType;
    [SerializeField] AudioClip explosionSFX;


    //////////////////////////////////
    ///////// START & UPDATE /////////
    //////////////////////////////////

    void Update(){
        if(target)
            AttackToTarget();
        else
            NoMoreUseForMe();
    }


    //////////////////////////////////
    ///////////// METHODS ////////////
    //////////////////////////////////   

    // This method is to destroy the projectiles if their target 
    // is already destroyed.
    void NoMoreUseForMe(){
        if(target == null)
            Destroy(gameObject);
        if(projectileType == "Rocket"){
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position, 0.5f); 
        }
    }

    // This method is to destroy the projectiles on collusion with 
    // enemies.
    public void Hit(){
        Destroy(gameObject);
        if(projectileType == "Rocket"){
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position, 0.5f); 
        }
    }

    // Getter method for the damage.
    public float GetDamage(){
        return damage;
    }

    // This method is to move the projectile towards the chosen
    // target.
    void AttackToTarget(){
        transform.position = Vector2.MoveTowards(transform.position, 
                                                target.transform.position, 
                                                speed*Time.deltaTime);
        // Face the projectile towards the target.
        transform.up = target.transform.position - transform.position;
    }

    // Setter method for target.
    public void SetTarget(GameObject target){
        this.target = target;
    }
}
