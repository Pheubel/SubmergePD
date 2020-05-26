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
    public static string S3BucketName = null;
    public static string keyName = null;
    public Text ResultText = null;
    public List<Texture> Images = null;
    public List<GameObject> imageHolders = null;
    public int count = 0;
    private static IAmazonS3 client;
    // Start is called before the first frame update
    void Start()
    {

        client = new AmazonS3Client(_S3Region);
    }

    static async Task ReadObjectDataAsync()
    {
        string responseBody = "";
        try
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = S3BucketName,
                Key = keyName
            };
            using (GetObjectResponse response = await client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                string contentType = response.Headers["Content-Type"];
                Console.WriteLine("Object metadata, Title: {0}", title);
                Console.WriteLine("Content type: {0}", contentType);

                responseBody = reader.ReadToEnd(); // Now you process the response body.
            }
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        }
    }
}
