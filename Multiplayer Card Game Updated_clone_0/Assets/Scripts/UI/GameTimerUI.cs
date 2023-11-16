using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    #region Events
    public delegate void TimerEnd();
    public static event TimerEnd OnTimerEnd;
    #endregion

    private float timeRemaining;
    private bool timerStarted;
    [SerializeField]
    private float totalTime = 6f;
    [SerializeField]
    private TMP_Text gameTimer;


    private void OnEnable()
    {
        PlayerSetupManager.OnPlayerSetUp += TimerStart;
    }

    private void OnDisable()
    {
        PlayerSetupManager.OnPlayerSetUp -= TimerStart;
    }

    private void Start()
    {

        timeRemaining = totalTime;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.currentState == GameManager.GameState.Start) return;
        if (timerStarted) { TimerCountDown(); }
    }

    private void TimerStart()
    {
        timerStarted = true;
    }

    private void TimerCountDown()
    {
        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            TimerUI();
        }
        else
        {
            string timeString = "00:00";

            gameTimer.text = timeString;
            OnTimerEnd?.Invoke();
        }
    }

    private void TimerUI()
    {
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

            gameTimer.text = timeString;
        }
    }
}
