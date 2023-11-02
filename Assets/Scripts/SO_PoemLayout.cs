using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif  


[CreateAssetMenu(fileName = "PoemLayout", menuName = "Poem Layout", order = 1)]
public class SO_PoemLayout : ScriptableObject
{
    [Serializable]
    public class Word
    {
        public string word;
        public int midiNote;
        public int noteIdleVelocity;
        public int noteActiveVelocity;
    }

    [Serializable]
    public class Line
    {
        public Word[] words;
        public Line(int numWords)
        {
            words = new Word[numWords];
            for (int i = 0; i < numWords; i++)
            {
                words[i] = new Word(); // Initialize each Word object
            }
        }
    }

    [Serializable]
    public class Poem
    {
        public Line[] lines;
        public Poem(int numLines, int numWords)
        {
            lines = new Line[numLines];
            for (int i = 0; i < numLines; i++)
            {
                lines[i] = new Line(numWords);
            }
        }
    }

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

        // assign the midi notes
        int[] midiMapping = new int[] {
                                        81, 82, 83, 84, 85, 86, 87, 88,
                                        71, 72, 73, 74, 75, 76, 77, 78,
                                        61, 62, 63, 64, 65, 66, 67, 68,
                                        51, 52, 53, 54, 55, 56, 57, 58,
                                        41, 42, 43, 44, 45, 46, 47, 48,
                                        31, 32, 33, 34, 35, 36, 37, 38,
                                        21, 22, 23, 24, 25, 26, 27, 28,
                                        11, 12, 13, 14, 15, 16, 17, 18
                                        };

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                // avoid index out of range error
                // assign the word to the poem in sequence one by one
                if (words.Count > 0)
                {
                    Debug.Log("i: " + i + " j: " + j + " word: " + words[0]);
                    poem.lines[i].words[j].word = words[0];
                    poem.lines[i].words[j].midiNote = midiMapping[i * numCols + j];
                    poem.lines[i].words[j].noteIdleVelocity = 78;
                    poem.lines[i].words[j].noteActiveVelocity = 5;
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
