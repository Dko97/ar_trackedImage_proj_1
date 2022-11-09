/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject placeablePrefab;
    GameObject spawnedPrefab;
*//*    [SerializeField]
    GameObject RendererBtn;*//*
    [SerializeField]
    bool test;


    private void Start()
    {
        EventsController.instance.CubeState += CubeStateAction;
        EventsController.instance.SpawnCubeEvent += ActivateCube;
        EventsController.instance.ChangeCubePositionEvent += UpdateCubePosition;
        EventsController.instance.IsCubeActive += CurrentCubeState;
        EventsController.instance.CurrentCubePosition += CurrentCubePosition;
        spawnedPrefab = GameObject.Instantiate(placeablePrefab);
        spawnedPrefab.name = "TrackedCube";
        spawnedPrefab.SetActive(false);
        #region TestCode // Commented
*//*        if (test)
        {
            StartCoroutine(ActivateCubeRoutine(RendererBtn.transform.position));
        }*//*
        #endregion
    }

    bool CurrentCubeState()
    {
        if (spawnedPrefab != null)
        {
            return spawnedPrefab.activeSelf;
        }
        else
            return false;
    }
    Vector3 CurrentCubePosition()
    {
        if(spawnedPrefab!=null)
            return spawnedPrefab.transform.position;
        else
            return Vector3.zero;
    }
    void CubeStateAction(bool active)
    {
        spawnedPrefab?.SetActive(active);
    }

    void UpdateCubePosition(Vector3 position)
    {
        spawnedPrefab.transform.position = position;
    }

    void ActivateCube(Vector3 buttonPos)
    {
        StartCoroutine(ActivateCubeRoutine(buttonPos));
    }
    public IEnumerator ActivateCubeRoutine(Vector3 buttonPos)
    {
        //DebugText.text = "Inside setCubeActiveRoutine";
        float val = 0;
        Material mat = spawnedPrefab.GetComponent<MeshRenderer>().material;
        mat.SetFloat("_Effect", 1f);
        val = mat.GetFloat("_Effect");
        spawnedPrefab.SetActive(true);

        while (val >= 0.1f)
        {
            val -= 0.015f;
            mat.SetFloat("_Effect", val);
            //DebugText.text = "Inside while";
            yield return null;
        }

        mat.SetFloat("_Effect", 0);
        //DebugText.text = "Outside while";
        // line renderer part
        EventsController.instance.StartAddLineEvent(spawnedPrefab.transform.position,buttonPos);
        yield return null;
    }

    private void OnDisable()
    {
        EventsController.instance.CubeState -= CubeStateAction;
        EventsController.instance.SpawnCubeEvent -= ActivateCube;
        EventsController.instance.ChangeCubePositionEvent -= UpdateCubePosition;
        EventsController.instance.IsCubeActive -= CurrentCubeState;
        Destroy(spawnedPrefab);
    }
}
*/