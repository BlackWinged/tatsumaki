using UnityEngine;
using System.Collections;

public class smallShipControl : MonoBehaviour
{
    public float mainThrustForce = 20;
    public float rotationalForce = 5;
    public float strafingForce = 15;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * Input.GetAxis("Vertical") * mainThrustForce, ForceMode2D.Force);
        GetComponent<Rigidbody2D>().AddTorque(Input.GetAxis("Horizontal") * -1 * rotationalForce, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.Q))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.right * -1 * strafingForce, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.E))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.right * strafingForce, ForceMode2D.Force);
        }
    }
}
