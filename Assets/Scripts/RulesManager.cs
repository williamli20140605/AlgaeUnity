using UnityEngine;
using UnityEngine.UI;

public class RulesManager : MonoBehaviour
{
    public Canvas thisCanvas;        // The main canvas that can be shown or hidden
    public Canvas[] ruleCanvases;    // Array to hold canvases for each rule image

    private int currentRuleIndex = 0; // Index of the current rule

    public Button backButton;
    public Button leftButton;
    public Button rightButton;


    void Start()
    {
        // Initialize rule display
        UpdateRuleDisplay();
    }

    void UpdateRuleDisplay()
    {
        // Hide all canvases
        foreach (var canvas in ruleCanvases)
        {
            canvas.gameObject.SetActive(false);
        }

        // Display the current rule canvas
        if (ruleCanvases.Length > 0)
        {
            ruleCanvases[currentRuleIndex].gameObject.SetActive(true);
        }
    }

    public void OnNextButtonClicked()
    {
        if (currentRuleIndex < ruleCanvases.Length - 1)
        {
            currentRuleIndex++;
            UpdateRuleDisplay();
        }
    }

    public void OnPrevButtonClicked()
    {
        if (currentRuleIndex > 0)
        {
            currentRuleIndex--;
            UpdateRuleDisplay();
        }
    }

    public void OnBackButtonClicked()
    {
        thisCanvas.gameObject.SetActive(false); // Hide the main canvas

        if (ruleCanvases.Length > 0)
        {
            foreach (var canvas in ruleCanvases)
            {
                canvas.gameObject.SetActive(false);
            }
        }
        backButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);

        UpdateRuleDisplay();


    }

    public void OnRulesButtonClick()
    {
        thisCanvas.gameObject.SetActive(true); // Show the main canvas
        if (ruleCanvases.Length > 0)
        {
            foreach (var canvas in ruleCanvases)
            {
                canvas.gameObject.SetActive(true);
            }
        }
        backButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
        backButton.transform.SetAsLastSibling();
        leftButton.transform.SetAsLastSibling();
        rightButton.transform.SetAsLastSibling();
        //thisCanvas.enabled = true;
    }
}
