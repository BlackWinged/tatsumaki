using UnityEngine;
using System.Collections;

public class HullControl : MonoBehaviour
{
    public float maxTimeToShotDetionation = 0.4f;
    public float HitPoints = 1000;
    public float Armor = 5;


    private ShieldControl shield;

    // Use this for initialization
    void Start()
    {
        shield = transform.root.GetComponentInChildren<ShieldControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void takeDamage(float damage, Vector2 forceDirection, out float timeToShotDetonation)
    {
        damage = shield.takeDamage(damage);
        if (HitPoints > 0)
        {
            HitPoints -= (damage - Armor) > 0 ? damage - Armor : 0;
        }
        if (HitPoints < 0)
        {
            HitPoints = 0;
        }
        timeToShotDetonation = maxTimeToShotDetionation * Random.value;
    }
}
