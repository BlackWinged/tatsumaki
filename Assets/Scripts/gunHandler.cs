using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class gunHandler : MonoBehaviour
{
    public SpriteRenderer gun;
    public Transform shots;
    public float maxShotSpeed = 20;
    public float scatterPercent = 5;
    public float shotInterval = 0.8f;

    public enum Direction { Forward, Aft, Port, Starboard, ForwardPort, ForwardStarboard, AftPort, AftStarboard };
    public Direction direction;

    private delegate bool comparePosDel(Vector2 transPos, Vector2 mousePos, Transform trans);
    private float Timer = 0;
    private class angleRange
    {
        float upperLimit;
        float lowerLimit;
        private comparePosDel compareMethod;
        public angleRange(float upperLimit, float lowerLimit, comparePosDel compare)
        {
            this.upperLimit = upperLimit;
            this.lowerLimit = lowerLimit;
            compareMethod = compare;
        }
        public bool isCurrent(Vector2 transPos, Vector2 mousePos, Transform trans)
        {
            Vector2 direction = mousePos - transPos;
            float angle = Vector2.Angle(trans.up, direction);

            if ((angle >= upperLimit && angle <= lowerLimit) || (upperLimit == lowerLimit))
            {
                return compareMethod(transPos, mousePos, trans);
            }
            else
            {
                return false;
            }
        }

    }

    private Dictionary<Direction, angleRange> angles = new Dictionary<Direction, angleRange>()
    {
        {Direction.Forward, new angleRange(45, 45, (transPos, mousePos, trans) => {
            Vector2 direction = mousePos - transPos;
            float angle = Vector2.Angle(trans.up, direction);
            return (angle < 45);
        }) },
        {Direction.Aft, new angleRange(135, 135, (transPos, mousePos, trans) => {
            Vector2 direction = mousePos - transPos;
            float angle = Vector2.Angle(trans.up, direction);
            return (angle > 135);
        })},
        {Direction.Port, new angleRange(45,135, (transPos, mousePos, trans) => {return transPos.x > mousePos.x;})},
        {Direction.Starboard, new angleRange(45,135, (transPos, mousePos, trans) => {return transPos.x < mousePos.x;})}
    };

    private SpriteRenderer gunInstance = new SpriteRenderer();
    // Use this for initialization
    void Start()
    {
        if (gun != null)
        {
            gunInstance = Instantiate(gun);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gunInstance != null)
        {
            gunInstance.transform.position = transform.position;
            gunInstance.transform.rotation = transform.rotation;
        }
        if (GetComponentInParent<playerInput>().touch)
        {
            if (angles[direction].isCurrent(transform.position, GetComponentInParent<playerInput>().positions[0], transform))
            {
                Timer -= Time.deltaTime;

                if (Timer <= 0)
                {
                    fire();
                    Timer = shotInterval;
                }
            }
        }
    }

    Quaternion rightfaceRotate(Vector3 direction)
    {
        return Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
    }

    void fire()
    {
        Transform shot = (Transform)Instantiate(shots);
        shot.position = transform.position;
        Vector2 newPos = GetComponentInParent<playerInput>().positions[0];
        float distance = Vector2.Distance(GetComponentInParent<playerInput>().positions[0], transform.position);
        distance = distance * (scatterPercent / 100);
        newPos += (Random.insideUnitCircle * distance);
        Vector2 velocity = newPos - (Vector2)transform.position;
        shot.rotation = rightfaceRotate(velocity);
        shot.GetComponent<Rigidbody2D>().velocity = (velocity * 300 ) ;
        shot.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(shot.GetComponent<Rigidbody2D>().velocity, maxShotSpeed + GetComponentInParent<Rigidbody2D>().velocity.magnitude);
        shot.GetComponent<Rigidbody2D>().velocity += GetComponentInParent<Rigidbody2D>().velocity;
        shot.GetComponent<LaserCollisionHandler>().firedFrom = transform.parent.gameObject;
    }
}
