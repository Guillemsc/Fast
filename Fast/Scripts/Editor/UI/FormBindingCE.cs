#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

namespace Fast.Editor.UI
{
    [CustomEditor(typeof(Fast.UI.FormBinding), true)]
    [Sirenix.OdinInspector.HideMonoScript]
    class FormBindingCE : EditorHelper
    {
        private Fast.UI.FormBinding target_script = null;

        protected void OnEnable()
        {
            target_script = (Fast.UI.FormBinding)target;
        }

        protected override void OnDrawInspectorGUI()
        {
            EditorGUILayout.Separator();

            DrawBindableForm();

            DrawBindablePropertiesGUI();

            DrawOtherPropertiesGUI();

            EditorElements.HorizontalLine(Style);
        }

        private void DrawBindableForm()
        {
            target_script.BindAutomatically = EditorGUILayout.Toggle("Bind automatically", target_script.BindAutomatically);

            if (target_script.BindAutomatically)
            {
                if (target_script.BindedForm != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Bind: ", $"GameObject ► {target_script.BindedForm.gameObject.name} " +
                            $"| Form ► {target_script.BindedForm.GetType().Name}");
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Form could not be found on parent GameObjects. " +
                            "This is okay if the binding is going to be created dinamically");
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                target_script.BindedForm = EditorGUILayout.ObjectField("Form Bind", target_script.BindedForm, 
                    typeof(Fast.UI.BindableForm), true) as Fast.UI.BindableForm;
            }
        }

        private void DrawBindablePropertiesGUI()
        {
            target_script.UseCustomBindingProperties = EditorGUILayout.Toggle("Custom properties", 
                target_script.UseCustomBindingProperties);

            List<string> properties = null;

            if (target_script.UseCustomBindingProperties)
            {
                properties = GetCustomBindableProperties();
            }
            else
            {
                properties = GetDefaultBindableProperties();
            }

            if (properties != null)
            {               
               int selected_property = GetSelectedPropertyFromBindingName(properties);
               int new_selected_property = EditorGUILayout.Popup("Property ", selected_property, properties.ToArray());

                if (selected_property >= 0 && selected_property < properties.Count)
                {
                    string new_selected_property_name = properties[new_selected_property];

                    if (new_selected_property != selected_property || target_script.BindingName != new_selected_property_name)
                    {                        
                        target_script.BindingName = new_selected_property_name;

                        EditorUtility.SetDirty(target_script);
                    }
                }
            }
        }

        private void DrawOtherPropertiesGUI()
        {
            target_script.Delay = EditorGUILayout.FloatField("Delay", target_script.Delay);
        }

        private List<string> GetCustomBindableProperties()
        {
            if(target_script.BindedForm == null)
            {
                return null;
            }

            List<string> ret = new List<string>();

            BindingFlags bindingFlags = BindingFlags.Instance |
                   BindingFlags.NonPublic |
                   BindingFlags.Public;

            Type type = target_script.BindedForm.GetType();

            FieldInfo[] fields = type.GetFields(bindingFlags);

            for(int i = 0; i < fields.Length; ++i)
            {
                FieldInfo curr_field = fields[i];

                object[] attrs = curr_field.GetCustomAttributes(false);
                
                for(int y = 0; y < attrs.Length; ++y)
                {
                    if(attrs[y].GetType() == typeof(Fast.UI.BindingProperty))
                    {
                        ret.Add(curr_field.Name);

                        break;
                    }
                }
            }

            return ret;
        }

        private List<string> GetDefaultBindableProperties()
        {
            if (target_script.BindedForm == null)
            {
                return null;
            }

            List<string> ret = new List<string>();

            ret.Add(nameof(Fast.UI.BindableForm.OnFormAwake));
            ret.Add(nameof(Fast.UI.BindableForm.OnFormShow));
            ret.Add(nameof(Fast.UI.BindableForm.OnFormHide));

            return ret;

        }

        private int GetSelectedPropertyFromBindingName(List<string> properties)
        {
            if (properties == null)
            {
                return 0;
            }

            for (int i = 0; i < properties.Count; ++i)
            {
                if (properties[i] == target_script.BindingName)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}

#endif