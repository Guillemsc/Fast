using System;

#if USING_AMAZON

using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Model;

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
        private RegionEndpoint region_endpoint = null;
#endif

        private string bucket_name = "";

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

                AWSConfigs.CorrectForClockSkew = true;

                var loggingConfig = AWSConfigs.LoggingConfig;
                loggingConfig.LogTo = LoggingOptions.UnityLogger;
                loggingConfig.LogMetrics = true;
                loggingConfig.LogResponses = ResponseLoggingOption.Always;
                loggingConfig.LogResponsesSizeLimit = 4096;
                loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;

                this.region_endpoint = region_endpoint;

                CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                 identity_pool_id, region_endpoint);

                s3_client = new AmazonS3Client(credentials, region_endpoint);
            }
        }

        public void SetCurrentBucket(string bucket_name)
        {
            this.bucket_name = bucket_name;
        }

        public async Task<List<S3Object>> GetFilesList()
        {
            return await GetFilesList("");
        }

        public async Task<List<S3Object>> GetFilesList(string folder)
        {
            TaskCompletionSource<List<S3Object>> tcs = new TaskCompletionSource<List<S3Object>>();

            if (!started || s3_client == null)
            {
                FastService.MLog.LogError(this, "Amazon service not started, please use StartAmazonServices");
                tcs.SetResult(null);
            }
            else
            {
                ListObjectsRequest list_objects_request = new ListObjectsRequest()
                {
                    BucketName = bucket_name,
                    Prefix = folder,
                };

                s3_client.ListObjectsAsync(list_objects_request, (response) =>
                {
                    if (response.Exception != null)
                    {
                        FastService.MLog.LogError(this, $"Error getting file list on Amazon S3: {response.Exception.Message}");
                    }

                    if (response.Response == null)
                    {
                        tcs.SetResult(new List<S3Object>());
                    }
                    else
                    {
                        tcs.SetResult(response.Response.S3Objects);
                    }
                });
            }

            return await tcs.Task;
        }

        public async Task<List<string>> GetFoldersList(string folder)
        {
            TaskCompletionSource<List<string>> tcs = new TaskCompletionSource<List<string>>();

            if (!started || s3_client == null)
            {
                FastService.MLog.LogError(this, "Amazon service not started, please use StartAmazonServices");
                tcs.SetResult(null);
            }
            else
            {
                ListObjectsRequest list_objects_request = new ListObjectsRequest()
                {
                    BucketName = bucket_name,
                    Prefix = folder,
                    Delimiter = "/",
                };

                s3_client.ListObjectsAsync(list_objects_request, (response) =>
                {
                    if (response.Exception != null)
                    {
                        FastService.MLog.LogError(this, $"Error getting folders list on Amazon S3: {response.Exception.Message}");
                    }

                    if (response.Response == null)
                    {
                        tcs.SetResult(new List<string>());
                    }
                    else
                    {
                        tcs.SetResult(response.Response.CommonPrefixes);
                    }
                });
            }

            return await tcs.Task;
        }

        public async Task<bool> GetFile(string filename, string save_filepath)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            if (!started || s3_client == null)
            {
                FastService.MLog.LogError(this, "Amazon service not started, please use StartAmazonServices");
                tcs.SetResult(false);
            }
            else
            {
                GetObjectRequest get_object_request = new GetObjectRequest()
                {
                    BucketName = bucket_name,
                    Key = filename,
                };

                s3_client.GetObjectAsync(get_object_request, async (response) =>
                {
                    if (response.Exception != null)
                    {
                        FastService.MLog.LogError(this, $"Error getting file {filename} on Amazon S3: {response.Exception.Message}");
                    }

                    if (response.Response.ResponseStream == null)
                    {
                        tcs.SetResult(false);
                    }
                    else
                    {
                        await Serializers.StreamSerializer.SerializeToPathAsync(save_filepath, response.Response.ResponseStream);

                        tcs.SetResult(true);
                    }
                });
            }

            return await tcs.Task;
        }

        public async Task PostFile(string load_filepath, string filename)
        {
            Stream stream = await Serializers.StreamSerializer.DeSerializeFromPathAsync(load_filepath);

            if (stream == null)
            {
                FastService.MLog.LogError(this, $"Stream is null posting file {load_filepath} on Amazon S3");
            }
            
            await PostFile(stream, filename);

            if (stream != null)
            {
                stream.Dispose();
            }
        }

        public async Task PostFile(Stream stream, string filename)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            if (!started || s3_client == null)
            {
                FastService.MLog.LogError(this, "Amazon service not started, please use StartAmazonServices");
                tcs.SetResult(null);
            }
            else
            {
                PutObjectRequest put_object_request = new PutObjectRequest()
                {
                    BucketName = bucket_name,
                    Key = filename,
                    InputStream = stream,
                    CannedACL = S3CannedACL.Private,
                };

                s3_client.PutObjectAsync(put_object_request, (response) =>
                {
                    if (response.Exception != null)
                    {
                        FastService.MLog.LogError(this, $"Error posting file {filename} on Amazon S3: {response.Exception.Message}");
                    }

                    tcs.SetResult(null);
                });
            }

            await tcs.Task;
        }

        public async Task DeleteFile(string filename)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            if (!started || s3_client == null)
            {
                FastService.MLog.LogError(this, "Amazon service not started, please use StartAmazonServices");
                tcs.SetResult(null);
            }
            else
            {
                KeyVersion key_version = new KeyVersion()
                {
                    Key = filename,
                };

                List<KeyVersion> objects = new List<KeyVersion>() { key_version };

                var delete_object_request = new DeleteObjectsRequest()
                {
                    BucketName = bucket_name,
                    Objects = objects,
                };

                s3_client.DeleteObjectsAsync(delete_object_request, (response) =>
                {
                    if (response.Exception != null)
                    {
                        FastService.MLog.LogError(this, $"Error deleting file {filename} on Amazon S3: {response.Exception.Message}");
                    }

                    tcs.SetResult(null);
                });
            }

            await tcs.Task;
        }
#endif

    }
}
