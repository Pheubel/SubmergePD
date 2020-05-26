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
    public Text ResultText = null;
    public int count = 0;
    public List<String> bhvs;
    public List<String> dates;
    public List<String> sins;
    public List<String> addresses;
    public List<Texture> images;

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

    #endregion

    // Start is called before the first frame update
    void Start()
    {

          ListingObjectsAsync(S3BucketName, Client);
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
            using (StreamReader reader = new StreamReader(responseStream))
            {
                byte[] data = null;
                if (response.ResponseStream != null)
                {
                    Debug.Log("Did we get here?");
     
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0) ;
                    {
                        ms.Write(buffer, 0, read);
                    }
                    data = ms.ToArray();

                }
                images.Add(bytesToTexture2D(data));

                string title = response.Metadata["x-amz-meta-sin"]; // Assume you have "title" as medata added to the object.
                string contentType = response.Headers["Content-Type"];

                Debug.Log("Object metadata, Title: {0} " + title);
                Debug.Log("Content type: {0} " + contentType);


                }
            }
        }
        catch (AmazonS3Exception e)
        {
            Debug.Log("Error encountered ***. Message:'{0}' when writing an object "+ e.Message);
        }
        catch (Exception e)
        {
            Debug.Log("Unknown encountered on server. Message:'{0}' when writing an object "+ e.Message);
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


}
