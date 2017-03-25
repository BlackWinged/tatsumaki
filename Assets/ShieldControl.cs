using UnityEngine;
using System.Collections;

public class ShieldControl : MonoBehaviour
{
    public float shieldStrength = 10000;

    public float shieldLingerTime = 1f;
    public float shieldStartupTime = 0.1f;

    private bool isActivating = false;
    private bool isActivated = false;
    private float timer = 0;

    private PolygonCollider2D shieldCollider;
    // Use this for initialization
    void Start()
    {
        shieldCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActivating)
        {
            shieldStartup();
        }
        if (isActivated)
        {
            shieldFade();
        }
    }

    public float takeDamage(float damage)
    {

        shieldStrength -= damage;
        if (shieldStrength > 0) { 

             if (shieldCollider != null) shieldCollider.enabled = true;
            damage = Mathf.Abs(shieldStrength);
            if (!isActivating)
            {
                timer = shieldStartupTime;
            }
            if (!isActivated)
            {
                isActivating = true;
                isActivated = false;
            } else
            {
                timer = shieldLingerTime;
            }
            damage = 0;
        } else
        {
            if (shieldCollider != null) shieldCollider.enabled = false;
        }
        return damage;
    }

    private void shieldStartup()
    {
        timer -= Time.deltaTime;
        float ratio = timer / shieldStartupTime;
        ratio = 1 - ratio;
        Color col = new Color();
        col.a = ratio;
        GetComponent<SpriteRenderer>().color = col;

        if (timer <= 0)
        {
            isActivating = false;
            isActivated = true;
            timer = shieldLingerTime;
        }
    }

    private void shieldFade()
    {
        timer -= Time.deltaTime;
        float ratio = timer / shieldLingerTime;
        Color col = new Color();
        col.a = ratio;
        GetComponent<SpriteRenderer>().color = col;

        if (timer <= 0)
        {
            isActivating = false;
            isActivated = false;
        }
    }
}
