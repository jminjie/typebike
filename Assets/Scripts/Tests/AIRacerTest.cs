using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AIRacerTest
    {
        [Test]
        public void AIRacerTestGetDirectionOfVector()
        {
            int dir = AIRacer.GetDirectionOfVector(new Vector2(0, 1));
            Assert.IsTrue(Floor.UP == dir);

            dir = AIRacer.GetDirectionOfVector(new Vector2(1, 0));
            Assert.IsTrue(Floor.RIGHT == dir);

            dir = AIRacer.GetDirectionOfVector(new Vector2(-1, 0));
            Assert.IsTrue(Floor.LEFT == dir);

            dir = AIRacer.GetDirectionOfVector(new Vector2(0, -1));
            Assert.IsTrue(Floor.DOWN == dir);
        }

        [Test]
        public void AIRacerTestGetDirectionOfVectorDiagonals()
        {
            int dir = AIRacer.GetDirectionOfVector(new Vector2(1, 2));
            Assert.IsTrue(Floor.UP == dir);

            dir = AIRacer.GetDirectionOfVector(new Vector2(2, 1));
            Assert.IsTrue(Floor.RIGHT == dir);

            dir = AIRacer.GetDirectionOfVector(new Vector2(-2, 1));
            Assert.IsTrue(Floor.LEFT == dir);

            dir = AIRacer.GetDirectionOfVector(new Vector2(1, -2));
            Assert.IsTrue(Floor.DOWN == dir);
        }
    }
}
