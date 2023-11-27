using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToPos : MonoBehaviour
{

    enum Axis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Axis axis;
    [SerializeField] private RectTransform rectTransform;

    private void Start()
    {
        scrollbar.onValueChanged.AddListener((float val) => TranslatePos(val));
    }

    public void TranslatePos(float a)
    {
        Vector3 newPos = rectTransform.localPosition;
      
        float newCoord = 1731.0f * a;

        Mathf.Clamp(newCoord, 0, 1731.0f);

        switch (axis)
        {
            case Axis.X:
                newPos.x = newCoord;
                break;
            case Axis.Y:
                newPos.y = newCoord;
                break;
            case Axis.Z:
                newPos.z = newCoord;
                break;
            default:
                break;
        }

        rectTransform.localPosition = newPos;
    }
}
