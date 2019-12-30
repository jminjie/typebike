using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WordSubmitterTest2
    {
        [Test]
        public void WordSubmitterTest2_HELLOisValid()
        {
            // Use the Assert cl  // Use the Assert class to test conditions
            WordSubmitter.initDict(null /* gameHandler not needed for this test */);
            WordSubmitter wordSubmitter = WordSubmitter.GetWordSubmitterForTest();
            wordSubmitter.addLetter("H");
            wordSubmitter.addLetter("E");
            wordSubmitter.addLetter("L");
            wordSubmitter.addLetter("L");
            wordSubmitter.addLetter("O");
            Assert.IsTrue(wordSubmitter.isValidWord());
        }

        [Test]
        public void WordSubmitterTest2_HELLPisInvalid()
        {
            // Use the Assert cl  // Use the Assert class to test conditions
            WordSubmitter.initDict(null /* gameHandler not needed for this test */);
            WordSubmitter wordSubmitter = WordSubmitter.GetWordSubmitterForTest();
            wordSubmitter.addLetter("H");
            wordSubmitter.addLetter("E");
            wordSubmitter.addLetter("L");
            wordSubmitter.addLetter("L");
            wordSubmitter.addLetter("P");
            Assert.IsFalse(wordSubmitter.isValidWord());
        }
    }
}
