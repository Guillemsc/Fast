using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Fast.Others
{
    public class CheckInternetReachabilitySuccessObject
    {

    }

    public class CheckInternetReachabilityErrorObject
    {
        public string error = "";
    }

    public class CheckInternetReachability : Request<CheckInternetReachabilitySuccessObject, CheckInternetReachabilityErrorObject>
    {
        private string web_to_test = "https://www.google.com";
        private int timeout = 5;

        private UnityWebRequest webRequest = null;

        public CheckInternetReachability()
        {

        }

        public CheckInternetReachability(string web_to_test, int timeout = 3)
        {
            this.web_to_test = web_to_test;
            this.timeout = timeout;
        }

        protected override void RunRequestInternal(Action<CheckInternetReachabilitySuccessObject> on_success, 
            Action<CheckInternetReachabilityErrorObject> on_fail)
        {
            webRequest = new UnityWebRequest(web_to_test);

            webRequest.timeout = timeout;

            webRequest.SendWebRequest().completed += delegate (AsyncOperation operation)
            {
                if(!webRequest.isNetworkError)
                {
                    webRequest.Dispose();

                    if (on_success != null)
                        on_success.Invoke(new CheckInternetReachabilitySuccessObject());
                }
                else
                {
                    CheckInternetReachabilityErrorObject ret = new CheckInternetReachabilityErrorObject();
                    ret.error = webRequest.error;

                    webRequest.Dispose();

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
            };
        }
    }
}
