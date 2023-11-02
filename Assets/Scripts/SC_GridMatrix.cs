using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GridMatrix : MonoBehaviour
{
    public List<SO_PoemLayout> poemLayouts;

    private int numRows;
    public int numCols;

    public float matrixWidth;
    public float matrixHeight;

    public static SC_GridMatrix Singleton;
    private List<List<SC_Grid>> gridRows;

    public float xOffset;
    public float yOffset;


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

    IEnumerator Start()
    {
        numRows = poemLayouts[0].poemNumRows;
        numCols = poemLayouts[0].poemNumCols;

        yield return new WaitForSeconds(0.02f);

        InitializeGridMatrix();
    }

    void Update()
    {
        //if (Keyboard.current.spaceKey.wasPressedThisFrame)
        //{
        //    InitializeGridMatrix();
        //}
    }

    public void InitializeGridMatrix()
    {
        // delete all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        gridRows = new List<List<SC_Grid>>(numRows);
        for (int i = 0; i < numRows; i++)
        {
            gridRows.Add(new List<SC_Grid>());
        }

        for (int j = 0; j < numCols; j++)
        {
            for (int i = 0; i < numRows; i++)
            {
                GameObject grid = Instantiate(Resources.Load("Prefabs/Grid")) as GameObject;
                grid.transform.parent = this.transform;

                // Calculate positions with respect to top-left origin
                float posX = i * matrixWidth / numRows;
                float posY = -(j * matrixHeight / numCols);  // Negative because in Unity the y-axis is positive upwards

                // Adjust positions so the entire grid system is centered around the SC_GridMatrix GameObject
                posX -= matrixWidth * 0.5f - xOffset;
                posY += matrixHeight * 0.5f + yOffset;

                grid.transform.localPosition = new Vector3(posX, posY, 0);
                grid.name = "Grid " + i + " " + j;
                grid.GetComponent<SC_Grid>().SetCoordinate(new Coordinate(i, j));
                grid.GetComponent<SC_Grid>().SetWord(poemLayouts[GlobalVariables.currentLineIndex].poem.lines[j].words[i].word);
                grid.GetComponent<SC_Grid>().SetMidiNote(poemLayouts[GlobalVariables.currentLineIndex].poem.lines[j].words[i].midiNote);
                grid.GetComponent<SC_Grid>().SetVelocity(poemLayouts[GlobalVariables.currentLineIndex].poem.lines[j].words[i].noteIdleVelocity, poemLayouts[GlobalVariables.currentLineIndex].poem.lines[j].words[i].noteActiveVelocity);

                gridRows[j].Add(grid.GetComponent<SC_Grid>());

            }
        }

        SC_MidiLightControl.Singleton.SetAllNotesToIdle();
    }

    public void DeselectRow(int rowIndex)
    {
        if (rowIndex >= 0 && rowIndex < gridRows.Count)
        {
            foreach (SC_Grid grid in gridRows[rowIndex])
            {
                grid.Deselect();
            }
        }
    }
}
