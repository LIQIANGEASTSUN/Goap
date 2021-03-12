
using UnityEngine;
using System.Collections.Generic;

public abstract class GoapAction {
    protected Transform transform;

	private Dictionary<string, bool> preconditions;
	private Dictionary<string, bool> effects;

	private bool inRange = false;

	/* The cost of performing the action. 
	 * Figure out a weight that suits the action. 
	 * Changing it will affect what actions are chosen during planning.*/
	public float cost = 1f;

	/**
	 * An action often has to perform on an object. This is that object. Can be null. */
	public GameObject target;

	public GoapAction(Transform tr) {
        transform = tr;
		preconditions = new Dictionary<string, bool>();
		effects = new Dictionary<string, bool>();
	}

	public void doReset() {
		inRange = false;
		target = null;
		reset ();
	}

	/**
	 * Reset any variables that need to be reset before planning happens again.
	 */
	public abstract void reset();

	/**
	 * Is the action done?
	 */
	public abstract bool isDone();

	/**
	 * Procedurally check if this action can run. Not all actions
	 * will need this, but some might.
	 */
	public abstract bool checkProceduralPrecondition(GameObject agent);

	/**
	 * Run the action.
	 * Returns True if the action performed successfully or false
	 * if something happened and it can no longer perform. In this case
	 * the action queue should clear out and the goal cannot be reached.
	 */
	public abstract bool perform(GameObject agent);

	/**
	 * Does this action need to be within range of a target game object?
	 * If not then the moveTo state will not need to run for this action.
	 */
	public abstract bool requiresInRange ();
	

	/**
	 * Are we in range of the target?
	 * The MoveTo state will set this and it gets reset each time this action is performed.
	 */
	public bool isInRange () {
		return inRange;
	}
	
	public void setInRange(bool inRange) {
		this.inRange = inRange;
	}


	public void addPrecondition(string key, bool value) {
        if (!preconditions.ContainsKey(key))
        {
            preconditions.Add(key, value);
        }
	}

	public void removePrecondition(string key) {
        if (preconditions.ContainsKey(key))
        {
            preconditions.Remove(key);
        }
	}

	public void addEffect(string key, bool value) {
		effects[key] = value;
	}

	public void removeEffect(string key) {
        if (effects.ContainsKey(key))
        {
            effects.Remove(key);
        }
	}

	
	public Dictionary<string, bool> Preconditions {
		get {
			return preconditions;
		}
	}

	public Dictionary<string, bool> Effects {
		get {
			return effects;
		}
	}
}