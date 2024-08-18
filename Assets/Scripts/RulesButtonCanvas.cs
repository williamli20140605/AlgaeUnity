using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulesButtonCanvas : MonoBehaviour
{
    public Button backButton;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void EnableRulesButton()
    {
        print("EnableRulesButton");
        backButton.enabled = true;
        gameObject.SetActive(true);
    }
}
