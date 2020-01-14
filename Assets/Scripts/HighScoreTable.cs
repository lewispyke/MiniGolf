using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private BallController ballController;

    private Transform entryContainer;
    private Transform entryTemplate;

    private void Awake()
    {
        ballController = GameObject.FindObjectOfType<BallController>();

        entryContainer = transform.Find("Score Card Entry Container");
        entryTemplate = entryContainer.Find("Score Card Entry Template");

        entryTemplate.gameObject.SetActive(false);

        int i = 0;
        float templateHeight = 40f;
        int parTotal = 0;
        int shotsTotal = 0;
        List<ScorecardEntry> scorecard = ballController.GetScorecard();
        foreach (ScorecardEntry entry in scorecard)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.Find("Hole Text").GetComponent<Text>().text = (entry.Hole + 1).ToString();
            entryTransform.Find("Shots Text").GetComponent<Text>().text = entry.Shots.ToString();
            entryTransform.Find("Par Text").GetComponent<Text>().text = entry.Par.ToString();
            entryTransform.Find("Score Text").GetComponent<Text>().text = entry.Score().ToString();
            entryTransform.gameObject.SetActive(true);
            i++;
            shotsTotal += entry.Shots;
            parTotal += entry.Par;
        }
        Transform entryTransformTotal = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransformTotal = entryTransformTotal.GetComponent<RectTransform>();
        entryRectTransformTotal.anchoredPosition = new Vector2(0, -templateHeight * i);
        entryTransformTotal.Find("Hole Text").GetComponent<Text>().text = "TOTAL";
        entryTransformTotal.Find("Shots Text").GetComponent<Text>().text = shotsTotal.ToString();
        entryTransformTotal.Find("Par Text").GetComponent<Text>().text = parTotal.ToString();
        entryTransformTotal.Find("Score Text").GetComponent<Text>().text = (shotsTotal - parTotal).ToString();
        entryTransformTotal.gameObject.SetActive(true);
    }

    private void AddEntryToScoreboard(ScorecardEntry entry, Transform entryTransform)
    {

    }
}
