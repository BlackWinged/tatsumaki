using UnityEngine;
using UnityEditor;
using System.Collections;

public class shipControl : MonoBehaviour {
    public Rigidbody2D cameraTracker;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody2D>().AddForce(transform.up * Input.GetAxis("Vertical"), ForceMode2D.Force);
        GetComponent<Rigidbody2D>().AddTorque(Input.GetAxis("Horizontal") * -1, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.Q))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.right * -1, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.E))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.right , ForceMode2D.Force);
        }
        if (cameraTracker != null)
        {
            cameraTracker.AddForce(transform.up * Input.GetAxis("Vertical"), ForceMode2D.Force);
            cameraTracker.AddTorque(Input.GetAxis("Horizontal") * -1, ForceMode2D.Force);
            if (Input.GetKey(KeyCode.Q))
            {
                cameraTracker.AddForce(transform.right * -1, ForceMode2D.Force);
            }
            if (Input.GetKey(KeyCode.E))
            {
                cameraTracker.AddForce(transform.right, ForceMode2D.Force);
            }
        }
    }
}
