using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class GoapAgent : MonoBehaviour {
    public GoapPlannerManager goapPlannerManager = null;
    public FSM stateMachine;
    public IGoap dataProvider; // this is the implementing class that provides our world data and listens to feedback on planning

	void Start () {
        findDataProvider();

        goapPlannerManager = new GoapPlannerManager(this);
        stateMachine = new FSM(this);

        stateMachine.pushState(FSMStateEnum.Idle);
	}

	void Update () {

        goapPlannerManager.Update();

        stateMachine.Update();
    }

	private void findDataProvider() {
		foreach (Component comp in gameObject.GetComponents(typeof(Component)) ) {
			if ( typeof(IGoap).IsAssignableFrom(comp.GetType()) ) {
				dataProvider = (IGoap)comp;
				return;
			}
		}
	}

	public static string prettyPrint(Dictionary<string, bool> state) {
		String s = "";
		foreach (KeyValuePair<string, bool> kvp in state) {
			s += kvp.Key + ":" + kvp.Value.ToString();
			s += ", ";
		}
		return s;
	}

	public static string prettyPrint(Queue<GoapAction> actions) {
		String s = "";
		foreach (GoapAction a in actions) {
			s += a.GetType().Name;
			s += "-> ";
		}
		s += "GOAL";
		return s;
	}

	public static string prettyPrint(GoapAction[] actions) {
		String s = "";
		foreach (GoapAction a in actions) {
			s += a.GetType().Name;
			s += ", ";
		}
		return s;
	}

	public static string prettyPrint(GoapAction action) {
		String s = ""+action.GetType().Name;
		return s;
	}
}
