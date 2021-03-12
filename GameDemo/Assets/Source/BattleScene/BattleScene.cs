using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        OnFrame();
	}

    private void OnFrame()
    {
        if (Room.Current != null)
        {
            Room.Current.OnFrame();
        }
    }
}
