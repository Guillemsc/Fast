using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class Toggle : MonoBehaviour
{
    [SerializeField] Button button = null;
    [SerializeField] Image button_image = null;
    [SerializeField] Image toggle_image = null;

    private bool is_on = true;

#if UNITY_EDITOR

    [MenuItem("CONTEXT/GameObject/Test1")]
    private void CreateToogle()
    {

    }

#endif

private void Awake()
    {
        if(button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        if(toggle_image != null)
        {
            SetIsOn(is_on);
        }
    }

    private void OnButtonClick()
    {
        SetIsOn(!is_on);
    }

    public bool IsOn
    {
        get { return is_on; }
        set
        {
            SetIsOn(value);
        }
    }

    private void SetIsOn(bool set)
    {
        is_on = set;

        toggle_image.enabled = is_on;
    }
}
