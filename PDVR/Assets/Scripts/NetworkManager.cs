using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Valve.Newtonsoft.Json;
using static LoadImages;
using System.Text;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public TMP_Text welcomeMessage;
    public GameObject loginScreen;
    public GameObject hideThis;
    public GameObject imagePrefab;
    private LoadImages imageManager;


    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }
    IEnumerator PostRequest(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Prefer", "resolution=merge-duplicates");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }

    public void Login(int ID)
    {
        StartCoroutine(GetRequest("//84.107.103.142:3000/officer?temp_id=eq." + ID, (UnityWebRequest req) =>
          {
              if (req.isNetworkError || req.isHttpError)
              {
                  Debug.Log($"{req.error}: {req.downloadHandler.text}");
              }
              else
              {
                  Officer[] officers = JsonConvert.DeserializeObject<Officer[]>(req.downloadHandler.text);
                  hideThis.SetActive(false);
                  welcomeMessage.SetText("Welkom " + officers[0].first_name + " " + officers[0].last_name);
                  ActiveUser.userID = officers[0].id;
                  ActiveUser.officer = officers[0];
                  StartCoroutine(HideScreen());
                  loadWorkspace(ActiveUser.userID);


              }
          }));
    }

    public void loadWorkspace(string userId)
    {
        StartCoroutine(GetRequest("//84.107.103.142:3000/image_cue?owner=eq." + userId, (UnityWebRequest req) =>
          {
              if (req.isNetworkError || req.isHttpError)
              {
                  Debug.Log($"{req.error}: {req.downloadHandler.text}");
              }
              else
              {

                  Image_cue[] images = JsonConvert.DeserializeObject<Image_cue[]>(req.downloadHandler.text);
                  foreach (Image_cue image in images)
                  {
                      GameObject picture = Instantiate(imagePrefab, new Vector3(image.vector_x, image.vector_y, image.vector_z), Quaternion.identity);

                      foreach (DbImage dbimage in imageList)
                      {
                          if (dbimage.sin == image.photo)
                          {
                              picture.GetComponent<Renderer>().material.mainTexture = dbimage.texture;
                              picture.GetComponent<Image_cue>().setImage_Cue(image);
                              picture.GetComponent<CustomTag>().AddTag(userId);
                              picture.GetComponent<CustomTag>().AddTag("image");

                              if(GameObject.Find(userId)==null)
                              {
                                  GameObject group = new GameObject(userId);
                                  picture.GetComponent<Transform>().parent = group.gameObject.transform;
                              } else
                              {
                                  picture.GetComponent<Transform>().parent = GameObject.Find(userId).transform;
                              }
                            
                           
                              break;
                          }
                      }

                  }
              }
          }));
    }


    public List<Officer> GetOfficers()
    {
        List<Officer> officerList = new List<Officer>();
        StartCoroutine(GetRequest("//84.107.103.142:3000/officer", (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            } else
            {
                Officer[] officers = JsonConvert.DeserializeObject<Officer[]>(req.downloadHandler.text);
              foreach(Officer officer in officers)
                {
                    officerList.Add(officer);
                }
            }
    
        }));

        return officerList;

    }
    public void saveImage(Image_cue cue)
    {
        if (ActiveUser.userID != null)
        {


            if (cue.id == 0)
        {
            StartCoroutine(GetRequest("//84.107.103.142:3000/rpc/get_new_id_image", (UnityWebRequest req) =>
            {

                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.Log($"{req.error}: {req.downloadHandler.text}");
                }
                else
                {
                    string body = req.downloadHandler.text;
                

                    cue.id = int.Parse(body);
                    string json = JsonUtility.ToJson(cue);
                    Debug.Log(ActiveUser.userID);
                    Debug.Log("INTEGER IS " + body);
                    Debug.Log(json);

                    StartCoroutine(PostRequest("//84.107.103.142:3000/image_cue", json));
                }


            }));
        } else
        {
            string json = JsonUtility.ToJson(cue);
            Debug.Log(ActiveUser.userID);
            Debug.Log(json);

            StartCoroutine(PostRequest("//84.107.103.142:3000/image_cue", json));
        }


        }
    }




    IEnumerator HideScreen()
    {
        yield return new WaitForSeconds(3);
        loginScreen.SetActive(false);
    }


}