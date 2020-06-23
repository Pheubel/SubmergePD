using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FilterTool : MonoBehaviour
{
    public GameObject prefabOfficerPanel;
    public NetworkManager NetworkManager;
    public List<Officer> officers;
    public GameObject content;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        
        
    }

    async void Initialize()
    {
        officers = NetworkManager.GetOfficers();

        await Task.Delay(3500);
        foreach (Officer officer in officers)
        {

            Debug.Log(officer.first_name);

            GameObject panel = Instantiate(prefabOfficerPanel, content.GetComponent<Transform>().position, content.GetComponent<Transform>().rotation, content.GetComponent<Transform>());

        
            ProfilePanel profile = panel.GetComponent<ProfilePanel>();
            profile.SetIdText(officer.id);
            profile.SetTempIdText(officer.temp_id.ToString());
            profile.SetLastNameText(officer.last_name);
            profile.SetFirstNameText(officer.first_name);
            profile.NetworkManager = NetworkManager;
              
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
