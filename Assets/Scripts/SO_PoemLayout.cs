using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif  


[CreateAssetMenu(fileName = "PoemLayout", menuName = "Poem Layout", order = 1)]
public class SO_PoemLayout : ScriptableObject
{
    public int poemNumRows = 8;
    public int poemNumCols = 8;

    public string allWords;

    public Poem poem;

    public void InitializePoem(int numRows, int numCols)
    {
        poem = new Poem(numRows, numCols);

        // parse allWords into a list of words
        List<string> words = new List<string>();
        string[] wordsArray = allWords.Split(' ');
        foreach (string word in wordsArray)
        {
            words.Add(word);
        }

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                // avoid index out of range error
                // assign the word to the poem in sequence one by one
                if (words.Count > 0)
                {
                    poem.lines[i].words[j] = words[0];
                    words.RemoveAt(0);
                }
            }
        }
    }
}

// Editor script
#if UNITY_EDITOR
[CustomEditor(typeof(SO_PoemLayout))]
public class SO_PoemLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get a reference to the scriptable object
        SO_PoemLayout poemLayout = (SO_PoemLayout)target;

        // Draw a button in the inspector
        if (GUILayout.Button("Initialize Poem"))
        {
            poemLayout.InitializePoem(poemLayout.poemNumRows, poemLayout.poemNumCols);
        }
    }
}
#endif
