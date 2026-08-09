using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DeepMine.Playmode
{
    public class HpSystemTest
    {
        const string PLAY_SCENE_NAME = "SampleScene";

        Player player;

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
        }

        [UnityTest]
        public IEnumerator PM001_HP_Decrease_Check()
        {
            int hp = player.Hp;
            player.Damaged(1);

            Assert.AreEqual(hp - 1, player.Hp);

            yield return null;
        }

        [UnityTest]
        public IEnumerator PM002_Die_Event_Check()
        {
            int damage = player.Hp;
            player.Damaged(damage);

            yield return null;

            GameObject ui = GameObject.Find("OverPopup");

            Assert.IsTrue(ui.activeSelf && Time.timeScale == 0f);

            Time.timeScale = 1f;
        }

        [UnityTest]
        public IEnumerator PM003_HP_Recovery_Check()
        {
            int hp = player.Hp;
            player.Damaged(1);
            player.Hp++;

            Assert.AreEqual(hp, player.Hp);

            yield return null;
        }

        [UnityTest]
        public IEnumerator PM004_InvincibilityTime_Check()
        {
            int hp = player.Hp;
            player.Damaged(1);
            player.Damaged(1);
            player.Damaged(1);

            Assert.AreEqual(hp-1, player.Hp);

            yield return new WaitForSeconds(2f); // After Invincibility Time

            player.Damaged(1);

            Assert.AreEqual(hp - 2, player.Hp);

            yield return null;
        }
    }
}