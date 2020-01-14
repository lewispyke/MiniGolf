using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScorecardEntry
{
    public ScorecardEntry()
    {
    }

    public ScorecardEntry(int hole, int par, int shots)
    {
        this.Hole = hole;
        this.Par = par;
        this.Shots = shots;
    }

    public int Hole { get; set; }
    public int Par { get; set; }
    public int Shots { get; set; }
    public int Score() { return (Shots - Par); }
};

public class BallController : MonoBehaviour
{
    static int totalScore;
    static List<ScorecardEntry> scorecard = new List<ScorecardEntry>();

    static int holeNumber = 0;

    public float power;
    private float maxPower;
    private float minPower;
    private float powerInc;
    private int powerDir;


    public Camera mainCamera;
    public Transform arrow;
    private float turnSpeed;
    private bool swingStarted;
    private bool ballHit;
    private Vector3 lastPosition;
    private int numOfShots;
    public Text shotText;

    public ParticleSystem particleSystem;
    public GameObject result;
    public Text resultText;

    public GameObject foul;
    public Text foulText;

    void Start()
    {
        scorecard.Add(new ScorecardEntry(holeNumber, 2, 0));

        result.SetActive(false);
        foul.SetActive(false);

        power = 100f;
        turnSpeed = 0.5f;

        maxPower = 1000f;
        minPower = 10f;
        powerInc = 500f;
        powerDir = 1;

        numOfShots = 0;

        SetArrowScale();

        swingStarted = false;
        ballHit = false;
    }

    void Update()
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
        //mainCamera.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
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
                numOfShots++;
                UpdateUI();
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
        particleSystem.Play();
        arrow.GetComponent<Renderer>().enabled = false;
        SetResultText();
        result.SetActive(true);
        //totalScore += (numOfShots - 2);
        scorecard[holeNumber].Shots = numOfShots;
        //Debug.Log(totalScore);
        Debug.Log(scorecard[holeNumber].Shots);
        holeNumber++;
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
        yield return new WaitForSeconds(6);
        EndShot();
    }

    IEnumerator DisplayFoul ()
    {
        foulText.text = "Foul Shot";
        foul.SetActive(true);
        yield return new WaitForSeconds(1);
        foul.SetActive(false);
    }

    private void SetArrowScale()
    {
        float arrowZScale = power / 200f;

        arrow.GetComponent<Transform>().localScale = new Vector3
            (arrow.GetComponent<Transform>().localScale.x,
            arrow.GetComponent<Transform>().localScale.y,
            arrowZScale);


        arrow.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f - (power / maxPower), 1f - (power / maxPower)));
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
        StartCoroutine(DisplayFoul());
    }

    private void UpdateUI()
    {
        shotText.text = "Shots: " + numOfShots;
    }

    private void SetResultText()
    {
        resultText.text = DetermineResultString(numOfShots, 2);
    }

    private string DetermineResultString(int shots, int par)
    {
        int score = shots - par;

        if (score < -3)
        {
            return (score * -1) + " under par, this is too easy for you";
        }
        else if (score == -3)
        {
            return "Double eagle, DAYUMMM!";
        }
        else if (score == -2)
        {
            return "Eagle, WOW!";
        }
        else if (score == -1)
        {
            return "Birdie, nice";
        }
        else if (score == 0)
        {
            return "Par, good job";
        }
        else if (score == 1)
        {
            return "Bogey, unlucky";
        }
        else if (score == 2)
        {
            return "Double bogey, get'um next time";
        }
        else if (score == 3)
        {
            return "Triple bogey, YIKES";
        }
        else if (score == 4)
        {
            return "Quadruple bogey, oh no...";
        }
        else if (score == 5)
        {
            return "Quintuple bogey, thats not gone well";
        }
        else if (score == 6)
        {
            return "Sextuple druple bogey, try a different sport maybe?";
        }
        else if (score > 6)
        {
            return score + " over par, I have nothing to say...";
        }

        string resultString = "---";
        return resultString;
    }

    public List<ScorecardEntry> GetScorecard()
    {
        return scorecard;
    }
}