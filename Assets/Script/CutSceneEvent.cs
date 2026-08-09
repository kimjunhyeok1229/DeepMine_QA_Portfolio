using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CutSceneEvent : MonoBehaviour
{
    [SerializeField] List<GameObject> cuts = new List<GameObject>();
    [SerializeField] float speed = 5f;

    int currentEvent = 0;
    float delay = 0f;
    Color color = Color.white;

    private void Start()
    {
        delay = 1f;
        color.a = 0f;
    }

    private void Update()
    {
        if(delay > 0f) { delay -= Time.deltaTime; return; }
        CutScene(currentEvent);
    }

    void CutScene(int num)
    {
        switch (num)
        {
            case 0:
                cuts[num].transform.position += speed * Vector3.down * Time.deltaTime;
                if (cuts[num].transform.position.y <= 0)
                {
                    cuts[num].transform.position = Vector3.zero;
                    currentEvent++;
                    delay = 1f;
                }
                break;

            case 1:
                cuts[num].transform.position += speed * Vector3.up * Time.deltaTime;
                if (cuts[num].transform.position.y >= 0)
                {
                    cuts[num].transform.position = Vector3.zero;
                    currentEvent++;
                    delay = 1f;
                }
                break;

            case 2:
                cuts[num].transform.position += speed * Vector3.down * Time.deltaTime;
                if (cuts[num].transform.position.y <= 0)
                {
                    cuts[num].transform.position = Vector3.zero;
                    currentEvent++;
                    delay = 1f;
                }
                break;

            case 3:
                color.a += 0.01f;
                cuts[num].GetComponent<SpriteRenderer>().color = color;
                if (color.a >= 1)
                {
                    cuts[num].GetComponent<SpriteRenderer>().color = Color.white;
                    color.a = 0f;
                    currentEvent++;
                    delay = 1f;
                    AudioManager.instance.Play_IntroSound(0);
                }
                break;

            case 4:
                color.a += 0.01f;
                cuts[num].GetComponent<SpriteRenderer>().color = color;
                if (color.a >= 1)
                {
                    cuts[num].GetComponent<SpriteRenderer>().color = Color.white;
                    color.a = 0f;
                    currentEvent++;
                    delay = 1f;
                }
                break;

            case 5:
                cuts[num].transform.position += speed * Vector3.down * Time.deltaTime;
                if (cuts[num].transform.position.y <= 0)
                {
                    cuts[num].transform.position = Vector3.zero;
                    currentEvent++;
                    delay = 1f;
                    AudioManager.instance.Play_IntroSound(1);
                }
                break;

            case 6:
                cuts[num].transform.position += speed * Vector3.left * Time.deltaTime;
                if (cuts[num].transform.position.x <= 0)
                {
                    cuts[num].transform.position = Vector3.zero;
                    GameManager.instance.ChangeScene("TitleScene");
                    delay += 10f;
                    AudioManager.instance.Play_IntroSound(2);
                }
                break;
        }
    }
}
