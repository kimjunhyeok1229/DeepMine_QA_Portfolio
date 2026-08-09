using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] protected Button[] buttons;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Toggle fullscreen;
    [SerializeField] RectTransform helpPopUp;
    [SerializeField] RectTransform exitPopUp;
    [SerializeField] RectTransform settingPopup;
    [SerializeField] Image panel;    

    public Button selectedButton = null;
    public int selectedNumber = -1;
    int buttonCount;
    Color selectColor;
    ScreenResolution resolutionManager;

    protected virtual void Awake()
    {
        resolutionManager = GameManager.instance.GetComponent<ScreenResolution>();

        buttonCount = buttons.Length;
        selectColor = new Color(1, 0, 1);

        for (int i = 0; i < buttonCount; i++)
        {
            string buttonName = buttons[i].name;
            buttons[i].onClick.AddListener(() => { OnClickButton(buttonName); });
        }

        int count = resolutionManager.Resolutions.Count;

        for(int i = 0; i < count; i++)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            Resolution resolution = resolutionManager.Resolutions[i];
            optionData.text = resolution.ToString();
            dropdown.options.Add(optionData);
            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                dropdown.value = i;
            }
        }

        dropdown.onValueChanged.AddListener(delegate { OnDropdownSelect(); });

        fullscreen.isOn = Screen.fullScreen;

        fullscreen.onValueChanged.AddListener(delegate { OnClickToggle(fullscreen.isOn); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && helpPopUp.gameObject.activeSelf == false && exitPopUp.gameObject.activeSelf == false)
        {
            if(settingPopup.gameObject.activeSelf == false)
            {
                settingPopup.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
            }
            else
            {
                settingPopup.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && helpPopUp.gameObject.activeSelf == true)
        {
            helpPopUp.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }        

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (helpPopUp.gameObject.activeSelf == true) return;

            if (selectedNumber == -1)
            {
                selectedNumber = 0;
                selectedButton = buttons[selectedNumber];
                selectedButton.image.color = selectColor;
            }
            else
            {
                selectedButton.image.color = Color.white;
                selectedNumber = Mathf.Clamp(--selectedNumber, 0, buttonCount - 1);
                selectedButton = buttons[selectedNumber];
                selectedButton.image.color = selectColor;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (helpPopUp.gameObject.activeSelf == true) return;
            if (selectedNumber == -1)
            {
                selectedNumber = 0;
                selectedButton = buttons[selectedNumber];
                selectedButton.image.color = selectColor;
            }
            else
            {
                selectedButton.image.color = Color.white;
                selectedNumber = Mathf.Clamp(++selectedNumber, 0, 3);
                selectedButton = buttons[selectedNumber];
                selectedButton.image.color = selectColor;
            }
        }

        if(exitPopUp.gameObject.activeSelf == true)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                if(selectedButton != null)
                    selectedButton.image.color = Color.white;
                selectedButton = buttons[4];
                selectedButton.image.color = selectColor;
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                if (selectedButton != null)
                    selectedButton.image.color = Color.white;
                selectedButton = buttons[5];
                selectedButton.image.color = selectColor;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance?.Play_Sfx(SFXList.Effect_Button);
            ButtonFunction(selectedButton.name);
        }        
    }

    void OnClickButton(string buttonName)
    {
        ButtonFunction(buttonName);
    }

    protected virtual void ButtonFunction(string buttonName)
    {
        selectedButton.image.color = Color.white;
        selectedButton = null;
        selectedNumber = -1;

        switch (buttonName)
        {
            case "Start":
                GameManager.instance.ChangeScene("SampleScene");
                break;

            case "Maker":
                GameManager.instance.ChangeScene("MakerScene");
                break;

            case "Exit":
                exitPopUp.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
                break;

            case "Help":
                helpPopUp.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
                break;

            case "Yes":
                Application.Quit();
                break;

            case "No":
                exitPopUp.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    void OnDropdownSelect()
    {
        Debug.Log("resolution changed");
        resolutionManager.SetResolution(resolutionManager.Resolutions[dropdown.value]);
    }

    void OnClickToggle(bool value)
    {
        Debug.Log("fullscreen");
        resolutionManager.SetFullScreen(value);
    }
}
