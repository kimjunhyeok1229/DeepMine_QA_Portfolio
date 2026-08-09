using UnityEngine;
using NUnit.Framework;

namespace DeepMine.EditMode
{
    public class SmokeTest
    {
        [Test]
        public void UTF_Environment_Check()
        {
            Assert.IsTrue(true, "UTF is working correctly.");
        }

        [Test]
        public void Assert_Check()
        {
            Assert.AreEqual(4, 2 + 2, "Basic arithmetic must work.");
            Assert.IsTrue(10 > 5, "10 must be greater than 5.");
            Assert.IsNotNull("hello", "String must not be null.");
        }

        [Test]
        public void GameObject_Creation_Check()
        {
            var obj = new GameObject("TestObject");

            Assert.IsNotNull(obj, "GameObject must be created successfully.");
            Assert.AreEqual("TestObject", obj.name, "GameObject name must match.");

            Object.DestroyImmediate(obj);
        }

        [Test]
        public void Component_Add_Check()
        {
            var obj = new GameObject("TestObject");
            obj.AddComponent<BoxCollider>();

            Assert.IsNotNull(obj.GetComponent<BoxCollider>(), "BoxCollider must be added to the GameObject.");

            Object.DestroyImmediate(obj);
        }

        [Test]
        public void ScriptableObject_Creation_Check()
        {
            var so = ScriptableObject.CreateInstance<ScriptableObject>();

            Assert.IsNotNull(so, "ScriptableObject must be created successfully.");

            Object.DestroyImmediate(so);
        }

        [Test]
        public void Data_Range_Validation_Example()
        {
            int playerHp = 100;
            Assert.GreaterOrEqual(playerHp, 1, "HP must be at least 1.");
            Assert.LessOrEqual(playerHp, 9999, "HP must not exceed 9999.");
        }
    }
}