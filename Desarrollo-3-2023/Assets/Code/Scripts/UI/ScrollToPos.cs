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
    [SerializeField] private bool negative = false;
    [SerializeField] private float maxValue;
    [SerializeField] private float minValue;

    private void Start()
    {
        scrollbar.onValueChanged.AddListener((float val) => TranslatePos(val));
    }

    public void TranslatePos(float a)
    {
        Vector3 newPos = rectTransform.localPosition;

        float newCoord;

        if (negative)
            newCoord = maxValue * (1 - a);
        else
            newCoord = maxValue * a;

        Mathf.Clamp(newCoord, minValue, maxValue);

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
