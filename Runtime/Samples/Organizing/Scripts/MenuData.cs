using System.Collections;
using System.Collections.Generic;
using DollUtil;
using UnityEngine;
using UnityEngine.UI;

public class MenuData : MonoBehaviour
{
    public string title;
    public string play_btn;
    public string exit_btn;

    // Start is called before the first frame update
    void Start()
    {
        Organizer.Grab<TMPro.TextMeshProUGUI>("PlayText").text = play_btn;
        Organizer.Grab<TMPro.TextMeshProUGUI>("ExitText").text = exit_btn;
        Organizer.Grab<TMPro.TextMeshProUGUI>("Title").text = title;

        Organizer.Grab<Button>("Play").onClick.AddListener(Play);
        Organizer.Grab<Button>("Exit").onClick.AddListener(Exit);
    }

    public void Play()
    {
        Debug.Log("Play");
    }

    public void Exit()
    {
        Debug.Log("Exit");
    }
}
