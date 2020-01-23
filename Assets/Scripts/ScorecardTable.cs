using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScorecardTable : MonoBehaviour
{
    private HoleController holeController;

    private Transform entryContainer;
    private Transform entryTemplate;

    private void Awake()
    {
        holeController = GameObject.FindObjectOfType<HoleController>();

        entryContainer = transform.Find("Score Card Entry Container");
        entryTemplate = entryContainer.Find("Score Card Entry Template");

        entryTemplate.gameObject.SetActive(false);

        int i = 0;
        float templateHeight = 40f;
        int parTotal = 0;
        int shotsTotal = 0;
        List<ScorecardEntry> scorecard = holeController.GetScorecard();
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
            entryTransform.Find("Entry Background").gameObject.SetActive(i % 2 == 0);
            i++;
            shotsTotal += entry.Shots;
            parTotal += entry.Par;
        }
        Transform entryTransformTotal = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransformTotal = entryTransformTotal.GetComponent<RectTransform>();
        entryRectTransformTotal.anchoredPosition = new Vector2(0, -templateHeight * (i + 0.4f));
        entryTransformTotal.Find("Hole Text").GetComponent<Text>().text = "TOTAL";
        entryTransformTotal.Find("Hole Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
        entryTransformTotal.Find("Shots Text").GetComponent<Text>().text = shotsTotal.ToString();
        entryTransformTotal.Find("Shots Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
        entryTransformTotal.Find("Par Text").GetComponent<Text>().text = parTotal.ToString();
        entryTransformTotal.Find("Par Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
        entryTransformTotal.Find("Score Text").GetComponent<Text>().text = (shotsTotal - parTotal).ToString();
        entryTransformTotal.Find("Score Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
        entryTransformTotal.gameObject.SetActive(true);
        entryTransformTotal.Find("Entry Background").gameObject.SetActive(true);
        entryTransformTotal.Find("Entry Background").gameObject.GetComponent<Image>().color = new Color(0.82f, 0.82f, 0.82f);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
