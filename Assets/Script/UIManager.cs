using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image focusImage;
    [SerializeField] Text mineCountText;
    [SerializeField] Player player;
    [SerializeField] List<GameObject> hpUI = new List<GameObject>();
    [SerializeField] GameObject shieldUI = null;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] RectTransform overPopup;
    [SerializeField] Button retry;
    [SerializeField] Button menu;
    [SerializeField] TextMeshProUGUI endScore;
    [SerializeField] TextMeshProUGUI highScore;
    Button focusButton = null;

    public GameObject ShieldUI { get { return shieldUI; } set { shieldUI = value; } }

    public static UIManager instance
    {
        get { return m_instance; }
    }
    private static UIManager m_instance;

    private void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(overPopup.gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (focusButton != null)
                {
                    focusButton.image.color = Color.white;
                }
                focusButton = retry;
                focusButton.image.color = new Color(1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (focusButton != null)
                {
                    focusButton.image.color = Color.white;
                }
                focusButton = menu;
                focusButton.image.color = new Color(1, 0, 1);
            }

            if (Input.GetKeyDown(KeyCode.Space) && focusButton != null)
            {
                AudioManager.instance?.Play_Sfx(SFXList.Effect_Button);
                GameManager.instance.Score = 0;
                if (focusButton == retry)
                {                    
                    GameManager.instance.ChangeScene("SampleScene");
                }
                else
                {
                    GameManager.instance.ChangeScene("TitleScene");
                }
            }
        }
    }

    public void SetScore(int _score)
    {
        score.text = _score.ToString();
    }

    public void moveFocusImage(Vector3Int pos)
    {
        focusImage.gameObject.SetActive(true);
        focusImage.transform.position = Camera.main.WorldToScreenPoint(pos + new Vector3(0.5f, 0.5f, 0));
    }

    public void FocusOff()
    {
        focusImage.gameObject.SetActive(false);
        
    }
    public void MineCount(int count, Vector3 pos)
    {
        mineCountText.text = count.ToString();
        mineCountText.transform.position = Camera.main.WorldToScreenPoint(pos);
    }

    public void HPUpdate(int hp)
    {
        int count = hpUI.Count;
        for(int i = 0; i < count; i++)
        {
            if(i<hp)
                hpUI[i].SetActive(true);
            else
                hpUI[i].SetActive(false);
        }

        if(hp==0) { OpenOverPopup(); }
    }

    public void OpenOverPopup()
    {
        AudioManager.instance?.Play_Sfx(SFXList.Effect_GameOver);
        int overScore = GameManager.instance.Score;
        int highscore = GameManager.instance.highScore;
        endScore.text = overScore.ToString();
        if (overScore > highscore)
        {
            GameManager.instance.highScore = overScore;
            highScore.text = overScore.ToString();
        }
        else
            highScore.text = highscore.ToString();
        
        overPopup.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
