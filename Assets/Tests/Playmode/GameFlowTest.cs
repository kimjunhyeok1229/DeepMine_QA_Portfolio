using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DeepMine.Playmode
{
    public class GameFlowTest
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

            yield return null;
        }

        //E2E001 -> Structural Issue

        //[UnityTest]
        //public IEnumerator E2E001_Data_Reset_Check()
        //{
        //    yield return null;
        //}

        [UnityTest]
        public IEnumerator E2E002_ScoreSave_Check()
        {
            int highScore = GameManager.instance.highScore;
            GameManager.instance.Score += 100;

            player.Damaged(player.Hp);

            yield return null;

            Assert.Greater(GameManager.instance.highScore, highScore, $"Before Score: {highScore}, Current Score: {GameManager.instance.highScore}");
        }
    }
}