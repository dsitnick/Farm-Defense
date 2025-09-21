using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbar : MonoBehaviour {

    public RectTransform toolIndicator;
    public float incrementWidth = 100;

    public void SetToolIndex(int toolIndex) {
        toolIndicator.gameObject.SetActive(toolIndex >= 0);
        if (toolIndex >= 0) {
            toolIndicator.anchoredPosition = Vector2.right * incrementWidth * toolIndex;
        }
    }

}
