﻿/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using UnityEngine.UI;
using XLua;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class MessageBox : MonoBehaviour{

    public static void ShowAlertBox(string message, string title, Action onFinished = null)
    {
        var alertPanel = GameObject.Find("Canvas").transform.Find("AlertBox");
        if (alertPanel == null)
        {
            alertPanel = (Instantiate(Resources.Load("AlertBox")) as GameObject).transform;
            alertPanel.gameObject.name = "AlertBox";
            alertPanel.SetParent(GameObject.Find("Canvas").transform);
            alertPanel.localPosition = new Vector3(-6f, -6f, 0f);
        }
        alertPanel.Find("title").GetComponent<Text>().text = title;
        alertPanel.Find("message").GetComponent<Text>().text = message;
        alertPanel.gameObject.SetActive(true);
        if (onFinished != null)
        {
            var button = alertPanel.Find("alertBtn").GetComponent<Button>();
            UnityAction onclick = null;
            onclick = () =>
            {
                onFinished();
                alertPanel.gameObject.SetActive(false);
                button.onClick.RemoveListener(onclick);
            };
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onclick);
        }
    }

    public  static void ShowConfirmBox(string message, string title, Action<bool> onFinished = null)
    {
        var confirmPanel = GameObject.Find("Canvas").transform.Find("ConfirmBox");
        if (confirmPanel == null)
        {
            confirmPanel = (Instantiate(Resources.Load("ConfirmBox")) as GameObject).transform;
            confirmPanel.gameObject.name = "ConfirmBox";
            confirmPanel.SetParent(GameObject.Find("Canvas").transform);
            confirmPanel.localPosition = new Vector3(-8f, -18f, 0f);
        }
        confirmPanel.Find("confirmTitle").GetComponent<Text>().text = title;
        confirmPanel.Find("conmessage").GetComponent<Text>().text = message;
        confirmPanel.gameObject.SetActive(true);
        if (onFinished != null)
        {
            var confirmBtn = confirmPanel.Find("confirmBtn").GetComponent<Button>();
            var cancelBtn = confirmPanel.Find("cancelBtn").GetComponent<Button>();
            UnityAction onconfirm = null;
            UnityAction oncancel = null;

            Action cleanup = () =>
            {
                confirmBtn.onClick.RemoveListener(onconfirm);
                cancelBtn.onClick.RemoveListener(oncancel);
                confirmPanel.gameObject.SetActive(false);
            };

            onconfirm = () =>
            {
                onFinished(true);
                cleanup();
            };

            oncancel = () =>
            {
                onFinished(false);
                cleanup();
            };
            confirmBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.RemoveAllListeners();
            confirmBtn.onClick.AddListener(onconfirm);
            cancelBtn.onClick.AddListener(oncancel);
        }
    }
}

public static class MessageBoxConfig
{
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Action),
        typeof(Action<bool>),
        typeof(UnityAction),
    };
}

