using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Part1 : MonoBehaviour
{
    public Button partOne;
    // Start is called before the first frame update
    void Start()
    {
        Button button1 = partOne.GetComponent<Button>();
        button1.onClick.AddListener(LoadPart1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadPart1()
    {
        UnityEngine.Debug.Log("Button clicked");
        SceneManager.LoadScene("Scenes/Tutorial/Tutorial");
    }
}
