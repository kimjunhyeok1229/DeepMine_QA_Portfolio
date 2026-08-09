using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject tileMap;
    [SerializeField] SpawnTile tileManager;
    [SerializeField] UnityEngine.UI.Image background;

    float checkPoint = 5.9f;
    float scrollSpeed = 3f;
    float scrolledDistance = 0f;
    Vector2 bgOffset = Vector2.zero;

    bool isScrolling = false;

    private void Start()
    {
        background.material.mainTextureOffset = Vector2.zero;
        scrollSpeed *= player.GravityScale;
    }

    private void Update()
    {
        //if(player.transform.position.y < checkPoint && isScrolling == false)
        //{
        //    isScrolling = true;
        //}

        //if (isScrolling == true)
        //{
        //    //tileMap.transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        //    ScrollTileMap();
        //    scrolledDistance += scrollSpeed * Time.deltaTime;
        //    if (scrolledDistance >= 1)
        //    {
        //        //tileMap.transform.position = Vector3Int.FloorToInt(tileMap.transform.position);
        //        OrganizeTileMap();
        //        scrolledDistance = 0;
        //        isScrolling = false;
        //        tileManager.MakeLineTile();
        //    }
        //}

        if (player.transform.position.y < checkPoint)
        {
            ScrollTileMap();
            scrolledDistance += scrollSpeed * Time.deltaTime;
            if (scrolledDistance >= 1)
            {
                scrolledDistance = 0;
                OrganizeTileMap();
            }
        }
    }

    void ScrollTileMap()
    {
        List<GameObject> lines = tileManager.Lines;
        int count = lines.Count;
        for (int i = 0; i < count; i++)
        {
            lines[i].transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        }
        bgOffset.y -= (Time.deltaTime * 0.3f);
        background.material.mainTextureOffset = bgOffset;
    }

    void OrganizeTileMap()
    {
        List<GameObject> lines = tileManager.Lines;
        int count = lines.Count;
        for (int i = 0; i < count; i++)
        {
            lines[i].transform.position = Vector3Int.FloorToInt(lines[i].transform.position);
        }
        tileManager.MakeLineTile();
    }
}
