using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;




public class SpawnTile : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField]GameObject map;

    [SerializeField] Sprite[] nomalImages; 
    [SerializeField] Sprite[] specialImages; 
    [SerializeField] Sprite fakeImages; 

    public List<GameObject> Lines;
    public List<GameObject> nums;

    public LayerMask tilemask;
    public LayerMask playermask;
    public LayerMask monstermask;

    
    void Awake()
    {
        
        
        
    }
    private void Start()
    {
        for (int y = -10; y < 5; y++)
        {
            MakeLineTile(y);
        }
        MakeLineSetTile(5);
        UpdateSearch();

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.CircleCast(pos, 0.1f,Vector2.zero, 0, tilemask);
            Debug.Log(pos);
            Debug.DrawRay(pos, Vector2.down);
            if(hit.collider != null)
            {
                Debug.Log("1");
                hit.collider.GetComponent<Tile>().Making();
            }

        }        
    }

    PoolType RandomType()
    {
        int r = UnityEngine.Random.Range(0, 100);
        if(r < 30)
        {
            return PoolType.Nomal;
        }
        else if(r < 60)
        {
            return PoolType.Mine;
        }
        else if(r < 85)
        {
            return PoolType.Treasure;
        }
        else if (r < 92)
        {
            return PoolType.Shock;
        }
        else
        {
            return PoolType.Fake;
        }
    }

    //타일 랜덤 생성
    Tile RandomSpawnTile(Vector3Int pos)
    {
        int hp = UnityEngine.Random.Range(1, 4);
        PoolType type = RandomType();
        GameObject obj = PoolManager.instance.GetObj(type);
        obj.transform.position = pos + new Vector3(0.5f,0.5f,0);
        switch (type)
        {
            case PoolType.Nomal:
            case PoolType.Mine:
                obj.GetComponent<SpriteRenderer>().sprite = nomalImages[UnityEngine.Random.Range(0, nomalImages.Length)];
                break;
            case PoolType.Treasure:
            case PoolType.Shock:
                obj.GetComponent<SpriteRenderer>().sprite = specialImages[UnityEngine.Random.Range(0, specialImages.Length)];
                break;
            case PoolType.Fake:
                obj.GetComponent<SpriteRenderer>().sprite = fakeImages;
                break;
        }

        obj.SetActive(true);

        Tile tile = obj.GetComponent<Tile>();
        tile.hp = hp;
        return tile;
    }
    Tile SetTile(Vector3Int pos, PoolType type)
    {
        int hp = UnityEngine.Random.Range(1, 4);
        GameObject obj = PoolManager.instance.GetObj(type);
        obj.transform.position = pos + new Vector3(0.5f, 0.5f, 0);
        if (type != PoolType.Fake)
            obj.GetComponent<SpriteRenderer>().sprite = nomalImages[UnityEngine.Random.Range(0, nomalImages.Length)];
        obj.SetActive(true);

        Tile tile = obj.GetComponent<Tile>();
        tile.hp = hp;
        return tile;
    }

    public void MakeLineSetTile(int y, PoolType type = PoolType.Nomal)
    {
        Vector3Int pos = new Vector3Int(0, y, 0);
        GameObject emetyLine = PoolManager.instance.GetObj(PoolType.Line);
        emetyLine.SetActive(true);
        emetyLine.transform.position = new Vector3(10, y, 0);
        emetyLine.transform.parent = map.transform;
        emetyLine.name = "Line";
        for (pos.x = 0; pos.x < 20; pos.x++)
        {
            Tile tile = SetTile(pos, type);
            tile.gameObject.transform.parent = emetyLine.transform;
        }
        Lines.Add(emetyLine);
    }
    public void MakeLineTile(int y = -10)
    {
        Vector3Int pos = new Vector3Int(0, y, 0);
        int emety1 = UnityEngine.Random.Range(0, 20);
        int emety2 = (UnityEngine.Random.Range(1, 19) + emety1) % 20;
        GameObject emetyLine = PoolManager.instance.GetObj(PoolType.Line);
        emetyLine.SetActive(true);
        emetyLine.transform.position = new Vector3(10, y, 0);
        emetyLine.transform.parent = map.transform;
        emetyLine.name = "Line";
        for (pos.x = 0; pos.x < 20; pos.x++)
        {
            if ((emety1 == pos.x || emety2 == pos.x) )
            {
                Vector3 p = pos + new Vector3(0.5f, 0.5f, 0);
                if (UnityEngine.Random.Range(0, 100) < 30)
                {
                    GameObject obj = PoolManager.instance.GetObj(PoolType.monster);
                    obj.transform.position = p;
                    obj.SetActive(true);
                    obj.transform.parent = emetyLine.transform;
                }
                SearchMine(p, emetyLine.transform); 
            }
            else
            {
                Tile tile = RandomSpawnTile(pos);
                tile.gameObject.transform.parent = emetyLine.transform;
            }
        }
        pos.x = emety1;
        SearchMine(pos + new Vector3(0.5f,0.5f,0), emetyLine.transform);
        pos.x = emety2;
        SearchMine(pos + new Vector3(0.5f, 0.5f, 0), emetyLine.transform);


        Lines.Add(emetyLine);
        GameObject g = null;
        foreach (GameObject obj in Lines)
        {
            if (obj.transform.position.y >= 13)
            {
                
                foreach(Transform t in obj.GetComponentsInChildren<Transform>())
                {
                    t.gameObject.SetActive(false);
                    t.transform.parent = PoolManager.instance.gameObject.transform;
                }

                obj.transform.DetachChildren();
                obj.SetActive(false);
                g = obj;
            }
        }
        if(g != null)
            Lines.Remove(g);
    }




    public void MakingTile(Tiledata tile)
    {
        Vector3 vec = tile.pos + new Vector3(0.5f, 0.5f, 0);
        //마킹 이미지 옮기기
        Debug.Log(vec);
    }

    public int Mining(Tile tile, int p = 1)
    {
         tile.hp -= p;
        if(tile.hp <= 0)
        {
            DestroyTile(tile);
            return 0;
        }
        return tile.hp;
    }
    #region DestroyTiles
    public void DestroyTile(Tile tile)
    {
        //포커스 해제
        player.FocusClear();

        //파괴 효과
        EffectTile(tile);
    }


    public void EffectTile(Tile tile, bool isRange = false)
    {
        GameManager.instance.Score += 300;
        UpdateSearch();
        SearchMine(tile.transform);
        switch (tile.type)
        {
            case PoolType.Nomal:
                DestroyNomal(tile, isRange);
                break;
            case PoolType.Treasure:
                DestroyTreasure(tile);
                break;
            case PoolType.Mine:
                DestroyMine(tile);
                break;
            case PoolType.Fake:
                SearchMine(tile.transform);
                tile.gameObject.SetActive(false);
                break;
            case PoolType.Shock:
                DestroyShock(tile);
                break;
        }
    }
    public void SearchMine(Vector3 pos,Transform parent)
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(pos, Vector2.one * 2, 0, Vector2.zero, 0, tilemask);
        int count = 0;
        foreach (RaycastHit2D h in hit)
        {
            if (h.collider == null) continue;

            Tile tile = h.collider.GetComponent<Tile>();
            if (tile.type == PoolType.Mine)
            {
                count++;
            }
        }


        GameObject obj = PoolManager.instance.GetObj(PoolType.MineText);
        obj.GetComponent<TextMeshPro>().text = count == 0 ? "" : count.ToString();
        obj.transform.position = pos;
        obj.transform.parent = parent;
        obj.SetActive(true);
        nums.Add(obj);

    }
    public void SearchMine(Transform t)
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(t.position, Vector2.one * 2, 0, Vector2.zero, 0, tilemask);
        int count = 0;
        foreach (RaycastHit2D h in hit)
        {
            if (h.collider == null) continue;

            Tile tile = h.collider.GetComponent<Tile>();
            if (tile.type == PoolType.Mine)
            {
                count++;
            }
        }


        GameObject obj = PoolManager.instance.GetObj(PoolType.MineText);
        obj.GetComponent<TextMeshPro>().text = count == 0 ? "" : count.ToString();
        obj.transform.position = t.transform.position;
        obj.transform.parent = t.transform.parent;
        obj.SetActive(true);
        nums.Add(obj);

    }

    void UpdateSearch()
    {
        List<GameObject> list = new List<GameObject>();
        foreach(GameObject obj in nums)
        {
            list.Add(obj);
        }
        nums.Clear();
        foreach(GameObject obj in list)
        {
            obj.SetActive(false);
            SearchMine(obj.transform);
        }
    }
    
    void DestroyNomal(Tile tile, bool isRange)
    {
        tile.gameObject.SetActive(false);
        RaycastHit2D[] hit = Physics2D.BoxCastAll(tile.transform.position, Vector2.one * 2, 0, Vector2.zero ,0, tilemask);
        int count = 0;
        foreach (RaycastHit2D h in hit)
        {
            if (h.collider == null) continue;

            Tile t = h.collider.GetComponent<Tile>();
            if (t.type == PoolType.Mine)
            {
                count++;
            }
        }
        
        if(count == 0 && !isRange)
        {
            foreach (RaycastHit2D h in hit)
            {
                if (h.collider == null) continue;

                Tile t = h.collider.GetComponent<Tile>();
                EffectTile(t,true);
            }
        }
        
    }
    PoolType RandomItemType()
    {
        int r = UnityEngine.Random.Range(0, 100);
        if (r < 2)
        {
            return PoolType.bomb;
        }
        else if (r < 12)
        {
            return PoolType.Healpack;
        }
        else if (r < 20)
        {
            return PoolType.Shield;
        }
        else if(r < 30)
        {
            return PoolType.crown;
        }
        else if(r < 50)
        {
            return PoolType.diamond;
        }
        else
        {
            return PoolType.gold;
        }
        
    }
    IEnumerator DestroyChest(Tile tile)
    {
        yield return new WaitForSeconds(0.2f);
        tile.OpenChest();
        yield return new WaitForSeconds(1f);
        tile.gameObject.SetActive(false);
        PoolType type = RandomItemType();
        GameObject obj = PoolManager.instance.GetObj(type);
        obj.transform.position = tile.transform.position;
        obj.transform.parent = tile.transform.parent;
        obj.GetComponent<Item>().tilemanager = this;
        obj.SetActive(true);
    }
    void DestroyTreasure(Tile tile)
    {
        tile.gameObject.SetActive(false);
        GameObject obj = PoolManager.instance.GetObj(PoolType.TreasureChest);
        obj.transform.position = tile.transform.position;
        obj.SetActive(true);
        obj.transform.parent = tile.transform.parent;
        StartCoroutine(DestroyChest(obj.GetComponent<Tile>()));
    }

    void DestroyMine(Tile tile)
    {
        tile.gameObject.SetActive(false);
        Boom(tile.transform);
    }

    public void Boom(Transform t)
    {
        StartCoroutine(BoomRoutine(t));
    }
    IEnumerator BoomRoutine(Transform t)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(t.position, 2, Vector2.zero, 0, tilemask);
        Debug.Log("펑");
        foreach (RaycastHit2D h in hit)
        {
            if (h.collider == null) continue;

            h.collider.gameObject.SetActive(false);
        }
        foreach (RaycastHit2D h in hit)
            SearchMine(h.transform);
        GameObject boomEffect = PoolManager.instance.GetObj(PoolType.boom);
        boomEffect.transform.position = t.position;
        boomEffect.transform.parent = t.parent;
        boomEffect.SetActive(true);
        AudioManager.instance?.Play_Sfx(SFXList.Effect_Boom);
        RaycastHit2D playerhit = Physics2D.CircleCast(t.position, 3, Vector2.zero, 0, playermask);
        if (playerhit.collider != null)
        {
            //데미지
            playerhit.collider.GetComponent<Player>().Damaged();
        }
        hit = Physics2D.CircleCastAll(t.position, 2, Vector2.zero, 0, monstermask);
        foreach (RaycastHit2D h in hit)
        {
            if(h.collider == null) continue;

            h.collider.gameObject.SetActive(false);
        }
        UpdateSearch();
        yield return new WaitForSeconds(0.5f);
        boomEffect.SetActive(false);
    }
    void DestroyShock(Tile tile)
    {
        player.PushPlayer(tile.transform.position);
        tile.gameObject.SetActive(false);
    }



    #endregion
}
