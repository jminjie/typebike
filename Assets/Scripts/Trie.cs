using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class is expected to only receive capital letters A-Z

public class Node
{
    const int numChildren = 26;
    public Node[] children;
    // Number of words starting with this prefix.
    public int prefixNum;
    public Node()
    {
        children = new Node[numChildren];
        prefixNum = 0;
    }
}

public class Trie 
{
    private Node root;

    public Trie()
    {
        root = new Node();
    }

    public Trie(HashSet<string> dict)
    {
        root = new Node();
        foreach (string word in dict)
        {
            Add(word);
        }
    }

    public void Add(string word)
    {
        Node node = root;
        foreach (char c in word)
        {
            int idx = c - 'A';
            if (node.children[idx] == null)
            {
                node.children[idx] = new Node();
            }
            node = node.children[idx];
            node.prefixNum++;
        }
    }
    public int Count(string prefix)
    {
        Node node = root;
        foreach (char c in prefix)
        {
            int idx = c - 'A';
            if (node.children[idx] == null)
            {
                return 0;
            }
            node = node.children[idx];
        }
        return node.prefixNum;
    }
     
}
