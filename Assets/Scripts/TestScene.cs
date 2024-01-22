using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    public TestUIView SceneTestUIView;
    // Start is called before the first frame update
    void Start()
    {
        SceneTestUIView.Initialize(UIViewTweeningType.OutBack, UIViewTweeningType.OutBack);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            SceneTestUIView.Show();
        }
        else if(Input.GetKey(KeyCode.Backspace))
        {
            SceneTestUIView.Hide();
        }
    }
}
