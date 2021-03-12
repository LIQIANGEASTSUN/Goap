using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
#pragma warning disable 0414
public class EventTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    //UIPanelState state = UIPanelState.None;
    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        EventManage.Instance.AddEventListener(1, OnEvent);
    //    }

    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        EventManage.Instance.RemoveEventLister(1, OnEvent);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        EventManage.Instance.DispachEvent(1, 100);
    //    }
    //}

    public void OnEvent(int type, object data)
    {
        int number = (int)data;
        Debug.LogError("number : " + number);
    }
}
