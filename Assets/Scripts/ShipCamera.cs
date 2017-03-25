using UnityEngine;
using System.Collections;

public class ShipCamera : MonoBehaviour {
    public Transform ship;
    public float smoothingForRotation = 5;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 newRot = ship.rotation.eulerAngles;
        newRot.x = 0;
        newRot.y = 0;
        Quaternion targetRot = new Quaternion();
        targetRot.eulerAngles = newRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, smoothingForRotation * Time.deltaTime);
        Vector3 pos = new Vector3(ship.position.x, ship.position.y, -10);
        transform.position = pos;

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            Camera.main.orthographicSize += 1;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            Camera.main.orthographicSize -= 1;
        }
    }
}
