using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static int currentLineIndex;

    public static Dictionary<int, SC_Grid> gridByMidiNote = new Dictionary<int, SC_Grid>();

    public static string[] deviceIds;
    public static int deviceIdIndex;
    public static int channel;

}
