using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class StopWatch : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField]private bool isRunning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    public void StartStopwatch()
    {
        isRunning = true;
    }
    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        elapsedTime = 0f;
        isRunning = false;
        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
       

        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        timeDisplay.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);

        Debug.Log(timeDisplay.text);

    }
}
