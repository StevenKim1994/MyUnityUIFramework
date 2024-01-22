using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigationController : Singleton<UINavigationController>
{
    private Dictionary<string, UINavigation> NavigationDictionary;

    public UINavigationController()
    {
        NavigationDictionary = new Dictionary<string, UINavigation>();
    }

    public void Add(string key, UINavigation navigation)
    {
        if (NavigationDictionary.ContainsKey(key) == false)
        {
            NavigationDictionary.Add(key, navigation);
        }
    }

    public void Remove(string key)
    {
        if(NavigationDictionary.ContainsKey(key))
        {
            NavigationDictionary.Remove(key);
        }
    }

    public UINavigation GetNavigation(string key)
    {
        UINavigation navigation = null;
        NavigationDictionary.TryGetValue(key, out navigation);
        return navigation;
    }
}
