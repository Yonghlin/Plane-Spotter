using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestAirport
{

    private GameObject airport;

    [SetUp]
    public void SetUp()
    {
        airport = GameObject.FindGameObjectWithTag("Airport");
    }

    // A Test behaves as an ordinary method
    [Test]
    public void TestAirportSimplePasses()
    {
        // Use the Assert class to test conditions

        Assert.AreEqual("Airport", airport.name);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestAirportWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
