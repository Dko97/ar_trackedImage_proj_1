using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;


    // Subscribing to events
    private void Start()
    {
        EventsController.instance.AddLineEvent += AddLineAction;
        EventsController.instance.LineState += LineStateAction;
        EventsController.instance.AutoLineStateChange += EnableDisableLineRenderer;
    }

    #region LineRendererRegion
    public void setLineRenderer(Vector3 startPos, Vector3 endPos)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }

    public void EnableDisableLineRenderer()
    {
        if (lineRenderer != null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
            }
            else
                lineRenderer.enabled = true;
        }
    }
    void AddLineAction(Vector3 p1, Vector3 p2)
    {
        setLineRenderer(p1, p2);
    }

    void LineStateAction(bool selection)
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = selection;
        }
    }

    #endregion



    // Unsubscribing to events
    private void OnDisable()
    {
        EventsController.instance.AddLineEvent -= AddLineAction;
        EventsController.instance.LineState -= LineStateAction;
        EventsController.instance.AutoLineStateChange -= EnableDisableLineRenderer;
    }
}
