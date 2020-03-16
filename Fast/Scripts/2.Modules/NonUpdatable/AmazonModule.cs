using System;
using System.Collections.Generic;
using UnityEngine;

#if USING_AMAZON

using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;

#endif

// To use this (in Unity 2019.1.14f), go to File -> BuildSettings -> PlayerSettings -> Player -> Scripting Define Simbols 
// and add the preprocessor directives when necessary:
// (keep in mind you have to do this for every platform)
// USING_AMAZON

// You can download the amazon sdk here https://docs.aws.amazon.com/mobile/sdkforunity/developerguide/setup-unity.html

namespace Fast.Modules
{
    public class AmazonModule : Module
    {
        private bool started = false;

#if USING_AMAZON

        private IAmazonS3 s3_client = null;

#endif

#if USING_AMAZON

        public void StartAmazonServices(string identity_pool_id, RegionEndpoint region_endpoint)
        {
            if (!started)
            {
                started = true;

                GameObject amazon_go = new GameObject("AmazonServices");
                amazon_go.SetParent(FastService.Instance.gameObject);

                UnityInitializer.AttachToGameObject(amazon_go);

                amazon_go.RemoveParent();

                AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

                CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                 identity_pool_id, region_endpoint);

                s3_client = new AmazonS3Client(credentials, region_endpoint);
            }
        }

        public void GetFile(string bucket_name, string filename, string save_filepath, Action on_finish)
        {
            if (started)
            {
                if (s3_client != null)
                {
                    s3_client.GetObjectAsync(bucket_name, filename,
                    (responseObj) =>
                    {
                        var response = responseObj.Response;

                        if (response != null)
                        {
                            if (response.ResponseStream != null)
                            {
                                Fast.Serializers.StreamSerializer.SerializeToPathAsync(save_filepath, response.ResponseStream,
                                delegate ()
                                {
                                    on_finish?.Invoke();
                                }
                                , delegate (string error)
                                {
                                    FastService.MLog.LogError(this, error);

                                    on_finish?.Invoke();
                                });
                            }
                            else
                            {
                                FastService.MLog.LogError(this, "GetFile response stream was null");

                                on_finish?.Invoke();
                            }
                        }
                        else
                        {
                            FastService.MLog.LogError(this, "GetFile response was null");

                            on_finish?.Invoke();
                        }
                    });
                }
                else
                {
                    FastService.MLog.LogError(this, "S3 client is null");

                    on_finish?.Invoke();
                }
            }
            else
            {
                FastService.MLog.LogError(this, "AmazonServices not started");

                on_finish?.Invoke();
            }
        }

#endif

    }
}
