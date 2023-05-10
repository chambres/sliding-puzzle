using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerBehaviour : MonoBehaviour
{
    public GameObject timerGameObject;

    private TextMeshProUGUI _textMesh;
    private int _seconds = 0;
    private int _minutes = 0;

    public void Start() {
        GameManager.OnWonCondition += FreezeTimer;
        _textMesh = timerGameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(IncreaseTimer());
    }

    public IEnumerator IncreaseTimer() {
        yield return new WaitForSeconds(1);
        _seconds++;
        if (_seconds > 59) {
            _minutes++;
            _seconds = 0;
        }
        UpdateUIText();
        StartCoroutine(IncreaseTimer());
    }

    public void ResetTimer() {
        _minutes = 0;
        _seconds = 0;
        UpdateUIText();
    }

    public void UpdateUIText() {
        _textMesh.SetText(string.Format("{0:00}:{1:00}", _minutes, _seconds));
    }

    public void FreezeTimer() {
        StopCoroutine(IncreaseTimer());
    }
}
