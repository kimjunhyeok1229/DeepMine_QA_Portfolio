using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    [SerializeField] Image panel = null;

    public void SceneChange(string sceneName)
    {
        StartCoroutine(FadeInOut(1.5f, sceneName));
    }

    IEnumerator FadeInOut(float time, string sceneName)
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(time / 100);
        Color color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
        while (true)
        {
            color.a += 0.01f;
            panel.color = color;
            yield return delay;

            if (color.a >= 1)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(sceneName);
                break;
            }
        }

        while (true)
        {
            color.a -= 0.01f;
            panel.color = color;
            yield return delay;

            if (color.a <= 0)
            {
                Destroy(gameObject);
                yield break;
            }
        }
    }
}
