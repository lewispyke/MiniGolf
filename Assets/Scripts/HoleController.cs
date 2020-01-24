using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

public class HoleController : MonoBehaviour
{
    static List<ScorecardEntry> scorecard = new List<ScorecardEntry>();
    static int holeNumber = 0;
    private int numOfShots;

    public Camera mainCamera;
    public ParticleSystem ballSankEffect;

    // UI Elements:
    public TextMeshProUGUI shotText;

    public GameObject result;
    public TextMeshProUGUI resultText;

    public GameObject foul;
    public TextMeshProUGUI foulText;

    // preview:
    bool InPreview = true;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Hole Data"))
        {
            int par = GameObject.Find("Hole Data").GetComponent<HoleData>().Par();
            scorecard.Add(new ScorecardEntry(holeNumber, par, 0));
        }

        if (result)
            result.SetActive(false);

        if (foul)
            foul.SetActive(false);

        numOfShots = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera)
        {
            // Only check this until we finish with the preview
            if (mainCamera.GetComponent<Animator>())
            {
                if (!mainCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(
                    GameObject.Find("Hole Data").GetComponent<HoleData>().AnimationName()))
                {
                    InPreview = false;
                    mainCamera.GetComponent<Animator>().enabled = false;
                }
            }
        }
    }

    public void IncShotCount()
    {
        numOfShots++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        shotText.text = "Shots: " + numOfShots;
    }

    private void SetResultText()
    {
        resultText.text = DetermineResultString(numOfShots, scorecard[holeNumber].Par);
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

    public void BallSank()
    {
        ballSankEffect.Play();
        SetResultText();
        result.SetActive(true);
        scorecard[holeNumber].Shots = numOfShots;
        holeNumber++;
    }

    public IEnumerator DisplayFoul()
    {
        foulText.text = "Foul Shot";
        foul.SetActive(true);
        yield return new WaitForSeconds(1);
        foul.SetActive(false);
    }

    public bool InPreviewMode()
    {
        return InPreview;
    }

}