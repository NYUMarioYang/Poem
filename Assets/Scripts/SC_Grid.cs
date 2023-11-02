using jp.kshoji.unity.midi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SC_Grid : MonoBehaviour
{
    public Coordinate coordinate { get; private set; }
    public string myWord;
    public int midiNote;
    public int noteIdleVelocity;
    public int noteActiveVelocity;

    private AudioSource audioSource;
    public List<AudioClip> audioClips;

    private float lastAudioPlayTime = 0f;
    private float audioCooldown = 0.5f;


    public static event Action<SC_Grid> OnGridClicked;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Material myMaterial;

    [ColorUsage(true, true)]
    public Color selectedColor;

    [ColorUsage(true, true)]
    public Color unselectedColor;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ParticleSystem particleSystem = transform.Find("VFX/shield").GetComponent<ParticleSystem>();
        myMaterial = new(particleSystem.GetComponent<Renderer>().material);
        particleSystem.GetComponent<Renderer>().material = myMaterial;

        myMaterial.SetColor("Color_EF401FF9", unselectedColor);
        myMaterial.SetColor("Color_C8F38133", unselectedColor);

        GlobalVariables.gridByMidiNote[midiNote] = this;
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
                OnGridClicked?.Invoke(this);
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

    public void SetMidiNote(int midiNote)
    {
        this.midiNote = midiNote;
    }

    public void SetVelocity(int idle, int active)
    {
        noteIdleVelocity = idle;
        noteActiveVelocity = active;
    }

    public static void TriggerGridSelection(int midiNote)
    {
        if (GlobalVariables.gridByMidiNote.TryGetValue(midiNote, out SC_Grid grid))
        {
            OnGridClicked?.Invoke(grid);
        }
    }

    public void Select()
    {
        MidiManager.Instance.SendMidiNoteOn(GlobalVariables.deviceIds[GlobalVariables.deviceIdIndex], 0, GlobalVariables.channel, midiNote, noteActiveVelocity);

        myMaterial.SetColor("Color_EF401FF9", selectedColor);
        myMaterial.SetColor("Color_C8F38133", selectedColor);
        var props = MaterialEditor.GetMaterialProperties(new UnityEngine.Object[] { myMaterial });

        foreach (var prop in props)
        {
            //Debug.Log(prop.name);
        }

        // play a random audio clip
        if (Time.time - lastAudioPlayTime >= audioCooldown)
        {
            audioSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            audioSource.Play();
            lastAudioPlayTime = Time.time; // Update the last play time
        }
    }



    public void Deselect()
    {
        MidiManager.Instance.SendMidiNoteOn(GlobalVariables.deviceIds[GlobalVariables.deviceIdIndex], 0, GlobalVariables.channel, midiNote, noteIdleVelocity);

        myMaterial.SetColor("Color_EF401FF9", unselectedColor);
        myMaterial.SetColor("Color_C8F38133", unselectedColor);



    }



}
