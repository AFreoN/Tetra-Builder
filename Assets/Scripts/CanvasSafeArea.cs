using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasSafeArea : MonoBehaviour
{
    public RectTransform[] allPanels;

    private Rect lastSafeArea = Rect.zero;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (lastSafeArea != Screen.safeArea)
        {
            lastSafeArea = Screen.safeArea;
            ApplySafeArea();
        }
    }

    void Start()
    {
        lastSafeArea = Screen.safeArea;
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        if (allPanels.Length > 0 && allPanels[0] == null)
        {
            return;
        }

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;      // Dividing by Screen Width
        anchorMin.y /= canvas.pixelRect.height;     //Dividing by Screen Height
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        for(int i = 0; i < allPanels.Length; i++)
        {
            allPanels[i].anchorMin = anchorMin;
            allPanels[i].anchorMax = anchorMax;
        }
    }
}