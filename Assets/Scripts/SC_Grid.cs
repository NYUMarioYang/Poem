using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Grid : MonoBehaviour
{
    public Coordinate coordinate { get; private set; }
    public string myWord;

    public static event Action<SC_Grid, int> OnGridClicked;

    private SpriteRenderer spriteRenderer;
    private bool selected = false;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // if left mouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            // if mouse is over this grid
            if (IsMouseOver())
            {
                Debug.Log("Clicked on " + myWord);
                OnGridClicked?.Invoke(this, coordinate.y);
            }
        }
    }

    private bool IsMouseOver()
    {
        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (rayHit.collider != null)
        {
            if (rayHit.collider.gameObject == this.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    public void SetCoordinate(Coordinate coordinate)
    {
        this.coordinate = coordinate;
    }

    public void SetWord(string word)
    {
        myWord = word;
    }

    public void Select()
    {
        spriteRenderer.color = Color.red;
        selected = true;
    }



    public void Deselect()
    {
        spriteRenderer.color = Color.white;
        selected = false;
    }



}
