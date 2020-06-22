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
using Dummiesman;
public class LoadObjects : MonoBehaviour
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
    public List<Texture> images;
    public Canvas Panel_Photos;
    public Canvas Panel_Models;



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
        
    }

    async Task ReadObjectDataAsync(String key, String bucketName, IAmazonS3 client)
    {
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

                if (response.ResponseStream != null)
                {


                    var stream = new MemoryStream();
                    await responseStream.CopyToAsync(stream);
                    stream.Position = 0;
                    var loadedObj = new OBJLoader().Load(stream);




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
                    Debug.Log("key = {0} size = {1}" +
                        entry.Key + entry.Size);
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
}
