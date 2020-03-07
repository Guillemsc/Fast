using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using Google.Apis.Sheets.v4;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Collections.Generic;
using System.Linq;

# if UNITY_EDITOR

namespace Fast.GoogleDrive
{
    public class DownloadSpreadsheetSuccessObject
    {
        public Data.GridData data { get; set; }
    }

    public class DownloadSpreadsheetErrorObject
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class DownloadSpreadsheet : Request<DownloadSpreadsheetSuccessObject, DownloadSpreadsheetErrorObject>
    {
        private string client_id = "";
        private string client_secret = "";
        private string document_id = "";
        private string document_data_range = "";

        public DownloadSpreadsheet(string client_id, string client_secret, string document_id, string document_data_range)
        {
            this.client_id = client_id;
            this.client_secret = client_secret;
            this.document_id = document_id;
            this.document_data_range = document_data_range;
        }

        protected override void RunRequestInternal(Action<DownloadSpreadsheetSuccessObject> on_success,
           Action<DownloadSpreadsheetErrorObject> on_fail)
        {
            //For Google Drive Api documentation visit https://developers.google.com/sheets/api/quickstart/dotnet

            string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };

            ClientSecrets secrets = new ClientSecrets();
            secrets.ClientId = client_id;
            secrets.ClientSecret = client_secret;

            GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, "user", CancellationToken.None)
                .ContinueWith(delegate(Task<UserCredential> user_credential_task)
                {
                    string error_msg = "";
                    Exception exception = null;

                    bool has_errors = user_credential_task.HasErrors(out error_msg, out exception);

                    if(!has_errors)
                    {
                        if (user_credential_task.Result != null)
                        {
                            BaseClientService.Initializer service_in = new BaseClientService.Initializer();
                            service_in.HttpClientInitializer = user_credential_task.Result;
                            service_in.ApplicationName = "Download Spreadhseet";

                            SheetsService service = new SheetsService(service_in);

                            SpreadsheetsResource.ValuesResource.GetRequest request =
                            service.Spreadsheets.Values.Get(document_id, document_data_range);

                            request.ExecuteAsync().ContinueWith(delegate (Task<Google.Apis.Sheets.v4.Data.ValueRange> values_task)
                            {
                                if (values_task.IsCanceled)
                                {
                                    DownloadSpreadsheetErrorObject ret = new DownloadSpreadsheetErrorObject();
                                    ret.ErrorException = values_task.Exception;
                                    ret.ErrorMessage = "Values tasks canceled";

                                    if (on_fail != null)
                                        on_fail.Invoke(ret);
                                }
                                else if (values_task.IsFaulted)
                                {
                                    DownloadSpreadsheetErrorObject ret = new DownloadSpreadsheetErrorObject();
                                    ret.ErrorException = values_task.Exception;
                                    ret.ErrorMessage = "Values tasks faulted";

                                    if (on_fail != null)
                                        on_fail.Invoke(ret);
                                }
                                else
                                {
                                    if (values_task.Result != null)
                                    {
                                        Google.Apis.Sheets.v4.Data.ValueRange values = values_task.Result;

                                        IList<IList<object>> values_data = values.Values;

                                        List<List<object>> data = new List<List<object>>();

                                        List<IList<object>> values_data_list = values_data.ToList();

                                        for (int i = 0; i < values_data_list.Count; ++i)
                                        {
                                            List<object> data_row = values_data_list[i].ToList();

                                            data.Add(data_row);
                                        }

                                        DownloadSpreadsheetSuccessObject ret = new DownloadSpreadsheetSuccessObject();
                                        ret.data = new Data.GridData(data);

                                        if (on_success != null)
                                            on_success.Invoke(ret);
                                    }
                                    else
                                    {
                                        DownloadSpreadsheetErrorObject ret = new DownloadSpreadsheetErrorObject();
                                        ret.ErrorException = values_task.Exception;
                                        ret.ErrorMessage = "Error on task";

                                        if (on_fail != null)
                                            on_fail.Invoke(ret);
                                    }
                                }

                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        }
                        else
                        {
                            DownloadSpreadsheetErrorObject ret = new DownloadSpreadsheetErrorObject();
                            ret.ErrorException = user_credential_task.Exception;
                            ret.ErrorMessage = "Error on task";

                            if (on_fail != null)
                                on_fail.Invoke(ret);
                        }
                    }
                    else
                    {
                        DownloadSpreadsheetErrorObject ret = new DownloadSpreadsheetErrorObject();
                        ret.ErrorMessage = error_msg;
                        ret.ErrorException = exception;

                        if (on_fail != null)
                            on_fail.Invoke(ret);
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}

#endif
