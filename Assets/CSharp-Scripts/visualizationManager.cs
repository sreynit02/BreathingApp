using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;


public class visualizationManager : MonoBehaviour
{
    private string filePath;

    public TMP_Text percentageText;
    public TMP_Text countdownText;
    public Image visualBar;
    public Image circleImage;
    private float force;
    private float maxForce = 50;

    public Image[] powerBars;


    public LineRenderer lineRenderer;
    private int maxPoints = 1500;
    private float yScale = 1f;
    private float xSpacing = 0.05f;
    public float zDepth = 0.0f;

    private float graphWidth = Screen.width;
    private float graphHeight = Screen.height;

    private List<Vector3> graphPoints = new List<Vector3>();

    public LineRenderer goalBar;
    public TMP_Text goalLabelText;


    public int totalTrials = 10;
    public float trialDuration = 30f;  
    private float trialTimer = 0f;     
    private int currentTrial = 0;      
    private bool isTrialRunning = false;  

    public Button startButton;
    public TMP_Text trialEndMessage;

    void Start()
    {

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartTrialManually);
        }

        force = 0;

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();

        }

        for (int i = 0; i < maxPoints; i++)
        {
            graphPoints.Add(new Vector3(i * xSpacing, 0, zDepth));
        }

        UpdateGraph();

    }

    void Update()
    {

        // Update the force value from WebSocketClient
        if (BreathingDataHandler.Instance != null && BreathingDataHandler.Instance.breathingData != null)
        {
            BreathingData data = BreathingDataHandler.Instance.breathingData;
            force = data.value;
        }


        // If a trial is running, update the timer and record data
        if (isTrialRunning)
        {
            trialTimer += Time.deltaTime;

            // Record the force data for this trial
            RecordForceData(force);

            // Track max force during the trial
            if (force > maxForce)
            {
                maxForce = force;
            }

            // Display the trial information
            Debug.Log("MaxForce in Update: " + maxForce);

            // Update UI elements
            percentageText.text = "Force: " + force + "N" + "     " + "Percent:" + (force / maxForce) * 100 + " %";

            // Update the bar fill amount and color
            barFiller();
            ColorChanger();
            circleFiller();

            addDataPoint(force);
            DrawHorizontalLine();

            int remainingTime = Mathf.CeilToInt(trialDuration - trialTimer);
            countdownText.text = "Time Left: " + remainingTime.ToString() + "s";

            // Stop the trial if it reaches the duration
            if (trialTimer >= trialDuration)
            {
                StopTrial();  // Automatically stop the trial after 30 seconds
            }
        }

    }



    // Function to create a new file for each trial
    void StartTrial()
    {
        if (currentTrial < totalTrials)
        {
            // Create a unique file path for each trial
            string trialFileName = "force_data_trial_" + (currentTrial + 1) + ".csv";
            filePath = Application.persistentDataPath + "/" + trialFileName;

            // If the file doesn't exist, create it and add headers
            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Timestamp,Force(N)"); // CSV headers
                }
            }

            // Start the trial
            isTrialRunning = true;
            trialTimer = 0f;  // Reset the timer for the new trial
            Debug.Log("Starting trial " + (currentTrial + 1) + ". Data will be saved to: " + filePath);

            // Re-enable visualizations and hide the trial end message
            EnableVisualizations();
            trialEndMessage.gameObject.SetActive(false);  // Hide message when the trial starts
        }
        else
        {
            Debug.Log("All trials completed.");
        }
    }

    // Function to stop the current trial
    void StopTrial()
    {
        if (isTrialRunning)
        {
            isTrialRunning = false;
            currentTrial++;  // Increment the trial count
            Debug.Log("Trial " + currentTrial + " stopped.");

            // Disable the visualizations (bars, line renderer, circle)
            DisableVisualizations();

            // Check if this is the last trial
            if (currentTrial == totalTrials)
            {
                // Show the final message
                ShowEndOfExperimentMessage();
            }
            else
            {
                // Show the trial end message for other trials
                ShowTrialEndMessage();
            }
        }
    }


    // Function to manually start the trial when the user clicks the button
    public void StartTrialManually()
    {
        if (!isTrialRunning)
        {
            StartTrial();
        }
    }

    // Function to record force data to CSV
    void RecordForceData(float force)
    {
        // Make sure we only write data if a trial is running
        if (isTrialRunning)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Append force data for this trial to the file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(timestamp + "," + force);
            }
            Debug.Log("Force data recorded for trial " + currentTrial);
        }
    }

    void DisableVisualizations()
    {
        // Disable the visual elements (bars, line renderer, circle)
        if (visualBar != null) visualBar.enabled = false;
        if (lineRenderer != null) lineRenderer.enabled = false;
        if (circleImage != null) circleImage.enabled = false;

        // Disable the power bars as well
        foreach (var bar in powerBars)
        {
            bar.enabled = false;
        }
    }

    void EnableVisualizations()
    {
        // Re-enable the visual elements (bars, line renderer, circle)
        if (visualBar != null) visualBar.enabled = true;
        if (lineRenderer != null) lineRenderer.enabled = true;
        if (circleImage != null) circleImage.enabled = true;

        // Re-enable the power bars as well
        foreach (var bar in powerBars)
        {
            bar.enabled = true;
        }
    }

    void ShowTrialEndMessage()
    {
        // Display the message at the end of each trial
        trialEndMessage.gameObject.SetActive(true);
        trialEndMessage.text = "A trial is over. You may click the start trial button to start another trial.";
    }

    void ShowEndOfExperimentMessage()
    {
        // Display the final message after the 10th trial
        trialEndMessage.gameObject.SetActive(true);
        trialEndMessage.text = "End of Experiment. Thank you for your participation!";
    }
    private float SimulatedData()
    {
        return Mathf.Sin(Time.time) * 10 + 10;
    }

    void circleFiller()
    {
        circleImage.fillAmount = Mathf.Lerp(circleImage.fillAmount, (force / maxForce), Time.deltaTime * 10f);

        Color circleColor = Color.Lerp(Color.red, Color.green, (force / maxForce));
        circleImage.color = circleColor;
    }
    
    void barFiller()
    {
        if (visualBar != null)
        {
            //visualBar.fillAmount = Mathf.Lerp(visualBar.fillAmount, (force / maxForce), Time.deltaTime * 10f);

            for (int i = 0; i < powerBars.Length; i++)
            {
                powerBars[i].enabled = !multipleBarDisplay(force, i);
            }

        }

    }

    void ColorChanger()
    {
        if (visualBar != null)
        {
            Color barColor = Color.Lerp(Color.red, Color.green, (force / maxForce));
            visualBar.color = barColor;
        }

    }

    //needed for the multiple bar functions
    bool multipleBarDisplay(float forceIn, int forceValue)
    {
        return ((forceValue * 5) >= forceIn);
    }

    public void addDataPoint(float newValue)
    {
        float y = newValue * yScale;

        print("Y: " + y);

        //Adjust x-coordinates to fit within the viewport
        float x = graphPoints.Count < maxPoints ? graphPoints.Count * xSpacing : (maxPoints - 1) * xSpacing;

        // Shift all existing points to the left when adding new points
        if (graphPoints.Count >= maxPoints)
        {
            for (int i = 0; i < graphPoints.Count; i++)
            {
                graphPoints[i] = new Vector3(graphPoints[i].x - xSpacing, graphPoints[i].y, graphPoints[i].z);
            }
            graphPoints.RemoveAt(0); // Remove the oldest point
        }

        // Add the new point
        graphPoints.Add(new Vector3(x, y, zDepth));



        UpdateGraph();
    }

    public void UpdateGraph()
    {
        //only use the last 'maxPoints'
        int count = Mathf.Min(maxPoints, graphPoints.Count);

        // Extract the points within the viewport
        Vector3[] viewportPoints = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            viewportPoints[i] = graphPoints[graphPoints.Count - count + i];
        }

        // Update the LineRenderer with the viewport points
        lineRenderer.positionCount = viewportPoints.Length;
        lineRenderer.SetPositions(viewportPoints);
    }

    private void DrawHorizontalLine()
    {
        Debug.Log("MaxForce: " + maxForce);
        float y = maxForce * yScale;
        Debug.Log("Y-Coordinate: " + y);

        Vector3 startpoint = new Vector3(0, y, zDepth);
        Vector3 endPoint = new Vector3(graphWidth, y, zDepth);

        goalBar.positionCount = 2;
        goalBar.SetPosition(0, startpoint);
        goalBar.SetPosition(1, endPoint);


        // Update the position of the label
        Vector3 labelPosition = goalBar.transform.TransformPoint(100, y+10, zDepth); // Adjust offset as needed
        goalLabelText.transform.position = labelPosition;

        //// Set the label text
        //goalLabelText.text = "Your Goal";
    }

}
