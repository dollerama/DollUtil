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
        Organizer.Grab<TMPro.TextMeshProUGUI>("UI", "PlayText").text = play_btn;
        Organizer.Grab<TMPro.TextMeshProUGUI>("UI", "ExitText").text = exit_btn;
        Organizer.Grab<TMPro.TextMeshProUGUI>("UI", "Title").text = title;

        Organizer.Grab<Button>("UI", "Play").onClick.AddListener(Play);
        Organizer.Grab<Button>("UI", "Exit").onClick.AddListener(Exit);
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
