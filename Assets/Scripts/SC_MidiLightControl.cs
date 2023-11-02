using jp.kshoji.unity.midi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SC_MidiLightControl : MonoBehaviour
{
    public int deviceIdIndex;
    [Range(0, 7)]
    public int channel;
    [Range(1, 99)]
    public int noteNumber;
    [Range(1, 127)]
    public int velocity;

    public int idleColor = 78;

    public static SC_MidiLightControl Singleton;

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
        yield return new WaitForSeconds(0.01f);

        GlobalVariables.deviceIds = MidiManager.Instance.DeviceIdSet.ToArray();
        GlobalVariables.deviceIdIndex = deviceIdIndex;
        GlobalVariables.channel = channel;
    }

    public void SetAllNotesToIdle()
    {

        for (int i = 11; i <= 88; i++)
        {
            // continue if number ends with 9 or 0
            if (i % 10 == 9 || i % 10 == 0)
            {
                continue;
            }
            MidiManager.Instance.SendMidiNoteOn(GlobalVariables.deviceIds[deviceIdIndex], 0, channel, i, idleColor);
        }
        MidiManager.Instance.SendMidiNoteOn(GlobalVariables.deviceIds[deviceIdIndex], 0, channel, 19, 5);
    }

    private void OnEnable()
    {
        SC_Grid.OnGridClicked += SendMidiNoteOn;
    }

    private void OnDestroy()
    {
        for (int i = 11; i <= 88; i++)
        {
            MidiManager.Instance.SendMidiNoteOn(GlobalVariables.deviceIds[deviceIdIndex], 0, channel, i, 0);

        }

        SC_Grid.OnGridClicked -= SendMidiNoteOn;
    }

    public void SendMidiNoteOn(SC_Grid grid)
    {
        MidiManager.Instance.SendMidiNoteOn(GlobalVariables.deviceIds[deviceIdIndex], 0, channel, grid.midiNote, grid.noteActiveVelocity);
    }
}
