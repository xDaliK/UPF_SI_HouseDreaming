using System.Collections;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    public float timeStart = 10;
    private TextMeshPro timerText;

    // Initializes the timer and starts the countdown.
    void Start()
    {
        timerText = GetComponent<TextMeshPro>();
        timerText.text = timeStart.ToString();
        StartCoroutine(CountdownToZero());
    }

    // This coroutine handles the countdown to zero.
    IEnumerator CountdownToZero()
    {
        while (timeStart > 0)
        {
            yield return new WaitForSeconds(1f);
            timeStart--;
            timerText.text = Mathf.Round(timeStart).ToString();
        }
    }
}
