using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using System.Threading.Tasks;
using System.Drawing;

public class LoadImages : MonoBehaviour
{

    public string IdentityPoolId = "";
    public string CognitoIdentityRegion = RegionEndpoint.EUWest1.SystemName;
    private RegionEndpoint _CognitoIdentityRegion
    {
        get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
    }
    public string S3Region = RegionEndpoint.EUWest1.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }


    public string S3BucketName = null;
    public int index = 0;
    public static List<DbImage> imageList = new List<DbImage>();

    public GameObject bhvText;
    public GameObject dateText;
    public GameObject sinText;
    public GameObject addressText;

    public Canvas Panel_Photos;
    public Canvas Panel_Models;


    public GameObject imageMain;
    public GameObject imageOne;
    public GameObject imageTwo;
    public GameObject imageThree;
    public GameObject imageFour;
    public GameObject imageFive;

    public GameObject prefabImage;
    public GameObject controller;

    public GameObject NetworkManager;

    #region private members

    private IAmazonS3 _s3Client;
    private AWSCredentials _credentials;

    private AWSCredentials Credentials
    {
        get
        {
            if (_credentials == null)
                _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
            return _credentials;
        }
    }

    private IAmazonS3 Client
    {
        get
        {
            if (_s3Client == null)
            {
                Debug.Log("Client created");
                _s3Client = new AmazonS3Client(Credentials, _S3Region);
            }
            //test comment
            return _s3Client;
        }
    }


    private int index1 = 1;
    private int index2 = 2;
    private int index3 = 3;
    private int index4 = 4;
    #endregion

     public class DbImage
    {
        public string bhv;
        public string date;
        public string sin;
        public string address;
        public string databaseID;
        public Texture texture;
        public DbImage(string bhv, string date, string sin, string address, Texture texture)
        {
            this.bhv = bhv;
            this.date = date;
            this.sin = sin;
            this.address = address;
            this.texture = texture;
            
        }

       
    }
    // Start is called before the first frame update
    void Start()
    {

        AsyncAwaitMethods();
        
    }

    async void AsyncAwaitMethods()
    {
        await ListingObjectsAsync(S3BucketName, Client);
        initializeListAsync();
    }

    async Task ReadObjectDataAsync(String key, String bucketName, IAmazonS3 client)
    {
        string responseBody = "";
        try
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };



            using (GetObjectResponse response = await client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            {

                byte[] data = null;
                if (response.ResponseStream != null)
                {
     

                    byte[] buffer = new byte[16 * 1024];
                    var stream = new MemoryStream();
                    await responseStream.CopyToAsync(stream);
                    stream.Position = 0;

                    data = stream.ToArray();



                    string title = response.Metadata["x-amz-meta-sin"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];

                    string bhv = response.Metadata["x-amz-meta-bhv"];
                    string date = response.Metadata["x-amz-meta-date"];
                    string sin = response.Metadata["x-amz-meta-sin"];
                    string address =response.Metadata["x-amz-meta-address"];
                    Texture texture = bytesToTexture2D(data);

                    Debug.Log(sin);
                    imageList.Add(new DbImage(bhv,date,sin,address, texture));


                }
            }
        }
        catch (AmazonS3Exception e)
        {
            Debug.Log("Error encountered ***. Message:'{0}' when writing an object " + e.Message);
        }
        catch (Exception e)
        {
            Debug.Log("Unknown encountered on server. Message:'{0}' when writing an object " + e.Message);
        }
    }

    async Task ListingObjectsAsync(String bucketName, IAmazonS3 client)
    {
        try
        {
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                MaxKeys = 10
            };
            ListObjectsV2Response response;
            do
            {
                response = 
                    
                    
                    
                    
                    await client.ListObjectsV2Async(request);

                // Process the response.
                foreach (S3Object entry in response.S3Objects)
                {
                    ReadObjectDataAsync(entry.Key, bucketName, client);
                    Debug.Log("key = {0} size = {1}"+
                        entry.Key+ entry.Size);
                }
                Console.WriteLine("Next Continuation Token: {0}", response.NextContinuationToken);
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            Debug.Log("S3 error occurred. Exception: " + amazonS3Exception.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
            
        }
    }

    public Texture2D bytesToTexture2D(byte[] imageBytes)
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        return tex;
    }


    private async Task initializeListAsync()
    {
        await Task.Delay(3500);
        RawImage image0 = imageMain.GetComponent<RawImage>();
        image0.texture = imageList[0].texture;

        bhvText.GetComponent<Text>().text = imageList[0].bhv;
        sinText.GetComponent<Text>().text = imageList[0].sin;
        addressText.GetComponent<Text>().text = imageList[0].address;
        dateText.GetComponent<Text>().text = imageList[0].date;



        RawImage image1 = imageOne.GetComponent<RawImage>();
        image1.texture = imageList[0].texture;
        RawImage image2 = imageTwo.GetComponent<RawImage>();
        image2.texture = imageList[1].texture;
        RawImage image3 = imageThree.GetComponent<RawImage>();
        image3.texture = imageList[2].texture;
        RawImage image4 = imageFour.GetComponent<RawImage>();
        image4.texture = imageList[3].texture;
        RawImage image5 = imageFive.GetComponent<RawImage>();
        image5.texture = imageList[4].texture;


    }

    public void buttonPrevious()
    {
        Debug.Log("CLICK");
        if (index > 0)
        {
            index--;
            index1--;
            index2--;
            index3--;
            index4--;
            imageMain.GetComponent<RawImage>().texture = imageList[index].texture;
            imageOne.GetComponent<RawImage>().texture = imageList[index].texture;

            imageTwo.GetComponent<RawImage>().texture = imageList[index1].texture;
            imageThree.GetComponent<RawImage>().texture = imageList[index2].texture;
            imageFour.GetComponent<RawImage>().texture = imageList[index3].texture;
            imageFive.GetComponent<RawImage>().texture = imageList[index4].texture;

            bhvText.GetComponent<Text>().text = imageList[index].bhv;
            sinText.GetComponent<Text>().text = imageList[index].sin;
            addressText.GetComponent<Text>().text = imageList[index].address;
            dateText.GetComponent<Text>().text = imageList[index].date;
        }
       

    }

    public void buttonNext()
    {
        Debug.Log("CLICK");

        if (index < imageList.Count)
        {
            index++;
            index1++;
            index2++;
            index3++;
            index4++;

            imageMain.GetComponent<RawImage>().texture = imageList[index].texture;
            imageOne.GetComponent<RawImage>().texture = imageList[index].texture;

            imageTwo.GetComponent<RawImage>().texture = imageList[index1].texture;
            imageThree.GetComponent<RawImage>().texture = imageList[index2].texture;
            imageFour.GetComponent<RawImage>().texture = imageList[index3].texture;
            imageFive.GetComponent<RawImage>().texture = imageList[index4].texture;



            bhvText.GetComponent<Text>().text = imageList[index].bhv;
            sinText.GetComponent<Text>().text = imageList[index].sin;
            addressText.GetComponent<Text>().text = imageList[index].address;
            dateText.GetComponent<Text>().text = imageList[index].date;
        }
       


    }

  
    public void setActiveModels()
    {
        Panel_Photos.GetComponent<Canvas>().enabled = false;
        Panel_Models.GetComponent<Canvas>().enabled = true;
    }

    public void createObjectFromImage(int indexNumber)
    {
        int selection =0;
        switch (indexNumber)
            
        {
            case 0:
                selection = index;
                break;

            case 1:
                selection = index1;
                break;

            case 2:
                selection = index2;
                break;

            case 3:
                selection = index3;
                break;

            case 4:
                selection = index4;
                break;
        }

        
        GameObject picture = Instantiate(prefabImage, controller.transform.position, Quaternion.identity);
        picture.GetComponent<Interactable>().m_ActiveHand = controller.GetComponent<HandInteraction>();
        picture.GetComponent<Renderer>().material.mainTexture = imageList[selection].texture;
        picture.GetComponent<Image_cue>().photo = imageList[selection].sin;
        picture.GetComponent<CustomTag>().AddTag("image");


    }




}
