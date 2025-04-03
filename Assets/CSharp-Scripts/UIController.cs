using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    public Image Circle;
    public Image VerticalBar;
    public LineRenderer LineGraph;
    public LineRenderer GoalLine;
    public TMP_Dropdown visualizationDropdown;
    public TMP_Text goalText;


    // Update is called once per frame

    private void Start()
    {
        visualizationDropdown.onValueChanged.AddListener(dropDownMenu);
        dropDownMenu(visualizationDropdown.value);
    }
    public void dropDownMenu(int index)
    {
        switch (index)
        {
            case 0:
                Circle.gameObject.SetActive(false);
                VerticalBar.gameObject.SetActive(false);
                LineGraph.gameObject.SetActive(true);
                GoalLine.gameObject.SetActive(true);
                goalText.gameObject.SetActive(true);
                break;
          
            case 1:
                Circle.gameObject.SetActive(false);
                VerticalBar.gameObject.SetActive(true);
                LineGraph.gameObject.SetActive(false);
                GoalLine.gameObject.SetActive(false);
                goalText.gameObject.SetActive(false);
                break;
            case 2:
                Circle.gameObject.SetActive(true);
                VerticalBar.gameObject.SetActive(false);
                LineGraph.gameObject.SetActive(false);
                GoalLine.gameObject.SetActive(false);
                goalText.gameObject.SetActive(false);
                break;
        }
    }
}
