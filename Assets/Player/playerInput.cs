using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class playerInput : MonoBehaviour
{
    public bool touch = false;
    public int scroll = 0;
    public float scrollCoef = 3;
    public float scrollThreshold = 1;
    public Vector3[] positions = new Vector3[4];
    public Vector3[] screenPositions = new Vector3[4];

    public Button indicator;
    private Vector3[] touchDownPositions = new Vector3[4];
    private bool touchDown = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckTouchAndPosition();
        ScrollDetect();
    }

    private void ScrollDetect()
    {

        if (Input.GetKey(KeyCode.W))
        {
            scroll = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            scroll = -1;
        }
        else
        {
            scroll = 0;
        }
        if (!Input.mousePresent)
        {
            if (!touchDown && touch)
            {
                scroll = 1;
                touchDown = true;
                //touchDownPositions[0] = screenPositions[0];
                //Vector2 indicPos;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(indicator.transform.parent.GetComponent<RectTransform>(), screenPositions[0], Camera.main, out indicPos);
                //indicator.GetComponent<Image>().enabled = true;
                //indicator.GetComponent<RectTransform>().localPosition = indicPos;
                //indicator.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollThreshold * 2 - 10, scrollThreshold * 2 - 10);
            }
            if (!touch)
            {
                //touchDown = false;
                scroll = 0;
                //indicator.GetComponent<Image>().enabled = false;
            }
            if (touch && touchDown)
            {
             //   if (Vector2.Distance(touchDownPositions[0], screenPositions[0]) > scrollThreshold)
              //  {
                       scroll = 1;
              //  }
            }
        }

    }
    private void CheckTouchAndPosition()
    {
        touch = false;
        if (Input.mousePresent)
        {
            if (Input.GetMouseButton(0))
            {
                touch = true;
            }
            positions[0] = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            positions[0].z = 0;
            screenPositions[0] = Input.mousePosition;
        }
        else
        {
            if (Input.touchCount > 0)
            {
                touch = true;
                int max = 4;
                if (Input.touchCount < 5)
                {
                    max = Input.touchCount - 1;
                }
                for (int i = 0; i <= max; i++)
                {
                    positions[i] = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    screenPositions[i] = Input.GetTouch(i).position;
                }
            }
        }
    }
}

