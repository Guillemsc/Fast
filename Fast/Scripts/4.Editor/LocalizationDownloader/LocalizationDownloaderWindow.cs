using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Editor.LocalizationDownloader
{
    class LocalizationDownloaderWindow : Windows.WindowHelper<LocalizationDownloaderWindowData>
    {
        private bool downloading = false;

        private string progress = "";

        public LocalizationDownloaderWindow() : base()
        {

        }

        [MenuItem("Fast/Localization downloader")]
        public static void ShowWindow()
        {
            GetWindow<LocalizationDownloaderWindow>("Fast Loc Downloader");
        }

        protected override void OnDrawGui()
        {
            DrawHeader();

            Windows.WindowElements.DropdownHeader("Settings", ref Data.ShowSettings, Style);

            if (Data.ShowSettings)
            {
                EditorGUI.BeginDisabledGroup(downloading);

                DrawMainBody();

                DrawSperadsheetList();

                EditorGUI.EndDisabledGroup();
            }

            if(!downloading)
            {
                DrawDownloadButton();
            }

            DrawDownloadProgress();
        }

        private void DrawHeader()
        {
            GUILayout.Label("FAST LOCALIZATION DOWNLOADER", Style.MainTitleStyle);
        }

        private void DrawMainBody()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.HelpBox("Find info about client id and client secret", MessageType.None);

                if (GUILayout.Button("Here", GUILayout.MaxWidth(50)))
                {
                    Application.OpenURL("https://developers.google.com/sheets/api/quickstart/dotnet");
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Google Drive client id", GUILayout.MaxWidth(175));
            Data.ClientId = EditorGUILayout.TextField(Data.ClientId);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Google Drive client secret", GUILayout.MaxWidth(175));
            Data.ClientSecret = EditorGUILayout.TextField(Data.ClientSecret);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Resources path to save at", GUILayout.MaxWidth(175));
            Data.PathToSave = EditorGUILayout.TextField(Data.PathToSave);
            string final_path = $"Assets/Resources/{Data.PathToSave}";
            EditorGUILayout.LabelField($"Final path: {final_path}");

            EditorGUILayout.Separator();
        }

        private void DrawSperadsheetList()
        {
            if (GUILayout.Button("Add spreadsheet"))
            {
                Data.FilesToDownload.Add(new LocalizationFile());
            }

            for (int i = 0; i < Data.FilesToDownload.Count; ++i)
            {
                EditorGUILayout.Separator();

                LocalizationFile curr_file = Data.FilesToDownload[i];

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("File " + (i + 1) + ": " + curr_file.NameToSaveAs, Style.BoldTextStyle);

                    if (GUILayout.Button("X", GUILayout.MaxWidth(50)))
                    {
                        Data.FilesToDownload.RemoveAt(i);

                        --i;
                    }
                }
                EditorGUILayout.EndHorizontal();

                DrawFileToDownload(curr_file);
            }
        }

        private void DrawFileToDownload(LocalizationFile file)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Name to save as", GUILayout.MaxWidth(190));
                file.NameToSaveAs = EditorGUILayout.TextField(file.NameToSaveAs);
            }
            EditorGUILayout.EndHorizontal();

            string final_path = $"Assets/Resources/{Data.PathToSave}/{file.NameToSaveAs}.txt";
            EditorGUILayout.LabelField($"Final path: {final_path}");

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Google Drive document id", GUILayout.MaxWidth(190));
                file.DocumentId = EditorGUILayout.TextField(file.DocumentId);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Google Drive document range", GUILayout.MaxWidth(190));
                file.DocumentRange = EditorGUILayout.TextField(file.DocumentRange);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }

        private void DrawDownloadButton()
        {
            EditorGUILayout.Separator();

            if (GUILayout.Button("Update", Style.BigButtonStyle))
            {
                Repaint();

                downloading = true;

                DownloadLocalizationDataAsync().ContinueWith(delegate (Task t)
                {
                    downloading = false;

                    Repaint();
                });
            }
        }

        private void DrawDownloadProgress()
        {
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField(progress, GUI.skin.textArea);
        }

        private void UpdateProgress(string progress)
        {
            this.progress = progress;

            Repaint();
        }

        private async Task DownloadLocalizationDataAsync()
        {
            progress = "";

            for (int i = 0; i < Data.FilesToDownload.Count; ++i)
            {
                LocalizationFile curr_file = Data.FilesToDownload[i];

                GoogleDrive.DownloadSpreadsheetRequest request 
                    = new GoogleDrive.DownloadSpreadsheetRequest(Data.ClientId, Data.ClientSecret, curr_file.DocumentId, curr_file.DocumentRange);

                request.OnProgressUpdate.Subscribe(delegate (RequestProgress prog)
                {
                    UpdateProgress(prog.ProgressDescription);
                });

                await request.RunRequest();

                if(request.HasErrors)
                {
                    string error = $"Error downloading file {i + 1} ({curr_file.NameToSaveAs}):\n\n{request.ErrorResult.MessageError})";
                    UpdateProgress(error);

                    return;
                }

                GoogleDrive.DownloadSpreadsheetSuccessObject result = request.SuccessResult;

                string tsv_data = Fast.Parsers.TSVParser.Compose(result.Data);

                string final_path = $"Resources/{Data.PathToSave}/{curr_file.NameToSaveAs}.txt";

                bool update_assets = Data.FilesToDownload.Count == i + 1;

                bool serialized = Fast.Serializers.TextAssetSerializer.SerializeToAssetsPath(final_path, tsv_data, update_assets);

                if(!serialized)
                {
                    string error = $"Error saving file {i + 1} ({curr_file.NameToSaveAs}):\n\nWrong path?";
                    UpdateProgress(error);

                    return;
                }
            }

            UpdateProgress("Download success");
        }

        //private void OnGUI()
        //{
        //    InitStyles();

        //    this.minSize = new Vector2(275, this.minSize.y);

        //    scroll_pos = EditorGUILayout.BeginScrollView(scroll_pos);

        //    GUILayout.Label("FAST LOCALIZATION DOWNLOADER", styles.MainTitleStyle);

        //    EditorGUILayout.Separator();

        //    EditorGUILayout.BeginHorizontal();
        //    {
        //        EditorGUILayout.HelpBox("Find info about client id and client secret", MessageType.None);

        //        if (GUILayout.Button("Here", GUILayout.MaxWidth(50)))
        //        {
        //            Application.OpenURL("https://developers.google.com/sheets/api/quickstart/dotnet");
        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.Separator();

        //    EditorGUILayout.BeginHorizontal();
        //    {
        //        EditorGUILayout.LabelField("Google Drive client id", GUILayout.MaxWidth(175));

        //        serialized_data.client_id = EditorGUILayout.TextField(serialized_data.client_id);
        //    }
        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.BeginHorizontal();
        //    {
        //        EditorGUILayout.LabelField("Google Drive client secret", GUILayout.MaxWidth(175));

        //        serialized_data.client_secret = EditorGUILayout.TextField(serialized_data.client_secret);
        //    }
        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.Separator();

        //    EditorGUILayout.BeginHorizontal();
        //    {
        //        EditorGUILayout.LabelField("Resources path to save at", GUILayout.MaxWidth(175));

        //        serialized_data.resources_path_to_save_at = EditorGUILayout.TextField(serialized_data.resources_path_to_save_at);
        //    }
        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.Separator();

        //    if (GUILayout.Button("Add spreadsheet"))
        //    {
        //        serialized_data.files_to_download.Add(new LocalizationFileToDownload());
        //    }

        //    for (int i = 0; i < serialized_data.files_to_download.Count; ++i)
        //    {
        //        EditorGUILayout.Separator();

        //        LocalizationFileToDownload curr_file = serialized_data.files_to_download[i];

        //        EditorGUILayout.BeginHorizontal();
        //        {
        //            EditorGUILayout.LabelField("File " + (i + 1) + ": " + curr_file.name_to_save_as, styles.BoldTextStyle);

        //            if (GUILayout.Button("X"))
        //            {
        //                serialized_data.files_to_download.RemoveAt(i);

        //                --i;
        //            }
        //        }
        //        EditorGUILayout.EndHorizontal();

        //        EditorGUI.indentLevel++;

        //        string filepath = "Assets/Resources/" + serialized_data.resources_path_to_save_at + "/" + curr_file.name_to_save_as + ".txt";
        //        EditorGUILayout.LabelField(filepath);

        //        EditorGUILayout.BeginHorizontal();
        //        {
        //            EditorGUILayout.LabelField("Name to save as", GUILayout.MaxWidth(190));

        //            curr_file.name_to_save_as = EditorGUILayout.TextField(curr_file.name_to_save_as);
        //        }
        //        EditorGUILayout.EndHorizontal();

        //        EditorGUILayout.BeginHorizontal();
        //        {
        //            EditorGUILayout.LabelField("Google Drive document id", GUILayout.MaxWidth(190));

        //            curr_file.document_id = EditorGUILayout.TextField(curr_file.document_id);
        //        }
        //        EditorGUILayout.EndHorizontal();

        //        EditorGUILayout.BeginHorizontal();
        //        {
        //            EditorGUILayout.LabelField("Google Drive document range", GUILayout.MaxWidth(190));

        //            curr_file.document_range = EditorGUILayout.TextField(curr_file.document_range);
        //        }
        //        EditorGUILayout.EndHorizontal();

        //        EditorGUI.indentLevel--;
        //    }

        //    EditorGUILayout.Separator();

        //    EditorGUI.BeginDisabledGroup(updating_data || serialized_data.files_to_download.Count == 0);

        //    if (GUILayout.Button("Update"))
        //    {
        //        files_to_download_now = new List<LocalizationFileToDownload>(serialized_data.files_to_download);

        //        updating_data = true;

        //        report_message = "Processing...";

        //        this.Repaint();

        //        DownloadAndSaveNextFile(delegate ()
        //        {
        //            updating_data = false;

        //            report_message = "All data downloaded without errors";

        //            this.Repaint();
        //        }
        //        , delegate (string error)
        //        {
        //            updating_data = false;

        //            report_message = "Error: " + error;

        //            this.Repaint();
        //        });
        //    }
        //    EditorGUI.EndDisabledGroup();

        //    EditorGUILayout.Separator();

        //    if (!string.IsNullOrEmpty(report_message))
        //    {
        //        GUILayout.TextArea(report_message);
        //    }

        //    EditorGUILayout.EndScrollView();
        //}

        //private void DownloadAndSaveNextFile(Action on_success, Action<string> on_fail)
        //{
        //    if (files_to_download_now.Count > 0)
        //    {
        //        LocalizationFileToDownload curr_file = files_to_download_now[0];

        //        files_to_download_now.RemoveAt(0);

        //        Fast.GoogleDrive.DownloadSpreadsheet download_spreadsheet_request
        //            = new Fast.GoogleDrive.DownloadSpreadsheet(serialized_data.client_id, serialized_data.client_secret, 
        //            curr_file.document_id, curr_file.document_range);

        //        download_spreadsheet_request.RunRequest(
        //        delegate (Fast.GoogleDrive.DownloadSpreadsheetSuccessObject success)
        //        {
        //            string tsv_data = Fast.Parsers.TSVParser.Compose(success.data);

        //            System.IO.Stream stream = tsv_data.ToStream();

        //            string filepath = serialized_data.resources_path_to_save_at + "/" + curr_file.name_to_save_as + ".txt";

        //            bool can_continue = true;

        //            try
        //            {
        //                Fast.Serializers.StreamSerializer.SerializeToPersistentPath(filepath, stream);
        //                //Fast.Serializers.TextAssetSerializer.SerializeToAssetsPath(filepath, tsv_data);
        //            }
        //            catch (Exception ex)
        //            {
        //                string error_msg = "";

        //                if (ex != null)
        //                {
        //                    error_msg = ex.Message;

        //                    if (on_fail != null)
        //                        on_fail.Invoke(error_msg);

        //                    can_continue = false;
        //                }
        //            }

        //            if (can_continue)
        //            {
        //                DownloadAndSaveNextFile(on_success, on_fail);
        //            }
        //        }
        //        , delegate (Fast.GoogleDrive.DownloadSpreadsheetErrorObject error)
        //        {
        //            string error_msg = "";

        //            if (error.ErrorException != null)
        //            {
        //                error_msg = error.ErrorException.Message;

        //                if (error.ErrorException.InnerException != null)
        //                    error_msg += " - " + error.ErrorException.InnerException.Message;

        //                error_msg += " - " + error.ErrorMessage;
        //            }

        //            if (on_fail != null)
        //                on_fail.Invoke(error_msg);
        //        });
        //    }
        //    else
        //    {
        //        if (on_success != null)
        //            on_success.Invoke();
        //    }
        //}
    }
}

#endif
