using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsController : MonoBehaviour
{
    public static EventsController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    #region LineRendererEvents
    public event Action<Vector3,Vector3> AddLineEvent;
    public event Action<bool> LineState;
    public event Action AutoLineStateChange;
    
    public void StartAddLineEvent(Vector3 point1,Vector3 point2)
    {
        AddLineEvent?.Invoke(point1,point2);
    }

    public void SetLineState(bool selection)
    {
        LineState?.Invoke(selection);
    }

    public void AutoLineStateChangeEvent()
    {
        AutoLineStateChange?.Invoke();
    }
    #endregion

 /*   #region CubeRelatedEvents
    public event Action<bool> CubeState;
    public event Action<Vector3> SpawnCubeEvent;
    public event Action<Vector3> ChangeCubePositionEvent;
    public event Func<bool> IsCubeActive;
    public event Func<Vector3> CurrentCubePosition;

    public bool StartIsCubeActive()
    {
        bool result;
        return result = (bool)(IsCubeActive?.Invoke());
    }
    public Vector3 StartGetCurrentCubePosition()
    {
        Vector3 result;
        return result = (Vector3)(CurrentCubePosition?.Invoke());
    }
    public void StartCubeState(bool selection)
    {
        CubeState?.Invoke(selection);
    }
    public void StartSpawnCubeEvent(Vector3 position)
    {
        SpawnCubeEvent?.Invoke(position);
    }
    public void StartChangeCubePositionEvent(Vector3 position)
    {
        ChangeCubePositionEvent?.Invoke(position);
    }
    #endregion*/
}
