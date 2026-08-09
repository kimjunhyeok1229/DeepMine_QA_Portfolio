using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public PoolType type;
    public SpawnTile tilemanager;
    [SerializeField] public int score;

    [SerializeField] LayerMask playermask;
    [SerializeField] LayerMask tilemask;

    private void OnEnable()
    {
        if (type == PoolType.bomb)
            StartCoroutine(Bomb());
        else
            Invoke("OnCollider", 0.5f);
    }

    void OnCollider()
    {
        GetComponent<Collider2D>().enabled = true;   
    }
    IEnumerator Bomb()
    {
        yield return new WaitForSeconds(3f);
        tilemanager.Boom(transform);
        gameObject.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (!collision.CompareTag("Player")) return;
        if (type == PoolType.bomb) return;

        if (type == PoolType.Healpack) collision.GetComponent<Player>().Hp++;
        if (type == PoolType.Shield) collision.GetComponent<Player>().HasShield = true;

        switch(type)
        {
            case PoolType.Healpack:
            case PoolType.Shield:
                AudioManager.instance?.Play_Sfx(SFXList.Effect_item_Health);
                break;
            case PoolType.gold:
            case PoolType.diamond:
            case PoolType.crown:
                AudioManager.instance?.Play_Sfx(SFXList.Effect_Item_Score);
                break;
        }

        GameManager.instance.Score += score;
        gameObject.SetActive(false);
    }
}
