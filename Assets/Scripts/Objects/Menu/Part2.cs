using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Part2 : MonoBehaviour
{
    public Button partTwo;
    // Start is called before the first frame update
    void Start()
    {
        Button button2 = partTwo.GetComponent<Button>();
        UnityEngine.Debug.Log("Button 2 Registered");
        button2.onClick.AddListener(LoadPart2);
        UnityEngine.Debug.Log("Listener Added Button 2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadPart2()
    {
        UnityEngine.Debug.Log("Button 2 Clicked");
        SceneManager.LoadScene("Part2Scene1");
    }
}
