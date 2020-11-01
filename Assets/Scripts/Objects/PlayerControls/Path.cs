using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Path : MonoBehaviour
{
    public Button button;
    public string scenePath;
    string scene1;
    // Start is called before the first frame update
    void Start()
    {
        Button button1 = button.GetComponent<Button>();
        button1.onClick.AddListener(loadScene);
    }

    void loadScene()
    {
        UnityEngine.Debug.Log("Button clicked");
        SceneManager.LoadScene(scenePath);
    }
}
