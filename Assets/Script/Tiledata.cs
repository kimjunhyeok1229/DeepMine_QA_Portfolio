using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class Tiledata
{
    public Vector3Int pos;
    public int hp;
    public PoolType type;
    public bool isMaking;
    public Tiledata(PoolType type, int hp, Vector3Int pos)
    {
        this.type = type;
        this.hp = hp;
        this.pos = pos;
        isMaking = false;
    }

}
