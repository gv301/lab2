using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    enum State { Normal, Debug, Vision};
    private State state;
    private LineRenderer lineRenderer;
    public GameObject closestPickup;
    public GameObject[] pickups;
    public GameObject player;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI velocityText;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Vision;
        closestPickup = FindClosestPickup();
        player = GameObject.Find("Player");
        SetModeText();
        
    }

    // Update is called once per frame
    void Update()
    {
        closestPickup = FindClosestPickup();
        if (Input.GetKeyDown("space"))
        {
            ChangeState();
            SetModeText();
        }

        if (state == State.Normal)
        {
            Normal();
        }
        else if (state == State.Debug)
        {
            Debug();
        }
        else
        {
            Vision();
        }
        


    }
    private void Normal()
    {

    }

    private void Debug()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        RenderLine();
        SetClosestBlue();
        SetOthersWhite();
        SetDistanceText();
        SetVelocityText();
        SetPositionText();

    }

    private void Vision()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        RenderLine();
        SetClosestGreen();
        SetOthersWhite();
        SetDistanceText();
        SetVelocityText();
        SetPositionText();
        closestPickup.transform.LookAt(player.transform);
    }


    public GameObject FindClosestPickup()
    {
        pickups = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject pickup in pickups)
        {
            Vector3 diff = pickup.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = pickup;
                distance = curDistance;
            }
        }
        return closest;
    }

    public void RenderLine()
    {
        
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, closestPickup.transform.position);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    public void ChangeState()
    {
        if (state == State.Normal)
        {
            state = State.Debug;
        }
        else if (state == State.Debug)
        {
            state = State.Vision;
            clearScreen();
        }
        else
        {
            state = State.Normal;
            clearScreen();
        }
    }
    public void SetClosestBlue()
    {
        closestPickup.GetComponent<Renderer>().material.color = Color.blue;
    }

    public void SetClosestGreen()
    {
        closestPickup.GetComponent<Renderer>().material.color = Color.green;
    }
    public void SetOthersWhite()
    {
        foreach (GameObject pickup in pickups)
        {
            if (pickup != closestPickup)
            {
                pickup.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    public void SetAllWhite()
    {
        foreach (GameObject pickup in pickups)
        { 
                pickup.GetComponent<Renderer>().material.color = Color.white;

        }
    }

    private void SetModeText()
    {
        if (state == State.Normal)
        {
            modeText.text = "Mode: Normal";
        }
        else if (state == State.Debug)
        {
            modeText.text = "Mode: Debug";
        }
        else
        {
            modeText.text = "Mode: Vision";
        }

    }
    private void SetPositionText()
    {
        positionText.text = "Position: " + transform.position.ToString("0.0");
    }

    private void SetVelocityText()
    {
        velocityText.text = "Velocity: " + GetComponent<Rigidbody>().velocity.magnitude.ToString("0.0");

    }

    public void SetDistanceText()
    {
        distanceText.text = "Distance: " + Vector3.Distance(player.transform.position, closestPickup.transform.position).ToString("0.0");
    }

    private void ClearText()
    {
        positionText.text = "";
        velocityText.text = "";
        distanceText.text = "";
    }

    private void clearScreen()
    {
        lineRenderer.SetPosition(0, new Vector3(500,0,0));
        lineRenderer.SetPosition(1, new Vector3(500, 0, 0));
        SetAllWhite();
        ClearText();
    }


}
