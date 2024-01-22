using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIMapper
{
    private const char SLASH = '/';

    public static bool HasChildByPath(Transform _rootTransform, string _childPath)
    {
        bool isValid = false;
        if (_rootTransform != null && isValidPath(_childPath))
        {
            string[] split = _childPath.Split(SLASH);
            if (0 < split.Length)
            {
                Transform child = _rootTransform.Find(split[0]);
                if (child != null && 1 < split.Length)
                {
                    StringBuilder newPathStringBuilder = new StringBuilder();
                    for (int i = 1; i < split.Length; ++i)
                    {
                        newPathStringBuilder.Append(split[i]);
                        if (i < split.Length - 1) newPathStringBuilder.Append(SLASH);
                    }

                    isValid = HasChildByPath(child, newPathStringBuilder.ToString());
                }
                else
                {
                    Stack<string> rootStack = new Stack<string>();
                    Transform parent = _rootTransform;
                    while (parent != null)
                    {
                        rootStack.Push(parent.name);
                        parent = parent.parent;
                    }

                    StringBuilder rootPath = new StringBuilder();
                    while (0 < rootStack.Count)
                    {
                        rootPath.Append(rootStack.Pop());
                        if (0 < rootStack.Count) rootPath.Append(SLASH.ToString());
                    }

                    if (child == null)
                    {
                        isValid = false;
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }
        }

        return isValid;
    }

    public static GameObject GetChildByPath(Transform _rootTransform, string _childPath)
    {
        GameObject result = null;
        if (_rootTransform != null && isValidPath(_childPath))
        {
            string[] split = _childPath.Split(SLASH);
            if (0 < split.Length)
            {
                Transform child = _rootTransform.Find(split[0]);
                if (child != null && 1 < split.Length)
                {
                    StringBuilder newPathStringBuilder = new StringBuilder();
                    for (int i = 1; i < split.Length; ++i)
                    {
                        newPathStringBuilder.Append(split[i]);
                        if (i < split.Length - 1) newPathStringBuilder.Append(SLASH);
                    }

                    result = GetChildByPath(child, newPathStringBuilder.ToString());
                }
                else
                {
                    Stack<string> rootStack = new Stack<string>();
                    Transform parent = _rootTransform;
                    while (parent != null)
                    {
                        rootStack.Push(parent.name);
                        parent = parent.parent;
                    }

                    StringBuilder rootPath = new StringBuilder();
                    while (0 < rootStack.Count)
                    {
                        rootPath.Append(rootStack.Pop());
                        if (0 < rootStack.Count) rootPath.Append(SLASH.ToString());
                    }

                    if (child == null)
                    {
                        Debug.LogWarning("UIMapper] GetchildByPath) \"" + _childPath + "\" Not Found..." + rootPath.ToString() + ">.");
                    }
                    else
                    {
                        result = child.gameObject;
                    }
                }
            }
        }

        return result;
    }

    public static TUIObject GetThisComponent<TUIObject>(Transform _this) where TUIObject : Component
    {
        TUIObject result = null;
        result = _this.GetComponent<TUIObject>();
        if (result == null) result = _this.gameObject.AddComponent<TUIObject>();
        return result;
    }

    public static TUIObject GetChildByPath<TUIObject>(Transform _rootTransform, string _childPath) where TUIObject : Component
    {
        TUIObject result = null;
        GameObject child = GetChildByPath(_rootTransform, _childPath);
        if (child != null)
        {
            bool isActive = child.gameObject.activeSelf;
            child.gameObject.SetActive(true);
            result = child.GetComponent<TUIObject>();
            if (result == null) result = child.AddComponent<TUIObject>();
            child.gameObject.SetActive(isActive);
        }

        return result;
    }

    private static bool isValidPath(string _path)
    {
        return string.IsNullOrEmpty(_path) == false && _path[_path.Length - 1] != SLASH;
    }
}
