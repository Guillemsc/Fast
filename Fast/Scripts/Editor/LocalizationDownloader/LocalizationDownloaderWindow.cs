#if UNITY_EDITOR

using System;
using UnityEngine;
using System.Threading.Tasks;

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

            Windows.WindowElements.BigDropdownHeader("Settings", ref Data.ShowSettings, Style);

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
            Windows.WindowElements.HorizontalLine(Style);

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
            Windows.WindowElements.HorizontalLine(Style);

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
    }
}

#endif
