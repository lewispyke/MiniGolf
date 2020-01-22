using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BallController : MonoBehaviour
{
    private HoleController holeController;

    public float power;
    private float maxPower;
    private float minPower;
    private float powerInc;
    private int powerDir;

    public Transform arrow;
    private float turnSpeed;
    private bool swingStarted;
    private bool ballHit;

    private Vector3 lastPosition;
    private int numOfShots;


    private void Awake()
    {
        holeController = GameObject.FindObjectOfType<HoleController>();
    }

    void Start()
    {
        power = 100f;
        turnSpeed = 0.5f;

        maxPower = 1500f;
        minPower = 10f;
        powerInc = 750f;
        powerDir = 1;

        numOfShots = 0;

        SetArrowScale();

        swingStarted = false;
        ballHit = false;
    }

    void Update()
    {
        if (!holeController.InPreviewMode())
        {
            if (!ballHit)
            {
                if (swingStarted)
                {
                    power += powerInc * Time.deltaTime * powerDir;
                    if (power >= maxPower)
                    {
                        power = maxPower;
                        powerDir = -1;
                    }
                    else if (power <= minPower)
                    {
                        power = minPower;
                        powerDir = 1;
                    }
                    SetArrowScale();
                }
                else
                {
                    if (Input.GetKey("a"))
                    {
                        transform.Rotate(0, -turnSpeed, 0);
                    }

                    if (Input.GetKey("d"))
                    {
                        transform.Rotate(0, turnSpeed, 0);
                    }

                    if (Input.GetKey("w"))
                    {
                        transform.Rotate(-turnSpeed, 0, 0);
                    }

                    if (Input.GetKey("s"))
                    {
                        transform.Rotate(turnSpeed, 0, 0);
                    }
                }
            }
            else
            {
                if (transform.position.y < -10)
                {
                    FoulShot();
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (!ballHit)
        {
            if (swingStarted)
            {
                // save position of last shot
                lastPosition = transform.position;

                // hit ball
                ballHit = true;
                holeController.IncShotCount();
                GetComponent<Rigidbody>().AddRelativeForce(0, 0, power);
                arrow.GetComponent<Renderer>().enabled = false;
                StartCoroutine(StopBall());
            }

            swingStarted = !swingStarted;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EndShot();
        arrow.GetComponent<Renderer>().enabled = false;
        holeController.BallSank();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Out Of Bound"))
        {
            FoulShot();
        }
    }

    IEnumerator StopBall()
    {
        yield return new WaitForSeconds(1f);

        float threshold = 0.25f;
        bool shouldStop = false;

        while (!shouldStop)
        {
            if (GetComponent<Rigidbody>().velocity.magnitude < threshold)
            {
                shouldStop = true;

                for (int i = 0; i < 5; i++)
                {
                    yield return new WaitForSeconds(0.2f);
                    if (GetComponent<Rigidbody>().velocity.magnitude > threshold)
                    {
                        shouldStop = false;
                        break;
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
        EndShot();
    }

    private void SetArrowScale()
    {
        float arrowZScale = power / 300f;

        arrow.GetComponent<Transform>().localScale = new Vector3
            (arrow.GetComponent<Transform>().localScale.x,
            arrow.GetComponent<Transform>().localScale.y,
            arrowZScale);


        arrow.GetComponent<Renderer>().material.SetColor("_Color", 
            new Color(1f, 1f - (power / maxPower), 1f - (power / maxPower)));
    }

    private void EndShot()
    {
        // The ball will no longer be hit if the shot was a foul, so check
        if (ballHit)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.localEulerAngles = new Vector3(0, 0, 0);
            arrow.GetComponent<Renderer>().enabled = true;
            power = 100f;
            powerDir = 1;
            ballHit = false;
            SetArrowScale();
        }
    }

    private void FoulShot()
    {
        transform.position = lastPosition;
        EndShot();
        StartCoroutine(holeController.DisplayFoul());
    }

    public bool BallIsHit()
    {
        return ballHit;
    }
}