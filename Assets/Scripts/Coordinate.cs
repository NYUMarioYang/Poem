using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate
{
    public int x {get; private set;}
    public int y {get; private set;}

    public Coordinate(int x, int y){
        this.x = x;
        this.y = y;
    }
}
