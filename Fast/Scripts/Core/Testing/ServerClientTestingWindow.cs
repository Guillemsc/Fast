using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

#if UNITY_EDITOR

using UnityEditor;

namespace Fast.Testing
{
    public class ServerClientTestingWindowData
    {
        public string build_folder = "";
        public string build_name = "";
        public string server_arg = "";
        public string client_arg = "";
        public bool server = true;
        public int clients = 1;
        public bool rebuild_server = false;
        public bool rebuild_client = false;
    }

    class ServerClientTestingWindow : Fast.EditorTools.SerializableEditorWindow<ServerClientTestingWindowData>
    {
        private Vector2 scroll_pos = new Vector2();

        [SerializeField] [HideInInspector] private ServerClientTestingWindowData serialized_data = new ServerClientTestingWindowData();

        private string server_build_filepath = "";
        private string client_build_filepath = "";

        private Fast.EditorTools.Styles styles = null;

        private ServerClientTestingWindow() : base("Fast-Testing-ServerClientTestingWindow")
        {

        }

        [MenuItem("Fast/Server Client Testing")]
        public static void ShowWindow()
        {
            GetWindow<ServerClientTestingWindow>("Fast S-C Testing");
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

            GUILayout.Label("FAST SERVER-CLIENT TESTING", styles.MainTitleStyle);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Build folder", GUILayout.MaxWidth(105));

                EditorGUILayout.TextField(serialized_data.build_folder);

                if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
                {
                    string last_path = serialized_data.build_folder;

                    serialized_data.build_folder = EditorUtility.OpenFolderPanel("Select folder", "", "");

                    if (last_path != serialized_data.build_folder)
                    {
                        this.Repaint();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Build name", GUILayout.MaxWidth(105));

                serialized_data.build_name = EditorGUILayout.TextField(serialized_data.build_name);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Server arg", GUILayout.MaxWidth(105));

                serialized_data.server_arg = EditorGUILayout.TextField(serialized_data.server_arg);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Client arg", GUILayout.MaxWidth(105));

                serialized_data.client_arg = EditorGUILayout.TextField(serialized_data.client_arg);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Server", GUILayout.MaxWidth(105));

                serialized_data.server = EditorGUILayout.Toggle(serialized_data.server);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Clients", GUILayout.MaxWidth(105));

                serialized_data.clients = EditorGUILayout.IntField(serialized_data.clients);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Rebuild Server", GUILayout.MaxWidth(105));

                serialized_data.rebuild_server = EditorGUILayout.Toggle(serialized_data.rebuild_server);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Rebuild Client", GUILayout.MaxWidth(105));

                serialized_data.rebuild_client = EditorGUILayout.Toggle(serialized_data.rebuild_client);
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Start"))
            {
                server_build_filepath = serialized_data.build_folder + "/Server/" + serialized_data.build_name + ".exe";
                client_build_filepath = serialized_data.build_folder + "/Client/" + serialized_data.build_name + ".exe";

                UnityEditor.Build.Reporting.BuildReport ret = null;

                if (serialized_data.rebuild_server)
                {
                    ret = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, server_build_filepath,
                        BuildTarget.StandaloneWindows, BuildOptions.EnableHeadlessMode);
                }

                if(serialized_data.rebuild_client)
                {
                    if (ret == null || ret.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                    {
                        PlayerSettings.resizableWindow = true;
                        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;

                        ret = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, client_build_filepath,
                            BuildTarget.StandaloneWindows, BuildOptions.Development);
                    }
                }

                if ((!serialized_data.rebuild_server && !serialized_data.rebuild_client) 
                    || ret.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                {
                    if (serialized_data.server)
                    {
                        StartProcess(true);
                    }

                    for(int i = 0; i < serialized_data.clients; ++i)
                    {
                        StartProcess(false);
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void StartProcess(bool as_server)
        {
            Process myProcess = new Process();
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

            if (as_server)
            {
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.FileName = server_build_filepath;
                myProcess.StartInfo.Arguments = "-" + serialized_data.server_arg;
            }
            else
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.CreateNoWindow = false;
                myProcess.StartInfo.FileName = client_build_filepath;
                myProcess.StartInfo.Arguments = "-" + serialized_data.client_arg;
            }

            myProcess.EnableRaisingEvents = true;
            myProcess.Start();
        }
    }
}

#endif
