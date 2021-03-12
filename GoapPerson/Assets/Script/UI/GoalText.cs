using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Goap;
using System.Text;

public class GoalText : MonoBehaviour {
    public static GoalText instance = null;

    private GoapGoal goapGoal;

    public Text goalText = null;
    public Text worldStatusText = null;

	// Use this for initialization
	void Start () {
        instance = this;
	}

    StringBuilder sb = new StringBuilder();
    // Update is called once per frame
    void Update () {
		if (goapGoal == null )
        {
            return;
        }

        GetWorldStatus();
        GetGoal();
    }

    private void GetWorldStatus()
    {
        GoapStatus goapStatus = goapGoal.GetWorldStatus();
        Dictionary<GoapCondition, object> statusDic = goapStatus.Status();

        sb.Remove(0, sb.Length);

        sb.AppendLine("WorldStatus:");
        foreach (var pair in statusDic)
        {
            string status = string.Format("{0}:{1}", pair.Key.ToString(), pair.Value.ToString());
            sb.AppendLine(status);
        }

        SetText(worldStatusText, sb.ToString());
    }

    private void GetGoal()
    {
        GoapStatus goapStatus = goapGoal.GetGoalStatus();
        Dictionary<GoapCondition, object> statusDic = goapStatus.Status();

        sb.Remove(0, sb.Length);

        sb.AppendLine("Goal:");
        foreach (var pair in statusDic)
        {
            string status = string.Format("{0}:{1}", pair.Key.ToString(), pair.Value.ToString());
            sb.AppendLine(status);
        }

        SetText(goalText, sb.ToString());
    }

    private void SetText( Text text, string content)
    {
        if (text != null)
        {
            text.text = content;
        }
    }

    public void SetGoapGoal(GoapGoal goapGoal)
    {
        this.goapGoal = goapGoal;
    }
}
