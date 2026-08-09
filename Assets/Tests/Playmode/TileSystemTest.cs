using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DeepMine.Playmode
{
    public class TileSystemTest
    {
        const string PLAY_SCENE_NAME = "SampleScene";

        SpawnTile spawnTile;

        GameObject parent;
        GameObject tileObject;
        Tile tile;

        Vector3 tilePos = new Vector3(0, 15, 0);

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync(
                PLAY_SCENE_NAME,
                LoadSceneMode.Single
            );

            yield return null;
            yield return null;

            spawnTile = Object.FindObjectOfType<SpawnTile>();

            parent = new GameObject();
            tileObject = new GameObject();
            tileObject.transform.SetParent(parent.transform);
            tileObject.transform.position = tilePos;

            tile = tileObject.AddComponent<Tile>();
            tile.hp = 3;
            tile.type = PoolType.Nomal;

            yield return null;
        }

        [UnityTearDown]
        public void TearDown()
        {
            Object.Destroy(parent);
            Object.Destroy(tileObject);
        }

        [UnityTest]
        public IEnumerator PM020_Tile_Mining_Check()
        {
            int hp = tile.hp;

            spawnTile.Mining(tile);

            Assert.AreEqual(tile.hp, hp - 1);

            yield return null;
        }

        [UnityTest]
        public IEnumerator PM021_ItemCreation_OnTileDestroy_Check()
        {
            tile.type = PoolType.Treasure;

            spawnTile.Mining(tile, tile.hp);

            yield return new WaitForSeconds(3f); // Item Creation Delay

            Item item = parent.GetComponentInChildren<Item>();

            Assert.AreEqual(item.transform.position, tilePos, $"Item: {item.transform.position}, Tile: {tilePos}");
        }
    }
}