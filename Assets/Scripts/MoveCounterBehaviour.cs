using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveCounterBehaviour : MonoBehaviour
{
    public GameObject counterGameObject;
    private int _moveCount = 0;
    private TextMeshProUGUI textMesh;

    public void Start() {
        textMesh = counterGameObject.GetComponent<TextMeshProUGUI>();
        GameManager.OnTileMoved += IncreaseMoveCount;
        UpdateUIText();
    }

    public void IncreaseMoveCount() {
        _moveCount++;
        UpdateUIText();
    }

    public void UpdateUIText() {
        textMesh.SetText(_moveCount.ToString());
    }

    public void ResetCount() {
        _moveCount = 0;
        UpdateUIText();
    }

}
