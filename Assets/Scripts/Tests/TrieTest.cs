using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TrieTest
    {
        [Test]
        public void TrieTestCount()
        {
            // Use the Assert class to test conditions
            Trie trie = new Trie();
            trie.Add("ASDF");
            Assert.IsTrue(trie.Count("A") == 1);
            Assert.IsTrue(trie.Count("ASD") == 1);
            trie.Add("ASFF");
            Assert.IsTrue(trie.Count("A") == 2);
            Assert.IsTrue(trie.Count("AS") == 2);
            Assert.IsTrue(trie.Count("ASF") == 1);
            Assert.IsTrue(trie.Count("ASX") == 0);
        }
        [Test]
        public void TrieTestFind()
        {
            // Use the Assert class to test conditions
            Trie trie = new Trie();
            trie.Add("ASDF");
            Assert.IsFalse(trie.Find("A"));
            Assert.IsFalse(trie.Find("ASD"));
            Assert.IsTrue(trie.Find("ASDF"));
            trie.Add("ASFF");
            Assert.IsFalse(trie.Find("A"));
            Assert.IsFalse(trie.Find("AS"));
            Assert.IsFalse(trie.Find("ASF"));
            Assert.IsTrue(trie.Find("ASFF"));
        }
    }
}
