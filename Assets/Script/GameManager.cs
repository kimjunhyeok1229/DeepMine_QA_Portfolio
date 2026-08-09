using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject fadePrefab = null;
    private int score;
    public int highScore = 0;

    public int Score { get { return score; } set { score = value; UIManager.instance.SetScore(score); } }

    public static GameManager instance
    {
        get { return m_instance; }        
    }
    private static GameManager m_instance;

    private void Awake()
    {
        if (m_instance == null) m_instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        GameObject fadePanel = Instantiate(fadePrefab);
        DontDestroyOnLoad (fadePanel);
        fadePanel.GetComponent<FadePanel>().SceneChange(sceneName);
    }
}
