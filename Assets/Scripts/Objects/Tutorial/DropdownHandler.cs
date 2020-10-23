using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropdownHandler : MonoBehaviour
{
    public GameObject exploreButton;
    public GameObject mode;
    private Text mod;
    bool init = true;
    // Start is called before the first frame update
    void Start()
    {
        mod = mode.GetComponent<Text>();
        mod.text = "Mode";
        var dropdown = transform.GetComponent<Dropdown>();
        //dropdown.options.Clear();
        List<string> items = new List<string>();
        items.Add("None");
        items.Add("Create");
        items.Add("Edit");
        foreach(var item in items){
            dropdown.options.Add(new Dropdown.OptionData(){text = item});
        }
        DropdownItemSelected(dropdown);
        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown);});
    }

    void DropdownItemSelected(Dropdown dropdown){
        int index = dropdown.value;
        Debug.Log(dropdown.options[index].text);
        if (dropdown.options[index].text == "Create" && !init){
            if (exploreButton.GetComponent<ExploreButton>().isExploring){
                exploreButton.GetComponent<Button>().onClick.Invoke();
            }
            //Player.instance.activeMode = InputMode.NONE;
            Player.instance.activeMode = InputMode.CREATE;
            mod.text = "Create";
        }
        else if (dropdown.options[index].text == "None" && !init){
            if (exploreButton.GetComponent<ExploreButton>().isExploring){
                exploreButton.GetComponent<Button>().onClick.Invoke();
            }
            Player.instance.activeMode = InputMode.NONE;
            mod.text = "None";
        }
        else if (dropdown.options[index].text == "Edit" && !init){
            if (exploreButton.GetComponent<ExploreButton>().isExploring){
                exploreButton.GetComponent<Button>().onClick.Invoke();
            }
            //Player.instance.activeMode = InputMode.NONE;
            Player.instance.activeMode = InputMode.EDIT;
            mod.text = "Edit";
        }
        else if (dropdown.options[index].text == "None" && init){
            Player.instance.activeMode = InputMode.NONE;
            mod.text = "Mode";
            init = false;
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
