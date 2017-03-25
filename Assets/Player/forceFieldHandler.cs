using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class forceFieldHandler : MonoBehaviour
{
    public float lifetime = 5;
    public SpriteRenderer wallSegment;

    private float timer;
    private List<SpriteRenderer> wallSegments = new List<SpriteRenderer>();
    private float wallSegmentLength;
    // Use this for initialization
    void Start()
    {
        SpriteRenderer buff = Instantiate(wallSegment);
        wallSegmentLength = buff.bounds.size.x;
        Destroy(buff.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        handleLifetime();
    }

    public void startWall(List<Vector2> points)
    {
        if (points.Count > 1)
        {
            foreach (SpriteRenderer segment in wallSegments)
            {
                Destroy(segment.gameObject);
            }
            wallSegments.Clear();
            timer = lifetime;
            EdgeCollider2D edge = GetComponent<EdgeCollider2D>();
            edge.enabled = true;
            edge.points = points.ToArray();

            for (int i = 0; i < points.Count - 1; i++)
            {
                wallSegments.Add(Instantiate(wallSegment));
                Color col = new Color();
                col = wallSegment.color;
                col.a = 255;
                wallSegment.color = col;
                drawBetweenPoints(points[i], points[i + 1], wallSegments[wallSegments.Count - 1].gameObject, wallSegmentLength);
            }
        }
    }

    void handleLifetime()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            foreach (SpriteRenderer segment in wallSegments)
            {
                float ratio = timer / lifetime;
                Color col = new Color();
                col.a =ratio;
                segment.color = col;
            }
        }
        else
        {
            GetComponent<EdgeCollider2D>().enabled = false;
            foreach (SpriteRenderer segment in wallSegments)
            {
                Destroy(segment.gameObject);
            }
            wallSegments.Clear();
        }
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

    Quaternion rightfaceRotate(Vector3 direction)
    {
        return Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
    }
}
