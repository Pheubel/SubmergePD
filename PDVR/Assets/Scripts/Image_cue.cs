using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Image_cue : MonoBehaviour
{

    public string owner;
    public int id;
        public int workspace;
        public float vector_x;
        public float vector_y;
        public float vector_z;
        public string photo;


        //UPSERT
        public Image_cue(string owner, int id, int workspace, float vector_x, float vector_y, float vector_z, string photo)
        {
            this.owner = owner;
            this.id = id;
            this.workspace = workspace;
            this.vector_x = vector_x;
            this.vector_y = vector_y;
            this.vector_z = vector_z;
            this.photo = photo;
        }

    public void setImage_Cue(Image_cue cue)
    {
        this.owner = cue.owner;
        this.id = cue.id;
        this.workspace = cue.workspace;
        this.vector_x = cue.vector_x;
        this.vector_y = cue.vector_y;
        this.vector_z = cue.vector_z;
        this.photo = cue.photo;
    }
    }
