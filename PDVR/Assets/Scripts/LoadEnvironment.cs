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

public class NewBehaviourScript : MonoBehaviour
{

    public int userID = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
