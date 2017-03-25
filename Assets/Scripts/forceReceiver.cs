using UnityEngine;
using System.Collections;

public class forceReceiver : MonoBehaviour
{
    public LayerMask forceLayer;

    private bool isTouched = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(forceLayer))
        {
            if (!isTouched)
            {
                Vector3 buff = transform.position;
                buff.x += 0.002f;
                transform.position = buff;
                isTouched = true;
            }
        } else
        {
            isTouched = false;
        }
    }
}
