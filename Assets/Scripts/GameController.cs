using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.XR.ARSubsystems;
using Unity.Jobs;
using UnityEngine.UI;
using UnityEditor;
using System.Net;
using System.IO;
using System.Text;



public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject placeablePrefab;
    [SerializeField]
    ARTrackedImageManager _arTrackedImageManager;
    [SerializeField]
    TextMeshProUGUI DebugText;
    [SerializeField]
    TextMeshProUGUI DebugText2;
    [SerializeField]
    Button RendererButton;
    [SerializeField]
    bool test;
    [SerializeField]
    Canvas canvas;
    Texture2D loadedTexture;
    RuntimeReferenceImageLibrary newLibrary;
    GameObject spawnedPrefab;

    
    bool coroutineRunning=false;

    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
        
    }

    private void Start()
    {
        spawnedPrefab = GameObject.Instantiate(placeablePrefab);
        spawnedPrefab.name = "TrackedCube";
        spawnedPrefab.SetActive(false);
        #region TestCode // Commented
        /*if (test)
        {
            StartCoroutine(setCubeActiveRoutine());
        }*/
        #endregion
    }


    #region ImageTrackingRegion 
    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            UpdateSpawnedObject(trackedImage);
        }
        foreach (var trackedImage in args.updated)
        {
            UpdateSpawnedObject(trackedImage);
        }
        foreach (var trackedImage in args.removed)
        {
            spawnedPrefab.SetActive(false);
        }
    }

    private void UpdateSpawnedObject(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState != TrackingState.Limited && trackedImage.trackingState != TrackingState.None)
        {
            // Spawn cube on recognizing the image
            if (!spawnedPrefab.activeSelf && !coroutineRunning)
            {
                StartCoroutine(setCubeActiveRoutine());
            }
            else
            {
                Vector3 position = trackedImage.transform.position;
                spawnedPrefab.transform.position = position;
                // move line renderer position
                EventsController.instance.StartAddLineEvent(spawnedPrefab.transform.position, RectTransformUtility.CalculateRelativeRectTransformBounds(canvas.transform, RendererButton.transform).center);
            }
        }
        else
        {
            spawnedPrefab.SetActive(false);
            EventsController.instance.SetLineState(false);
            coroutineRunning = false;
        }
    }

    #endregion

    
    public IEnumerator setCubeActiveRoutine()
    {
        DebugText.text = "Inside setCubeActiveRoutine";
        coroutineRunning = true;
        float val = 0;
        Material mat = spawnedPrefab.GetComponent<MeshRenderer>().material;
        mat.SetFloat("_Effect", 1f);
        val = mat.GetFloat("_Effect");
        spawnedPrefab.SetActive(true);

        while (val >= 0.1f)
        {
            val -= 0.015f;
            mat.SetFloat("_Effect", val);
            yield return null;
        }

        mat.SetFloat("_Effect", 0);
        // line renderer part
        EventsController.instance.StartAddLineEvent(spawnedPrefab.transform.position, RectTransformUtility.CalculateRelativeRectTransformBounds(canvas.transform, RendererButton.transform).center);
        EventsController.instance.SetLineState(true);
        yield return null;
    }
 

    #region ChangingImageRegion
    public void AddImage(int selection)
    {
        DebugText.text = "Downloading...";
        // Loading Image
        switch (selection)
        {
            case 0:
                GetTextureFromUrl("https://drive.google.com/uc?id=1BT3Z9kuL9v24PM_P_EFCSbaonumxwsLa&export=download", "Lion");
                break;
            case 1:
                GetTextureFromUrl("https://drive.google.com/uc?id=1O_Z56-SpNOBrCb5YTOAxWoGyqSbVaHcn&export=download", "Cat");
                break;
            case 2:
                GetTextureFromUrl("https://drive.google.com/uc?id=1VtlKgK-tf85Rg1emhYmP7PUwR44I2mPB&export=download","Skull");
                break;
            case 3:
                GetTextureFromUrl("https://drive.google.com/uc?id=1TEWEb8_qvv57QrwzFZguqgt8D0LSG30K&export=download", "Tat");
                break;
            default:
                GetTextureFromUrl("https://drive.google.com/uc?id=1BT3Z9kuL9v24PM_P_EFCSbaonumxwsLa&export=download", "Lion");
                break;
        }
        // Adding image to TrackedImageManager
        StartCoroutine(AddImageRoutine());
    }
    public IEnumerator AddImageRoutine()
    {
        yield return null;
        DebugText.text = "In Add image function for " + loadedTexture.name;
        try
        {
            newLibrary = _arTrackedImageManager.CreateRuntimeLibrary();
            if (newLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                var jobHandle = MutableRuntimeReferenceImageLibraryExtensions.ScheduleAddImageJob(mutableLibrary, loadedTexture, loadedTexture.name, 0.3f);
                while (!jobHandle.IsCompleted)
                {
                    DebugText.text = "Job in progress";
                }
                _arTrackedImageManager.referenceLibrary = newLibrary;
                EventsController.instance.SetLineState(false);
                DebugText.text = "Job completed for " + loadedTexture.name;
                
            }

        }
        catch (Exception e)
        {
            DebugText.text = "Exception is : " + e;
        }

    }
    #endregion

    #region Downloading texture from URL 
    public void GetTextureFromUrl(string url,string texName)
    {
        StartCoroutine(GetTextureFromUrlRoutine(url,texName));
    }

    public IEnumerator GetTextureFromUrlRoutine(string url,string textName)
    {
        WebClient m_downloadClient = new WebClient();
        System.Uri uri = new System.Uri(url);
        Texture2D tex = new Texture2D(2,2,TextureFormat.RGBA32,false);
        byte[] downloadedData = m_downloadClient.DownloadData(uri);
        tex.LoadImage(downloadedData);
        tex = duplicateTexture(tex);
        loadedTexture = tex;
        loadedTexture.name = textName;
        yield return null;
    }

    // To enable the Read/Write option on texture
    Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    // Tryouts

    /*public IEnumerator GetTextureFromLocal(string path)
    {

        if (System.IO.File.Exists(path))
        {
            //DebugText2.text = "File exists";
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            loadedTexture = texture;
        }

        yield return null;
    }*/

    /*    private void FileDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs args)
        {
            DebugText2.text = args.ProgressPercentage.ToString() + " %";
        }*/

    #endregion

    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }
}
