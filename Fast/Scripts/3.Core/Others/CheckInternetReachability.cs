using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Fast.Others
{
    public class CheckInternetReachabilitySuccessObject
    {
        private readonly bool can_reach_internet = false;

        public CheckInternetReachabilitySuccessObject(bool can_reach_internet)
        {
            this.can_reach_internet = can_reach_internet;
        }

        public bool CanReachInternet => can_reach_internet;
    }

    public class CheckInternetReachabilityErrorObject
    {

    }

    public class CheckInternetReachability : AwaitRequest<CheckInternetReachabilitySuccessObject, CheckInternetReachabilityErrorObject>
    {
        private readonly string web_to_test = "https://www.google.com";
        private readonly int timeout = 5;

        private UnityWebRequest webRequest = null;
 
        public CheckInternetReachability(string web_to_test, int timeout = 3)
        {
            this.web_to_test = web_to_test;
            this.timeout = timeout;
        }

        protected override async Task RunRequestInternal()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            UnityWebRequest webRequest = new UnityWebRequest(web_to_test);
            webRequest.timeout = timeout;

            webRequest.SendWebRequest().completed += delegate (AsyncOperation operation)
            {
                if (webRequest.isNetworkError)
                {
                    success_result = new CheckInternetReachabilitySuccessObject(false);
                }
                else
                {
                    success_result = new CheckInternetReachabilitySuccessObject(true);
                }

                webRequest.Dispose();

                tcs.SetResult(null);
            };

            await tcs.Task;
        }
    }
}
