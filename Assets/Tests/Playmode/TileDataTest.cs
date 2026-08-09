using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DeepMine.Playmode
{
    public class TileDataTest 
    {
        const string PLAY_SCENE_NAME = "SampleScene";

        Tile[] tiles;
        Monster[] monsters;

        int tileCount;                

        [OneTimeSetUp]
        public IEnumerator SetUp()
        {
            SceneManager.LoadScene(
                PLAY_SCENE_NAME,
                LoadSceneMode.Single
            );
                        
            yield return null;
            yield return null;

            tiles = Object.FindObjectsOfType<Tile>();
            monsters = Object.FindObjectsOfType<Monster>();

            tileCount = tiles.Length;// + monsters.Length;
        }

        [UnityTest]
        public IEnumerator EM001_TileHP_Range_Check()
        {
            foreach (var tile in tiles)
            {
                Assert.That(tile.hp, Is.InRange(1, 4), $"[{tile.name}] HP Range Error");
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator EM002_TileType_Ratio_Check()
        {
            List<int> typeCount = Enumerable.Repeat(0, 17).ToList();
            int max = 0;

            foreach (var tile in tiles)
            {
                typeCount[(int)tile.type]++;
            }

            foreach (int type in typeCount)
            {
                if (type > max) max = type;
            }

            //if (monsters.Length > max) max = monsters.Length; // Monster Type Check

            float ratio = max / tileCount;
            Assert.Less(ratio, 0.5f, $"Max Ratio is {ratio}.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator EM003_EmptyTile_Check()
        {
            int count = 15 * 18 + 20; // height(Line) * (width(Tile) - TextTile) + FirstLine

            Assert.AreEqual(count, tileCount, $"Tile Count is {tileCount}.");

            yield return null;
        }
    }

}