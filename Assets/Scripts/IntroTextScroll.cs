using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroTextScroll : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public float scrollSpeed = 25f;
    private float textYPosition;
    private bool isScrolling = true;
    public Canvas introCanvas;
    public Canvas introBG;

    public CheatManager cheatManager;

    private bool canScroll = true;

    void Start()
    {
        canScroll = true;
        cheatManager = GameObject.Find("CheatManager").GetComponent<CheatManager>();
        textYPosition = -introText.rectTransform.rect.height - 150; // Start below the screen
        StartCoroutine(ScrollText());
    }

    IEnumerator ScrollText()
    {
        while (canScroll)
        {
            Invoke("StopScrolling", 10f);
            if (textYPosition >= Screen.height + 300)
            {
                isScrolling = false;
            }
            
            if (isScrolling)
            {
                textYPosition += scrollSpeed * Time.deltaTime;
                introText.rectTransform.anchoredPosition = new Vector2(introText.rectTransform.anchoredPosition.x, textYPosition);
            }

            if (cheatManager.isCheating)
            {
                canScroll = false;
                break;
            }

            yield return null;
        }

        introCanvas.enabled = false;
        introBG.enabled = false;
    }

    public void StopScrolling()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp (KeyCode.Escape))
        {
            canScroll = false;
        }
    }
}
