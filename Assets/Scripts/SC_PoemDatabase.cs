using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Line
{
    public String[] words;
    public Line(int numWords)
    {
        words = new String[numWords];
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

public class SC_PoemDatabase : MonoBehaviour
{
    [SerializeField]
    private string[] currentLine;

    public Poem poem;
    public int poemNumLines;
    private int poemNumWords;

    public string testWord;

    public static SC_PoemDatabase Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        SC_Grid.OnGridClicked += AddNextWordToCurrentLine;
    }

    private void OnDisable()
    {
        SC_Grid.OnGridClicked -= AddNextWordToCurrentLine;
    }

    void Start()
    {
        poemNumWords = SC_GridMatrix.Singleton.numCols;
        InitializePoem(poemNumLines, poemNumWords);
    }

    public void InitializePoem(int numLines, int numWords)
    {
        poem = new Poem(numLines, numWords);
        Debug.Log($"poem.lines.Length: {poem.lines.Length}, numwords: {numWords}");
        currentLine = new string[numWords];
    }


    void Update()
    {
        // if(Keyboard.current.spaceKey.wasPressedThisFrame){
        //     InitializePoem(poemNumLines, poemNumWords);
        // }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // if currentLine is not empty
            foreach (string word in currentLine)
            {
                if (word == null)
                {
                    return;
                }
            }

            AddLineToDatabase();
            currentLine = new string[poemNumWords];
        }
    }

    private void AddLineToDatabase()
    {


        poem.lines[GlobalVariables.currentLineIndex].words = currentLine;
        foreach (string word in poem.lines[GlobalVariables.currentLineIndex].words)
        {
            Debug.Log(word);
        }

        GlobalVariables.currentLineIndex++;
        Debug.Log($"currentLineIndex: {GlobalVariables.currentLineIndex}");
        SC_GridMatrix.Singleton.InitializeGridMatrix();
    }


    public void AddNextWordToCurrentLine(SC_Grid grid, int row)
    {
        currentLine[row] = grid.myWord;

        // deselect all grid of this row
        SC_GridMatrix.Singleton.DeselectRow(row);

        // then select the grid
        grid.Select();

    }
}
