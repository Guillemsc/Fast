using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Enhance
{
    public class EnhanceApkSuccessObject
    {
        public string session_id = "";
    }

    public class EnhanceApkErrorObject
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public enum EnhanceApkProgress
    {
        CREATING_SESSION,
        UPLOADING_APP,
        LINKING_APP_TO_SESSION,
        ADDING_SDKS,
        ENHANCING_APP
    }

    public class EnhanceApk : Request<EnhanceApkSuccessObject, EnhanceApkErrorObject>
    {
        private string enhance_key = "";
        private string enhance_secret = "";
        private string app_filepath = "";
        private List<SDK> sdks = new List<SDK>();

        private List<SDK> sdks_to_add = new List<SDK>();

        // Response classes

        private class CreateSessionResponse
        {
            public string session_id { get; set; }
        }

        public class Metadata
        {
            public string package_name { get; set; }
            public string title { get; set; }
            public string version_name { get; set; }
            public int version_code { get; set; }
        }

        public class UploadAppResponse
        {
            public string id { get; set; }
            public string filename { get; set; }
            public int size { get; set; }
            public string type { get; set; }
            public DateTime upload_date { get; set; }
            public bool metadata_extracted { get; set; }
            public Metadata metadata { get; set; }
        }

        public class LinkAppSessionResponse
        {
            public string session_id { get; set; }
            public string app_id { get; set; }
        }

        // ----------------

        public EnhanceApk(string enhance_key, string enhance_secret, string app_filepath, List<SDK> sdks)
        {
            this.enhance_key = enhance_key;
            this.enhance_secret = enhance_secret;
            this.app_filepath = app_filepath;
            this.sdks = sdks;
        }

        protected override void RunRequestInternal(Action<EnhanceApkSuccessObject> on_success,
            Action<EnhanceApkErrorObject> on_fail)
        {
            //For Enhance Api documentation visit https://enhance.co/documentation/api/

            UpdateProgress((int)EnhanceApkProgress.CREATING_SESSION, 0);

            RestClient client = new RestClient("https://enhance.co/api/v2/");
            client.Authenticator = new HttpBasicAuthenticator(enhance_key, enhance_secret);

            RestRequest create_seesion_request = new RestRequest("create-session", Method.POST);
            create_seesion_request.AddHeader("content-type", " application/json");

            client.ExecuteTaskAsync(create_seesion_request).ContinueWith(delegate(Task<IRestResponse> create_session_response)
            {
                if (create_session_response.Result.IsSuccessful)
                {
                    UpdateProgress((int)EnhanceApkProgress.UPLOADING_APP, 10);

                    CreateSessionResponse create_session_response_object = Fast.Parsers.JSONParser.
                    ParseObject<CreateSessionResponse>(create_session_response.Result.Content);

                    RestRequest upload_app_request = new RestRequest("upload-app", Method.POST);
                    upload_app_request.AddHeader("content-type", " multipart/form-data");
                    upload_app_request.AddFile("app", app_filepath);

                    client.ExecuteTaskAsync(upload_app_request).ContinueWith(delegate (Task<IRestResponse> upload_app_response)
                    {
                        if (upload_app_response.Result.IsSuccessful)
                        {
                            UpdateProgress((int)EnhanceApkProgress.LINKING_APP_TO_SESSION, 20);

                            UploadAppResponse apload_app_response_object = Fast.Parsers.JSONParser.
                            ParseObject<UploadAppResponse>(upload_app_response.Result.Content);

                            RestRequest link_app_session_request = new RestRequest("set-session-app", Method.POST);
                            link_app_session_request.AddHeader("content-type", "application/x-www-form-urlencoded");
                            link_app_session_request.AddParameter("session_id", create_session_response_object.session_id);
                            link_app_session_request.AddParameter("app_id", apload_app_response_object.id);

                            client.ExecuteTaskAsync(link_app_session_request).ContinueWith(delegate (Task<IRestResponse> link_app_session_response)
                            {
                                if (link_app_session_response.Result.IsSuccessful)
                                {
                                    UpdateProgress((int)EnhanceApkProgress.ADDING_SDKS, 60);

                                    LinkAppSessionResponse link_app_session_object = Fast.Parsers.JSONParser.
                                    ParseObject<LinkAppSessionResponse>(link_app_session_response.Result.Content);

                                    sdks_to_add.Clear();
                                    sdks_to_add.AddRange(sdks);

                                    AddAllSDKsToSession(client, create_session_response_object.session_id, 
                                    delegate()
                                    {
                                        UpdateProgress((int)EnhanceApkProgress.ENHANCING_APP, 80);

                                        RestRequest invoke_build_request = new RestRequest("invoke-build", Method.POST);
                                        invoke_build_request.AddHeader("content-type", "application/x-www-form-urlencoded");
                                        invoke_build_request.AddParameter("session_id", create_session_response_object.session_id);

                                        client.ExecuteTaskAsync(invoke_build_request).ContinueWith(delegate (Task<IRestResponse> invoke_build_response)
                                        {
                                            if (invoke_build_response.Result.IsSuccessful)
                                            {
                                                string response1 = invoke_build_response.Result.Content;

                                                EnhanceApkSuccessObject ret = new EnhanceApkSuccessObject();
                                                ret.session_id = create_session_response_object.session_id;

                                                if (on_success != null)
                                                    on_success.Invoke(ret);
                                            }
                                            else
                                            {
                                                EnhanceApkErrorObject ret = new EnhanceApkErrorObject();
                                                ret.ErrorException = invoke_build_response.Result.ErrorException;
                                                ret.ErrorMessage = invoke_build_response.Result.ErrorMessage;

                                                if (on_fail != null)
                                                    on_fail.Invoke(ret);
                                            }
                                        }, TaskScheduler.FromCurrentSynchronizationContext());                           
                                    }
                                    , delegate(IRestResponse add_all_sdks_to_session_response)
                                    {
                                        EnhanceApkErrorObject ret = new EnhanceApkErrorObject();
                                        ret.ErrorException = add_all_sdks_to_session_response.ErrorException;
                                        ret.ErrorMessage = add_all_sdks_to_session_response.ErrorMessage;

                                        if (on_fail != null)
                                            on_fail.Invoke(ret);
                                    });
                                }
                                else
                                {
                                    EnhanceApkErrorObject ret = new EnhanceApkErrorObject();
                                    ret.ErrorException = link_app_session_response.Result.ErrorException;
                                    ret.ErrorMessage = link_app_session_response.Result.ErrorMessage;

                                    if (on_fail != null)
                                        on_fail.Invoke(ret);
                                }
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        }
                        else
                        {
                            EnhanceApkErrorObject ret = new EnhanceApkErrorObject();
                            ret.ErrorException = upload_app_response.Result.ErrorException;
                            ret.ErrorMessage = upload_app_response.Result.ErrorMessage;

                            if (on_fail != null)
                                on_fail.Invoke(ret);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    EnhanceApkErrorObject ret = new EnhanceApkErrorObject();
                    ret.ErrorException = create_session_response.Result.ErrorException;
                    ret.ErrorMessage = create_session_response.Result.ErrorMessage;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AddAllSDKsToSession(RestClient client, string session_id, Action on_success, Action<IRestResponse> on_fail)
        {
            if (sdks_to_add.Count > 0)
            {
                SDK curr_sdk = sdks_to_add[0];
                sdks_to_add.RemoveAt(0);

                RestRequest add_sdk_to_session_request = new RestRequest("add-sdk-to-session", Method.POST);
                add_sdk_to_session_request.AddHeader("content-type", "application/x-www-form-urlencoded");
                add_sdk_to_session_request.AddParameter("session_id", session_id);
                add_sdk_to_session_request.AddParameter("sdk_id", curr_sdk.ID);
                add_sdk_to_session_request.AddParameter("sdk_type_id", curr_sdk.TypeID);
                add_sdk_to_session_request.AddParameter("data", curr_sdk.GenerateData());

                client.ExecuteTaskAsync(add_sdk_to_session_request).ContinueWith(delegate (Task<IRestResponse> add_sdk_to_session_response)
                {
                    string response = add_sdk_to_session_response.Result.Content;

                    if (add_sdk_to_session_response.Result.IsSuccessful)
                    {
                        AddAllSDKsToSession(client, session_id, on_success, on_fail);
                    }
                    else
                    {
                        if (on_fail != null)
                            on_fail.Invoke(add_sdk_to_session_response.Result);
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                if (on_success != null)
                    on_success.Invoke();
            }
        }
    }

    public class GetSDKsSuccessObject
    {

    }

    public class GetSDKsErrorObject
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class GetSDKs : Request<GetSDKsSuccessObject, GetSDKsErrorObject>
    {
        private string enhance_key = "";
        private string enhance_secret = "";

        public GetSDKs(string enhance_key, string enhance_secret)
        {
            this.enhance_key = enhance_key;
            this.enhance_secret = enhance_secret;
        }

        protected override void RunRequestInternal(Action<GetSDKsSuccessObject> on_success,
            Action<GetSDKsErrorObject> on_fail)
        {
            RestClient client = new RestClient("https://enhance.co/api/v2/");
            client.Authenticator = new HttpBasicAuthenticator(enhance_key, enhance_secret);

            RestRequest get_sdks_request = new RestRequest("sdks", Method.GET);

            client.ExecuteTaskAsync(get_sdks_request).ContinueWith(delegate (Task<IRestResponse> get_sdks_response)
            {
                if (get_sdks_response.Result.IsSuccessful)
                {
                    Newtonsoft.Json.Linq.JArray sdks_array = Fast.Parsers.JSONParser.ParseArray
                    (get_sdks_response.Result.Content);

                    Newtonsoft.Json.Linq.JToken curr_token = sdks_array.First;

                    while (curr_token != null)
                    {
                        string id = curr_token.Value<string>("id");
                        string type_id = curr_token.Value<string>("type_id");

                        if (id == "chartboost")
                        {
                            UnityEngine.Debug.Log(id + ": " + type_id);

                            Newtonsoft.Json.Linq.JObject parameters = curr_token.Value<Newtonsoft.Json.Linq.JObject>("parameters");

                            UnityEngine.Debug.Log(parameters);
                        }

                        curr_token = curr_token.Next;
                    }

                    GetSDKsSuccessObject ret = new GetSDKsSuccessObject();

                    if (on_success != null)
                        on_success.Invoke(ret);
                }
                else
                {
                    GetSDKsErrorObject ret = new GetSDKsErrorObject();
                    ret.ErrorException = get_sdks_response.Result.ErrorException;
                    ret.ErrorMessage = get_sdks_response.Result.ErrorMessage;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public class GetSessionStatusSuccessObject
    {
        public bool completed = false;
    }

    public class GetSessionStatusErrorObject
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class GetSessionStatus : Request<GetSessionStatusSuccessObject, GetSessionStatusErrorObject>
    {
        private string enhance_key = "";
        private string enhance_secret = "";

        private string session_id = "";

        public class GetSessionStatusResponse
        {
            public string status { get; set; }
        }

        public GetSessionStatus(string enhance_key, string enhance_secret, string session_id)
        {
            this.enhance_key = enhance_key;
            this.enhance_secret = enhance_secret;
            this.session_id = session_id;
        }

        protected override void RunRequestInternal(Action<GetSessionStatusSuccessObject> on_success,
            Action<GetSessionStatusErrorObject> on_fail)
        {
            RestClient client = new RestClient("https://enhance.co/api/v2/");
            client.Authenticator = new HttpBasicAuthenticator(enhance_key, enhance_secret);

            string url = "status/" + session_id;

            RestRequest get_session_status_request = new RestRequest(url, Method.GET);
            get_session_status_request.AddHeader("content-type", "application/json");

            client.ExecuteTaskAsync(get_session_status_request).ContinueWith(delegate (Task<IRestResponse> get_session_status_response)
            {
                if (get_session_status_response.Result.IsSuccessful)
                {
                    GetSessionStatusResponse get_session_status_object = Fast.Parsers.JSONParser.
                        ParseObject<GetSessionStatusResponse>(get_session_status_response.Result.Content);

                    GetSessionStatusSuccessObject ret = new GetSessionStatusSuccessObject();

                    if(get_session_status_object.status == "complete")
                    {
                        ret.completed = true;
                    }

                    if (on_success != null)
                        on_success.Invoke(ret);
                }
                else
                {
                    GetSessionStatusErrorObject ret = new GetSessionStatusErrorObject();
                    ret.ErrorException = get_session_status_response.Result.ErrorException;
                    ret.ErrorMessage = get_session_status_response.Result.ErrorMessage;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public class DownloadEnhancedApkSuccessObject
    {
        
    }

    public class DownloadEnhancedApkErrorObject
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class DownloadEnhancedApk : Request<DownloadEnhancedApkSuccessObject, DownloadEnhancedApkErrorObject>
    {
        private string enhance_key = "";
        private string enhance_secret = "";

        private string session_id = "";
        private string save_enhanced_app_filepath = "";

        public class GetSessionStatusResponse
        {
            public string status { get; set; }
        }

        public DownloadEnhancedApk(string enhance_key, string enhance_secret, string session_id, string save_enhanced_app_filepath)
        {
            this.enhance_key = enhance_key;
            this.enhance_secret = enhance_secret;
            this.session_id = session_id;
            this.save_enhanced_app_filepath = save_enhanced_app_filepath;
        }

        protected override void RunRequestInternal(Action<DownloadEnhancedApkSuccessObject> on_success,
            Action<DownloadEnhancedApkErrorObject> on_fail)
        {
            RestClient client = new RestClient("https://enhance.co/api/v2/");
            client.Authenticator = new HttpBasicAuthenticator(enhance_key, enhance_secret);

            string url = "download-enhanced-app/" + session_id;

            RestRequest download_app_request = new RestRequest(url, Method.GET);
            download_app_request.AddHeader("content-type", "application/x-www-form-urlencoded");
            download_app_request.AddParameter("id", session_id);

            client.ExecuteTaskAsync(download_app_request).ContinueWith(delegate (Task<IRestResponse> download_app_response)
            {
                if (download_app_response.Result.IsSuccessful)
                {
                    byte[] file_Raw = download_app_response.Result.RawBytes;

                    File.WriteAllBytes(save_enhanced_app_filepath, file_Raw);

                    DownloadEnhancedApkSuccessObject ret = new DownloadEnhancedApkSuccessObject();

                    if (on_success != null)
                        on_success.Invoke(ret);
                }
                else
                {
                    DownloadEnhancedApkErrorObject ret = new DownloadEnhancedApkErrorObject();
                    ret.ErrorException = download_app_response.Result.ErrorException;
                    ret.ErrorMessage = download_app_response.Result.ErrorMessage;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}

