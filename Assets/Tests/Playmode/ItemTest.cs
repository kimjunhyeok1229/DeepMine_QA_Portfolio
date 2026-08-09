using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DeepMine.Playmode
{
    public class ItemTest
    {
        const string PLAY_SCENE_NAME = "SampleScene";

        Player player;

        GameObject itemObject;
        Item item;
        BoxCollider2D collider;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync(
                PLAY_SCENE_NAME,
                LoadSceneMode.Single
            );

            yield return null;
            yield return null;

            player = Object.FindObjectOfType<Player>();

            itemObject = new GameObject();
            item = itemObject.AddComponent<Item>();
            collider = itemObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            itemObject.transform.position = player.transform.position; 

            itemObject.SetActive(false);

            yield return null;
        }

        [UnityTearDown]
        public void TearDown()
        {
            Object.Destroy(itemObject);
        }

        [UnityTest]
        public IEnumerator PM010_Item_Disappear_Check()
        {
            item.type = PoolType.gold;
            itemObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            Assert.IsFalse(itemObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator PM011_Item_Heal_Check()
        {
            int hp = player.Hp;
            player.Damaged(1);

            item.type = PoolType.Healpack;
            itemObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            Assert.AreEqual(hp, player.Hp);
        }

        [UnityTest]
        public IEnumerator PM012_Item_Shield_Check()
        {
            int hp = player.Hp;

            item.type = PoolType.Shield;
            itemObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            player.Damaged(1);            

            Assert.AreEqual(hp, player.Hp);
        }

        [UnityTest]
        public IEnumerator PM013_Item_Score_Check()
        {
            int score = GameManager.instance.Score;

            item.type = PoolType.crown;
            item.score = 10000;
            itemObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            Debug.Log(score);
            Debug.Log(GameManager.instance.Score);

            Assert.Greater(GameManager.instance.Score, score, $"Before: {score}, After: {GameManager.instance.Score}");
        }
    }
}