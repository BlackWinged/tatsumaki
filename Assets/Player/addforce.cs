using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class addforce : MonoBehaviour
{
    public float smoothingThreshold = 0.05f;
    public forceFieldHandler wallCollider;
    public BoxCollider2D areaEffector;
    public CircleCollider2D pointEffector;

    public bool canSetWall = true;
    public bool canStraightAreaAffect = true;
    public bool canPointEffect = false;

    public float pointEffectForce = 10f;
    public float areaEffectForce = 10f;
    public Text stateText;


    private float drawSubjectLength;
    private float affectorSize;
    private bool isWallSet = true;
    private List<Vector2> wallPoints = new List<Vector2>();
    private playerInput controls;
    private List<Collider2D> colliderList = new List<Collider2D>();
    private List<Collider2D> colliderListArea = new List<Collider2D>();
    private bool hasSetEffector = false;
    // Use this for initialization
    void Start()
    {
        controls = GetComponent<playerInput>();
        affectorSize = areaEffector.GetComponent<SpriteRenderer>().bounds.size.x;

        colliderList.Add(pointEffector);
        colliderList.Add((Collider2D)Instantiate(pointEffector));
        colliderList[1].transform.parent = transform;
        colliderList[1].transform.localScale = colliderList[0].transform.localScale;

        colliderListArea.Add(areaEffector);
        colliderListArea.Add((Collider2D)Instantiate(areaEffector));
        colliderListArea[1].transform.parent = transform;
        colliderListArea[1].transform.localScale = colliderListArea[0].transform.localScale;
        colliderListArea[1].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motionHandle();
        setState();
        if (canSetWall)
        {
            defineWall();
            setWall();
        }

        defineArea();

        if (canPointEffect)
        {
            pointEffect();
        }

    }
    void defineArea()
    {
        foreach (BoxCollider2D collider in colliderListArea)
        {
              if (canStraightAreaAffect && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)))
            //if (canStraightAreaAffect)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    collider.GetComponent<AreaEffector2D>().forceMagnitude = areaEffectForce;
                }
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    collider.GetComponent<AreaEffector2D>().forceMagnitude = areaEffectForce * -1;
                }

                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                if (!hasSetEffector)
                {
                    collider.gameObject.SetActive(true);
                    Vector2 targetPoint = new Vector2();
                    targetPoint = controls.positions[0] + (controls.positions[0] - transform.position) * 5;
                    collider.transform.localScale = new Vector3(1, 3, 1);
                    drawBetweenPoints(transform.position, targetPoint, collider.gameObject, affectorSize);
                    if (collider.Equals(colliderListArea[1]))
                    {
                        hasSetEffector = true;
                    }
                }

            }
            else
            {
                collider.gameObject.SetActive(false);
                hasSetEffector = false;
            }
        }
    }


    void pointEffect()
    {
        foreach (Collider2D collider in colliderList)
        {
            PointEffector2D effector = collider.GetComponent<PointEffector2D>();
            if (Input.GetKey(KeyCode.Mouse0))
            {
                effector.forceMagnitude = pointEffectForce;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                hasSetEffector = true;
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                effector.forceMagnitude = pointEffectForce * -1;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                hasSetEffector = true;
            }
            else
            {
                effector.forceMagnitude = 0;
                hasSetEffector = false;
            }
        }
        if (!colliderList[0].transform.position.Equals(controls.positions[0]))
        {
            colliderList[1].gameObject.SetActive(true);
            colliderList[1].transform.position = controls.positions[0];
            colliderList.Reverse();
        }
        else
        {
            colliderList[1].gameObject.SetActive(false);
        }

    }

    void defineWall()
    {
        if (controls.touch)
        {
            if (isWallSet)
            {
                wallPoints.Clear();
            }
            bool canAdd = true;
            foreach (Vector2 point in wallPoints)
            {
                if (Vector2.Distance(point, controls.positions[0]) < smoothingThreshold)
                {
                    canAdd = false;
                }
            }
            if (canAdd)
            {
                wallPoints.Add(controls.positions[0]);
            }
            if (wallPoints.Count == 0)
            {
                wallPoints.Add(controls.positions[0]);
            }
            isWallSet = false;
        }
    }

    void setWall()
    {
        if (!controls.touch && !isWallSet)
        {
            wallCollider.startWall(wallPoints);
            isWallSet = true;
        }
    }

    void motionHandle()
    {
        if (!hasSetEffector)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * Input.GetAxis("Horizontal") * 10);
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * Input.GetAxis("Vertical") * 10);
        }
        else if (canStraightAreaAffect)
        {
            Vector3 scale = colliderListArea[0].transform.localScale;
            scale = new Vector3(scale.x, scale.y + Input.GetAxis("Vertical") /15 , scale.z);

            if (!colliderListArea[0].transform.localScale.Equals(scale))
            {
                colliderListArea[1].gameObject.SetActive(true);
                colliderListArea[1].transform.localScale = scale;
                colliderListArea.Reverse();
            }
            else
            {
                colliderListArea[1].gameObject.SetActive(false);
            }

        }

    }

    Quaternion rightfaceRotate(Vector3 direction)
    {
        return Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
    }

    void drawBetweenPoints(Vector3 from, Vector3 to, GameObject drawSubject, float drawSubjectLength)
    {
        Vector3 halfwayPoint = Vector3.Lerp(from, to, 0.5f);
        drawSubject.transform.position = halfwayPoint;
        float scaleCoef = Vector3.Distance(from, to) / drawSubjectLength;
        Vector3 scaledSize = drawSubject.transform.localScale;
        scaledSize.x = scaleCoef;
        drawSubject.transform.localScale = scaledSize;
        Vector3 direction = from - to;
        drawSubject.transform.rotation = rightfaceRotate(direction);

    }

    void drawBox2DBetweenPoints(Vector3 from, Vector3 to, GameObject drawSubject, float drawSubjectLength)
    {
        Vector2 halfwayPoint = Vector2.Lerp(from, to, 0.5f);
        drawSubject.transform.position = halfwayPoint;
        float scaleCoef = Vector2.Distance(from, to);
        Vector2 scaledSize = drawSubject.GetComponent<BoxCollider2D>().size;
        scaledSize.x = scaleCoef * 3;
        drawSubject.GetComponent<BoxCollider2D>().size = scaledSize;
        Vector2 direction = from - to;
        drawSubject.transform.rotation = rightfaceRotate(direction);

    }

    void setState()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            canStraightAreaAffect = true;
            canPointEffect = false;
            canSetWall = false;
            stateText.text = "Area Effect";
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            canStraightAreaAffect = false;
            canPointEffect = true;
            canSetWall = false;
            stateText.text = "Point Effect";
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            canStraightAreaAffect = false;
            canPointEffect = false;
            canSetWall = true;
            stateText.text = "Set Wall";
        }
    }
}
