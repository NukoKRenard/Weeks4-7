using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModularSliderVisuals : MonoBehaviour
{
    Slider slider;
    public float value;
    public bool countUp = false;

    //event driven code
    public bool timerIsRunning = false;
    public UnityEvent TimeIsUp;

    public void SetDirection(bool isTimer)
    {
        //a timer counts up from 0
        //a healthbar counts down from max health
        countUp = isTimer;
    }
    public void SetupSlider(float maxValue)
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxValue;
        if (countUp)
        {
            slider.value = 0;
        }
        else
        {
            slider.value = maxValue;
        }
    }

    public void UpdateSlider(float currentValue)
    {
        slider.value = currentValue;
    }

    public void ShowSlider()
    {
        gameObject.SetActive(true);
    }

    public void HideSlider()
    {
        gameObject.SetActive(false);
    }

    //this is our event-driven code
    public void StartTimer()
    {
        timerIsRunning = true;
        ShowSlider();
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            slider.value += Time.deltaTime;

            if(slider.value >= slider.maxValue)
            {
                TimeIsUp.Invoke();
                StopTimer();
            }
        }
    }

    public void StopTimer()
    {
        timerIsRunning = false;
        slider.value = 0;
        HideSlider();
    }
}
