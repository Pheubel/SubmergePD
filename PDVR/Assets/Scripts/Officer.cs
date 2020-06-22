using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Officer : MonoBehaviour
{
    public string id;
    public string first_name;
    public string last_name;
    public int temp_id;

    public Officer(string id, string first_name, string last_name, int temp_id)
    {
        this.id = id;
        this.first_name = first_name;
        this.last_name = last_name;
        this.temp_id = temp_id;
    }
}
