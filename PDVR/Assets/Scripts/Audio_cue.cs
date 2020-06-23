using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Audio_cue : MonoBehaviour
{

    public Officer owner;
    public string title;
    public int id;
    public int workspace;
    public float vector_x;
    public float vector_y;
    public float vector_z;
    public string audioID;
    public TMP_Text titleText;
    public TMP_Text userName;

    public Audio_cue(Officer owner, string title, int id, int workspace, float vector_x, float vector_y, float vector_z, string audioID)
    {
        this.owner = owner;
        this.title = title;
        this.id = id;
        this.workspace = workspace;
        this.vector_x = vector_x;
        this.vector_y = vector_y;
        this.vector_z = vector_z;
        this.audioID = audioID;
        this.titleText.text = title;
        this.userName.text = (owner.first_name + owner.last_name);
        
    }

    public void SetTitleListening()
    {
        this.titleText.text = "Luisteren...";
    }
    public void SetAudio_Cue(Audio_cue cue)
    {
        this.owner = cue.owner;
        this.title = cue.title;
        this.id = cue.id;
        this.workspace = cue.workspace;
        this.vector_x = cue.vector_x;
        this.vector_y = cue.vector_y;
        this.vector_z = cue.vector_z;
        this.audioID = cue.audioID;
        this.titleText.text = cue.title;
        this.userName.text = (cue.owner.first_name + cue.owner.last_name);
    }

    public void SetTitle(string text)
    {
        this.titleText.text = text;
    }
}
