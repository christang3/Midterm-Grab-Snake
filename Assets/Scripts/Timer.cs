using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private static Timer instance;
    private GameObject TimerO;
    private TextMeshProUGUI TimerT;
    private float DefaultTimer = 45;
    private float CurrentTimer;
    // Start is called before the first frame update
    private Snake snake;

    public void Setup(Snake snake) {
        this.snake = snake;
    }
    private void Awake()
    {
        instance = this;
        TimerO = transform.Find("Timer").gameObject;
        Debug.Log(TimerO);
        TimerT = TimerO.GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer()
    {
        CurrentTimer = DefaultTimer;
        TimerO.SetActive(true);
    }

    public void StopTimer()
    {
        TimerO.SetActive(false);
    }

    public void ReduceTimer(float ReduceBy)
    {
        CurrentTimer -= ReduceBy;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerO.activeSelf)
        {
            CurrentTimer -= Time.deltaTime;
            TimerT.SetText(CurrentTimer.ToString("#"));
            if (CurrentTimer <= 0)
            {
                Debug.Log("Over");
                snake.TimeToDie();
            }
        }
    }
}
