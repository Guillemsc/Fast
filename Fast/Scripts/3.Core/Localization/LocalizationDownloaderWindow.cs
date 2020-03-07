using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Localization
{
    [System.Serializable]
    public class LocalizationWindowData
    {
        public string client_id = "";
        public string client_secret = "";
        public List<LocalizationFileToDownload> files_to_download = new List<LocalizationFileToDownload>();
        public string resources_path_to_save_at = "";
    }

    [System.Serializable]
    public class LocalizationFileToDownload
    {
        public string document_id = "";
        public string document_range = "";
        public string name_to_save_as = "";
    }

    class LocalizationDownloaderWindow : Fast.EditorTools.SerializableEditorWindow<LocalizationWindowData>
    {
        [SerializeField] private LocalizationWindowData serialized_data = new LocalizationWindowData();

        private Vector2 scroll_pos = new Vector2();

        private List<LocalizationFileToDownload> files_to_download_now = new List<LocalizationFileToDownload>();

        private string report_message = "";

        private string server_build_filepath = "";
        private string client_build_filepath = "";

        private bool updating_data = false;

        private Fast.EditorTools.Styles styles = null;

        public LocalizationDownloaderWindow() : base("Fast-EditorTools-LocalizationDownloaderWindow")
        {

        }

        [MenuItem("Fast/Localization downloader")]
        public static void ShowWindow()
        {
            GetWindow<LocalizationDownloaderWindow>("Fast Loc Downloader");
        }

        private void Awake()
        {
            DeSerialize(ref serialized_data);
        }

        private void OnLostFocus()
        {
            Serialize(serialized_data);
        }

        private void OnDestroy()
        {
            Serialize(serialized_data);
        }

        private void OnEnable()
        {
            DeSerialize(ref serialized_data);
        }

        private void InitStyles()
        {
            if (styles == null)
                styles = new Fast.EditorTools.Styles();
        }

        private void OnGUI()
        {
            InitStyles();

            this.minSize = new Vector2(275, this.minSize.y);

            scroll_pos = EditorGUILayout.BeginScrollView(scroll_pos);

            GUILayout.Label("FAST LOCALIZATION DOWNLOADER", styles.MainTitleStyle);

            EditorGUILayout.Separator();

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

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Google Drive client id", GUILayout.MaxWidth(175));

                serialized_data.client_id = EditorGUILayout.TextField(serialized_data.client_id);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Google Drive client secret", GUILayout.MaxWidth(175));

                serialized_data.client_secret = EditorGUILayout.TextField(serialized_data.client_secret);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Resources path to save at", GUILayout.MaxWidth(175));

                serialized_data.resources_path_to_save_at = EditorGUILayout.TextField(serialized_data.resources_path_to_save_at);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (GUILayout.Button("Add spreadsheet"))
            {
                serialized_data.files_to_download.Add(new LocalizationFileToDownload());
            }

            for (int i = 0; i < serialized_data.files_to_download.Count; ++i)
            {
                EditorGUILayout.Separator();

                LocalizationFileToDownload curr_file = serialized_data.files_to_download[i];

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("File " + (i + 1) + ": " + curr_file.name_to_save_as, styles.BoldTextStyle);

                    if (GUILayout.Button("X"))
                    {
                        serialized_data.files_to_download.RemoveAt(i);

                        --i;
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;

                string filepath = "Assets/Resources/" + serialized_data.resources_path_to_save_at + "/" + curr_file.name_to_save_as + ".txt";
                EditorGUILayout.LabelField(filepath);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Name to save as", GUILayout.MaxWidth(190));

                    curr_file.name_to_save_as = EditorGUILayout.TextField(curr_file.name_to_save_as);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Google Drive document id", GUILayout.MaxWidth(190));

                    curr_file.document_id = EditorGUILayout.TextField(curr_file.document_id);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Google Drive document range", GUILayout.MaxWidth(190));

                    curr_file.document_range = EditorGUILayout.TextField(curr_file.document_range);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Separator();

            EditorGUI.BeginDisabledGroup(updating_data || serialized_data.files_to_download.Count == 0);

            if (GUILayout.Button("Update"))
            {
                files_to_download_now = new List<LocalizationFileToDownload>(serialized_data.files_to_download);

                updating_data = true;

                report_message = "Processing...";

                this.Repaint();

                DownloadAndSaveNextFile(delegate ()
                {
                    updating_data = false;

                    report_message = "All data downloaded without errors";

                    this.Repaint();
                }
                , delegate (string error)
                {
                    updating_data = false;

                    report_message = "Error: " + error;

                    this.Repaint();
                });
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            if (!string.IsNullOrEmpty(report_message))
            {
                GUILayout.TextArea(report_message);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DownloadAndSaveNextFile(Action on_success, Action<string> on_fail)
        {
            if (files_to_download_now.Count > 0)
            {
                LocalizationFileToDownload curr_file = files_to_download_now[0];

                files_to_download_now.RemoveAt(0);

                Fast.GoogleDrive.DownloadSpreadsheet download_spreadsheet_request
                    = new Fast.GoogleDrive.DownloadSpreadsheet(serialized_data.client_id, serialized_data.client_secret, 
                    curr_file.document_id, curr_file.document_range);

                download_spreadsheet_request.RunRequest(
                delegate (Fast.GoogleDrive.DownloadSpreadsheetSuccessObject success)
                {
                    string tsv_data = Fast.Parsers.TSVParser.Compose(success.data);

                    string filepath = "Resources/" + serialized_data.resources_path_to_save_at + "/" + curr_file.name_to_save_as + ".txt";

                    bool can_continue = true;

                    try
                    {
                        Fast.Serializers.TextAssetSerializer.SerializeToAssetsPath(filepath, tsv_data);
                    }
                    catch (Exception ex)
                    {
                        string error_msg = "";

                        if (ex != null)
                        {
                            error_msg = ex.Message;

                            if (on_fail != null)
                                on_fail.Invoke(error_msg);

                            can_continue = false;
                        }
                    }

                    if (can_continue)
                    {
                        DownloadAndSaveNextFile(on_success, on_fail);
                    }
                }
                , delegate (Fast.GoogleDrive.DownloadSpreadsheetErrorObject error)
                {
                    string error_msg = "";

                    if (error.ErrorException != null)
                    {
                        error_msg = error.ErrorException.Message;

                        if (error.ErrorException.InnerException != null)
                            error_msg += " - " + error.ErrorException.InnerException.Message;

                        error_msg += " - " + error.ErrorMessage;
                    }

                    if (on_fail != null)
                        on_fail.Invoke(error_msg);
                });
            }
            else
            {
                if (on_success != null)
                    on_success.Invoke();
            }
        }
    }
}

#endif
