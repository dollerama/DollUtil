using System.Collections;
using System.Collections.Generic;
using DollUtil;
using DollUtil.Attributes;
using UnityEngine;
using UnityEngine.UI;

/*
 ISyncable is only necessary if you want a [SyncVar] or [SaveData] to keep reference when synced.
 In this case we do not need this functionality but i have implimented as an example. If gamestate was needed to be referenced
 in a List or other container its reference will not be broken.
*/
[System.Serializable]
public class GameState : ISyncable
{
    public int state;
    public Color color;

    public GameState(int _state, Color _color)
    {
        state = _state;
        color = _color;
    }

    public void MapFrom(object r)
    {
        GameState tmp = r as GameState;
        state = tmp.state;
        color = tmp.color;
    }
}

public class PersistentObj : SyncBehaviour
{
    [SaveData]
    public GameState currentState;

    public GameState state1;
    public GameState state2;

    public override void Start()
    {
        base.Start();

        Organizer.Grab<Button>("State1").onClick.AddListener(State_1);
        Organizer.Grab<Button>("State2").onClick.AddListener(State_2);
        Organizer.Grab<TMPro.TextMeshProUGUI>("StateText").text = $"State:{currentState.state}";
    }

    public override void Update()
    {
        base.Update();

        var panel = Organizer.Grab<Image>("Panel");
        panel.color = Color.Lerp(panel.color, currentState.color, Time.deltaTime);
    }

    public void State_1()
    {
        if (currentState.state == state1.state) return;
        currentState = state1;
        Organizer.Grab<TMPro.TextMeshProUGUI>("StateText").text = $"State:{currentState.state}";
        SaveState.GrabFirst.Save();
    }

    public void State_2()
    {
        if (currentState.state == state2.state) return;
        currentState = state2;
        Organizer.Grab<TMPro.TextMeshProUGUI>("StateText").text = $"State:{currentState.state}";
        SaveState.GrabFirst.Save();
    }
}
