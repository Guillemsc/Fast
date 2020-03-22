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

#if UNITY_EDITOR

namespace Fast.GoogleDrive
{
    public class DownloadSpreadsheetSuccessObject
    {
        private readonly Data.GridData data = null;

        public DownloadSpreadsheetSuccessObject(Data.GridData data)
        {
            this.data = data;
        }

        public Data.GridData Data => data;
    }

    public class DownloadSpreadsheetErrorObject
    {
        private string message_error = "";
        private Exception exception = null;

        public DownloadSpreadsheetErrorObject(string message_error, Exception exception)
        {
            this.message_error = message_error;
            this.exception = exception;
        }

        public string MessageError => message_error;
        public Exception Exception => exception;
    }

    public class DownloadSpreadsheetRequest : AwaitRequest<DownloadSpreadsheetSuccessObject, DownloadSpreadsheetErrorObject>
    {
        private readonly string client_id = "";
        private readonly string client_secret = "";
        private readonly string document_id = "";
        private readonly string document_data_range = "";

        public DownloadSpreadsheetRequest(string client_id, string client_secret, string document_id, string document_data_range)
        {
            this.client_id = client_id;
            this.client_secret = client_secret;
            this.document_id = document_id;
            this.document_data_range = document_data_range;
        }

        protected override async Task RunRequestInternal()
        { 
            //For Google Drive Api documentation visit https://developers.google.com/sheets/api/quickstart/dotnet

            string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };

            ClientSecrets secrets = new ClientSecrets();
            secrets.ClientId = client_id;
            secrets.ClientSecret = client_secret;

            UpdateProgress("Authenticating", 0);

            Task<UserCredential> user_credential_task = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, "user", CancellationToken.None);
            AwaitResult user_credential_task_result = await AwaitUtils.AwaitTask(user_credential_task);

            if(user_credential_task_result.HasErrors)
            {
                has_errors = true;
                error_result = new DownloadSpreadsheetErrorObject(
                    user_credential_task_result.Exception.Message, user_credential_task_result.Exception);

                return;
            }

            UserCredential credential = user_credential_task.Result;

            if (credential == null)
            {
                has_errors = true;
                error_result = new DownloadSpreadsheetErrorObject("Credentials are null", null);

                return;
            }

            UpdateProgress("Getting data", 50);

            BaseClientService.Initializer service_in = new BaseClientService.Initializer();
            service_in.HttpClientInitializer = credential;
            service_in.ApplicationName = "Download spreadhseet GoogleDrive";

            SheetsService service = new SheetsService(service_in);

            SpreadsheetsResource.ValuesResource.GetRequest get_values_request =
            service.Spreadsheets.Values.Get(document_id, document_data_range);

            Task<Google.Apis.Sheets.v4.Data.ValueRange> get_values_task = get_values_request.ExecuteAsync();
            AwaitResult get_values_task_result = await AwaitUtils.AwaitTask(get_values_task);

            UpdateProgress("Processing data", 50);

            if (get_values_task_result.HasErrors)
            {
                has_errors = true;
                error_result = new DownloadSpreadsheetErrorObject(
                    get_values_task_result.Exception.Message, get_values_task_result.Exception);

                return;
            }

            Google.Apis.Sheets.v4.Data.ValueRange values = get_values_task.Result;

            if (values == null)
            {
                has_errors = true;
                error_result = new DownloadSpreadsheetErrorObject("Values are null", null);

                return;
            }

            IList<IList<object>> values_data = values.Values;

            List<List<object>> data = new List<List<object>>();

            List<IList<object>> values_data_list = values_data.ToList();

            for (int i = 0; i < values_data_list.Count; ++i)
            {
                List<object> data_row = values_data_list[i].ToList();

                data.Add(data_row);
            }

            Data.GridData grid_data = new Data.GridData(data);

            success_result = new DownloadSpreadsheetSuccessObject(grid_data);
        }
    }
}

#endif
