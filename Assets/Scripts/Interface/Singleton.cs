using UnityEngine;

public class Singleton<T> where T : new()
{
    private static T Instance = default(T);

    public static T GetInstance()
    {
         if(Instance == null)
         {
            Instance = new T();
         }
         return Instance;
    }
}

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : UnityEngine.Component
{
    private static T Instance;

    public static T GetInstance()
    {
        if(Instance == null)
        {
            Instance = FindObjectOfType<T>(true);
            if(Instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;

                Instance = obj.AddComponent<T>();
            }
        }

        return Instance;
    }
}
