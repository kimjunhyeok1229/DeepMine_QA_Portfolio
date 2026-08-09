using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine.SceneManagement;

namespace DeepMine.Playmode
{
    public class SmokeTest 
    {
        [UnityTest]
        public IEnumerator UTF_Environment_Check()
        {
            yield return null;

            Assert.IsTrue(true, "UTF is working correctly in Playmode.");
        }

        [UnityTest]
        public IEnumerator GameObject_Creation_Check()
        {
            var obj = new GameObject("TestObject");
            yield return null;

            Assert.IsNotNull(obj, "GameObject must be created successfully.");

            Object.Destroy(obj);
        }

        [UnityTest]
        public IEnumerator Component_Lifecycle_Check()
        {
            var obj = new GameObject("TestObject");
            var rb = obj.AddComponent<Rigidbody>();

            yield return null;

            Assert.IsNotNull(
                obj.GetComponent<Rigidbody>(),
                "Rigidbody must be initialized after one frame."
            );
            Assert.IsFalse(rb.isKinematic, "Rigidbody must not be kinematic by default.");

            Object.Destroy(obj);
        }

        [UnityTest]
        public IEnumerator Physics_Gravity_Check()
        {
            var obj = new GameObject("TestObject");
            var rb = obj.AddComponent<Rigidbody>();
            obj.transform.position = Vector3.zero;

            float initialY = obj.transform.position.y;

            // Wait for physics to apply gravity over several frames
            yield return new WaitForSeconds(0.5f);

            Assert.Less(
                obj.transform.position.y,
                initialY,
                "Object must fall due to gravity."
            );

            Object.Destroy(obj);
        }

        [UnityTest]
        public IEnumerator Coroutine_Execution_Check()
        {
            bool coroutineCompleted = false;

            // Use a MonoBehaviour runner to start the coroutine
            var obj = new GameObject("CoroutineRunner");
            var runner = obj.AddComponent<CoroutineRunner>();

            runner.StartCoroutine(TestCoroutine(() => coroutineCompleted = true));

            yield return new WaitForSeconds(0.2f);

            Assert.IsTrue(coroutineCompleted, "Coroutine must complete within 0.2 seconds.");

            Object.Destroy(obj);
        }

        private IEnumerator TestCoroutine(System.Action onComplete)
        {
            yield return new WaitForSeconds(0.1f);
            onComplete?.Invoke();
        }

        [UnityTest]
        public IEnumerator Scene_Load_Check()
        {
            yield return SceneManager.LoadSceneAsync("TitleScene");
            yield return null;

            Assert.AreEqual(
                "TitleScene",
                SceneManager.GetActiveScene().name,
                "Scene must load successfully."
            );
        }
    }

    // ----------------------------------------------------------------
    // Helper MonoBehaviour for running coroutines in tests
    // ----------------------------------------------------------------
    public class CoroutineRunner : MonoBehaviour { }
}