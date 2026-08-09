using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum PoolType
{
    Nomal,
    Treasure,
    TreasureChest,
    Mine,
    Shock,
    Fake,
    Line,
    diamond,
    crown,
    gold,
    bomb,
    monster,
    boom,
    MineText,
    Make,
    Healpack,
    Shield
}


public class Tile : MonoBehaviour
{
    public int hp;
    public PoolType type;
    public bool isMaking;
    public Animator animator;
    GameObject flag; //¸¶Ä¿
    private void Awake()
    {
        if(type == PoolType.TreasureChest)
            animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        animator.SetTrigger("Open");
    }

    public void Making()
    {
        isMaking = !isMaking;
        AudioManager.instance?.Play_Sfx(SFXList.Effect_Mark);
        if (isMaking )
        {
            flag = PoolManager.instance.GetObj(PoolType.Make);
            flag.transform.parent = transform;
            flag.transform.position = transform.position;
            flag.SetActive(true);
        }
        else
        {
            flag.gameObject.SetActive(false);
            flag.transform.parent = PoolManager.instance.gameObject.transform;
            flag = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(type == PoolType.Fake && collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("TileManager").GetComponent<SpawnTile>().EffectTile(this);
        }
        
    }
}
