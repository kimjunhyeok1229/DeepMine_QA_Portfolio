using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class ScreenResolution : MonoBehaviour
{
    [Header("Start Setting")]
    [SerializeField] Vector2Int screenResolution = new Vector2Int(1920, 1080);
    [SerializeField] bool fullScreen = true;
    [SerializeField] bool fixScreenRatio = true;

    Vector2Int screenRatio = Vector2Int.zero;
    List<Resolution> resolutions = new List<Resolution>();

    public List<Resolution> Resolutions => resolutions;

    private void Awake()
    {
        SetFullScreen(fullScreen);
        SetResolution(screenResolution.x, screenResolution.y);
        if(fixScreenRatio == true)
            screenRatio = GetScreenRatio(Screen.currentResolution);
        GetResolutions();
    }

    Vector2Int GetScreenRatio(Resolution resolution)
    {
        int a = resolution.width, b = resolution.height, c;

        while (b != 0)
        {
            c = a % b;
            a = b;
            b = c;
        }

        return new Vector2Int(resolution.width / a, resolution.height / a);
    }

    void GetResolutions()
    {
        Resolution[] list = Screen.resolutions;
        int count = list.Length;

        for(int i =0; i < count; i++)
        {
            if (list[i].refreshRate == 60)
            {
                if (fixScreenRatio == true)
                {
                    if (screenRatio == GetScreenRatio(list[i]))
                        resolutions.Add(list[i]);
                }
                else
                {
                    resolutions.Add(list[i]);
                }
            }            
        }
    }

    public void SetResolution(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    public void SetResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool _fullScreen)
    {
        Screen.fullScreen = _fullScreen;
    }
}
