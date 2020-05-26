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
    public List<String> bhvs;
    public List<String> dates;
    public List<String> sins;
    public List<String> addresses;
    public List<Texture> images;

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
                    images.Add(bytesToTexture2D(data));



                    string title = response.Metadata["x-amz-meta-sin"]; // Assume you have "title" as medata added to the object.
                    bhvs.Add(response.Metadata["x-amz-meta-bhv"]);
                    dates.Add(response.Metadata["x-amz-meta-date"]);
                    sins.Add(response.Metadata["x-amz-meta-sin"]);
                    addresses.Add(response.Metadata["x-amz-meta-address"]);
                    string contentType = response.Headers["Content-Type"];

                    Debug.Log("Object metadata, Title: {0} " + title);
                    Debug.Log("Content type: {0} " + contentType);



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
        await Task.Delay(2500);
        RawImage image0 = imageMain.GetComponent<RawImage>();
        image0.texture = images[0];

        bhvText.GetComponent<Text>().text = bhvs[0];
        sinText.GetComponent<Text>().text = sins[0];
        addressText.GetComponent<Text>().text = addresses[0];
        dateText.GetComponent<Text>().text = dates[0];



        RawImage image1 = imageOne.GetComponent<RawImage>();
        image1.texture = images[0];
        RawImage image2 = imageTwo.GetComponent<RawImage>();
        image2.texture = images[1];
        RawImage image3 = imageThree.GetComponent<RawImage>();
        image3.texture = images[2];
        RawImage image4 = imageFour.GetComponent<RawImage>();
        image4.texture = images[3];
        RawImage image5 = imageFive.GetComponent<RawImage>();
        image5.texture = images[4];


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
            imageMain.GetComponent<RawImage>().texture = images[index];
            imageOne.GetComponent<RawImage>().texture = images[index];

            imageTwo.GetComponent<RawImage>().texture = images[index1];
            imageThree.GetComponent<RawImage>().texture = images[index2];
            imageFour.GetComponent<RawImage>().texture = images[index3];
            imageFive.GetComponent<RawImage>().texture = images[index4];

            bhvText.GetComponent<Text>().text = bhvs[index];
            sinText.GetComponent<Text>().text = sins[index];
            addressText.GetComponent<Text>().text = addresses[index];
            dateText.GetComponent<Text>().text = dates[index];
        }
       

    }

    public void buttonNext()
    {
        Debug.Log("CLICK");

        if (index < images.Count)
        {
            index++;
            index1++;
            index2++;
            index3++;
            index4++;

            imageMain.GetComponent<RawImage>().texture = images[index];
            imageOne.GetComponent<RawImage>().texture = images[index];

            imageTwo.GetComponent<RawImage>().texture = images[index1];
            imageThree.GetComponent<RawImage>().texture = images[index2];
            imageFour.GetComponent<RawImage>().texture = images[index3];
            imageFive.GetComponent<RawImage>().texture = images[index4];



            bhvText.GetComponent<Text>().text = bhvs[index];
            sinText.GetComponent<Text>().text = sins[index];
            addressText.GetComponent<Text>().text = addresses[index];
            dateText.GetComponent<Text>().text = dates[index];
        }
       


    }

  
    public void setActiveModels()
    {
        Panel_Photos.GetComponent<Canvas>().enabled = false;
        Panel_Models.GetComponent<Canvas>().enabled = true;
    }


}
