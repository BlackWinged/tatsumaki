using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserCollisionHandler : MonoBehaviour
{
    public LayerMask Ground;
    public LayerMask Ships;
    public float maxRange;
    public float forceAdd = 100f;
    public float maxVelocity = 15f;
    public float damage = 20;
    public float forceAmount = 500;
    public GameObject firedFrom;

    private Vector3 positionOld;
    private float distance;
    private float timeToShotDetonation;
    private Transform player;

    private bool hasHit = false;
    // Use this for initialization
    void Start()
    {
        // player = GameObject.FindGameObjectWithTag("Player").transform;
        positionOld = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        detonation();
        distance += Vector2.Distance(transform.position, positionOld);
        //if (GetComponent<PolygonCollider2D>().IsTouchingLayers(Ground) || distance > maxRange)
        if (distance > maxRange)
        {
            Destroy(gameObject);
        }
        positionOld = transform.position;

        //Vector3 direction = player.position - transform.position;
        //GetComponent<Rigidbody2D>().drag = 0;
        //GetComponent<Rigidbody2D>().AddForce(direction * forceAdd);
        //GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maxVelocity);

        if (GetComponent<PolygonCollider2D>().IsTouchingLayers(Ships))
        {
            //if (GetComponent<PolygonCollider2D>().IsTouching(player.GetComponent<BoxCollider2D>()))
            //{
            // //   player.GetComponent<healthControl>().takeDamage(damage, GetComponent<Rigidbody2D>().velocity, forceAmount);

            //}
            //else
            //{
            //    List<GameObject> enemies = new List<GameObject>();
            //    enemies.AddRange(GameObject.FindGameObjectsWithTag("EnemyWithArm"));
            //    foreach (GameObject enemy in enemies)
            //    {
            //        if (GetComponent<PolygonCollider2D>().IsTouching(enemy.GetComponent<BoxCollider2D>()))
            //        {
            //     //       enemy.GetComponent<healthControl>().takeDamage(damage, GetComponent<Rigidbody2D>().velocity, forceAmount);
            //        }
            //    }
            //}
         //   death(gameObject);
        }

    }


    void death(GameObject e)
    {
        if (e.transform.parent != null)
        {
            death(e);
        }
        Destroy(e);
    }

     void OnTriggerEnter2D(Collider2D coll)
    {

        if (FlagsHelper.IsSet(Ships.value,  Mathf.RoundToInt(Mathf.Pow(2, coll.gameObject.layer))) && firedFrom != coll.transform.root.gameObject && !hasHit)
        {
            hit(coll);
        }
    }
    private void hit(Collider2D coll)
    {
        coll.transform.root.GetComponentInChildren<HullControl>().takeDamage(damage, GetComponent<Rigidbody2D>().velocity, out timeToShotDetonation);
        hasHit = true;
    }

    public void detonation()
    {
        if (hasHit)
        {
            timeToShotDetonation -= Time.deltaTime;
            if (timeToShotDetonation < 0)
            {
                death(gameObject);
            }
        }
    }

}


public static class FlagsHelper
{
    public static bool IsSet<T>(T flags, T flag) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        return (flagsValue & flagValue) != 0;
    }

    public static void Set<T>(ref T flags, T flag) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)(flagsValue | flagValue);
    }

    public static void Unset<T>(ref T flags, T flag) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)(flagsValue & (~flagValue));
    }

}