using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Fast.Prefabs.DebugConsole
{
    class DebugConsole : MonoBehaviour
    {
        [SerializeField] private string toggle_key = "f1";

        [SerializeField] private GameObject console_parent = null;

        [SerializeField] private Button warnings_button = null;
        [SerializeField] private Button errors_button = null;
        [SerializeField] private Button logs_button = null;

        [SerializeField] private GameObject console_logs_parent = null;
        [SerializeField] private TMPro.TextMeshProUGUI console_log_prefab = null;

        private List<TMPro.TextMeshProUGUI> console_logs_instances = new List<TMPro.TextMeshProUGUI>();

        private List<DebugConsoleLog> console_logs = new List<DebugConsoleLog>();

        private List<UnityEngine.LogType> log_types_showing = new List<LogType>(); 

        private void Awake()
        {
            console_parent.SetActive(false);

            Application.logMessageReceived += OnLogCallback;

            InitButtons();
        }

        private void Update()
        {
            UpdateToggle();
        }

        private void InitButtons()
        {
            warnings_button.onClick.AddListener(OnWarningsButtonDown);
            errors_button.onClick.AddListener(OnErrorsButtonDown);
            logs_button.onClick.AddListener(OnLogsButtonDown);
        }

        private void OnLogCallback(string condition, string stack_trace, UnityEngine.LogType type)
        {
            DebugConsoleLog new_log = new DebugConsoleLog(condition, stack_trace, type);

            lock(console_logs)
            {
                console_logs.Add(new_log);

                UpdateLogInstance(new_log);
            }
        }

        private void UpdateToggle()
        {
            if(Input.GetKeyDown(toggle_key))
            {
                console_parent.SetActive(!console_parent.activeSelf);
            }
        }

        private void OnWarningsButtonDown()
        {
            log_types_showing.Clear();

            log_types_showing.Add(UnityEngine.LogType.Warning);

            UpdateLogsInstances();
        }

        private void OnErrorsButtonDown()
        {
            log_types_showing.Clear();

            log_types_showing.Add(UnityEngine.LogType.Error);
            log_types_showing.Add(UnityEngine.LogType.Assert);

            UpdateLogsInstances();
        }

        private void OnLogsButtonDown()
        {
            log_types_showing.Clear();

            log_types_showing.Add(UnityEngine.LogType.Log);

            UpdateLogsInstances();
        }

        private void UpdateLogsInstances()
        {
            ClearLogsInstances();

            for(int i = 0; i < log_types_showing.Count; ++i)
            {
                AddLogsInstances(log_types_showing[i]);
            }
        }

        private void UpdateLogInstance(DebugConsoleLog instance)
        {
            for (int i = 0; i < log_types_showing.Count; ++i)
            {
                if (log_types_showing[i] == instance.Type)
                {
                    CreateLogInstace(instance);

                    break;
                }
            }
        }

        private void AddLogsInstances(UnityEngine.LogType types)
        {
            for (int i = 0; i < console_logs.Count; ++i)
            {
                DebugConsoleLog curr_log = console_logs[i];

                if (curr_log.Type == types)
                {
                    CreateLogInstace(curr_log);
                }
            }
        }

        private void CreateLogInstace(DebugConsoleLog log)
        {
            GameObject instance = Instantiate(console_log_prefab.gameObject, Vector3.zero, Quaternion.identity);

            instance.transform.SetParent(console_logs_parent.transform);

            instance.transform.localScale = Vector3.one;

            TMPro.TextMeshProUGUI text_instance = instance.GetComponent<TMPro.TextMeshProUGUI>();

            text_instance.text = "- [" + log.Type.ToString() + "]: " + log.Condition;

            lock (console_logs_instances)
            {
                console_logs_instances.Add(text_instance);
            }
        }

        private void ClearLogsInstances()
        {
            lock (console_logs_instances)
            {
                for (int i = 0; i < console_logs_instances.Count; ++i)
                {
                    Destroy(console_logs_instances[i].gameObject);
                }

                console_logs_instances.Clear();
            }
        }
    }
}
