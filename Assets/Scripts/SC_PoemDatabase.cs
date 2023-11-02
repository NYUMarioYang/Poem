using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SC_PoemDatabase : MonoBehaviour
{
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

    [SerializeField]
    private string[] currentLine;

    public Poem poem;
    public int poemNumLines;
    private int poemNumWords;
    public TextMeshProUGUI poemTMP;

    public static SC_PoemDatabase Singleton;

    [SerializeField] private string poemString;

    [SerializeField]
    private float typingSpeed = 0.1f;

    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddLineToDatabase();
        }
    }

    public void AddLineToDatabase() {
        // Check if all words in the current line are not null or empty.
        foreach (string word in currentLine) {
            if (string.IsNullOrEmpty(word)) {
                return; // If there is a null or empty word, we don't add the line to the database.
            }
        }

        // Add the current line to the poem data structure.
        poem.lines[GlobalVariables.currentLineIndex].words = currentLine;

        // Prepare the latest line to be added to the poem display.
        string latestLine = "";
        foreach (string word in poem.lines[GlobalVariables.currentLineIndex].words) {
            latestLine += word + " ";
        }
        latestLine = latestLine.TrimEnd() + "\n"; // Trim the last space and add a newline.

        // Call TypeLine with only the latest line.
        StartCoroutine(TypeLine(latestLine));

        // Reset for the next line.
        GlobalVariables.currentLineIndex++;
        SC_GridMatrix.Singleton.InitializeGridMatrix();
        currentLine = new string[poemNumWords];
    }

    IEnumerator TypeLine(string line) {
        GlobalVariables.ready = false;
        audioSource.Play();
        int startLength = poemTMP.text.Length; // Start typing from the current length of the text.
        foreach (char letter in line.ToCharArray()) {
            poemTMP.text += letter; // Append one character to the text.
            yield return new WaitForSeconds(typingSpeed); // Wait before adding the next character.
        }
        audioSource.Stop();
        GlobalVariables.ready = true;
    }



    public void AddNextWordToCurrentLine(SC_Grid grid)
    {
        currentLine[grid.coordinate.y] = grid.myWord;

        // deselect all grid of this row
        SC_GridMatrix.Singleton.DeselectRow(grid.coordinate.y);

        // then select the grid
        grid.Select();

    }
}
