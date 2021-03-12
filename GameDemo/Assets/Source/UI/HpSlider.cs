using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour {

    private Slider slider = null;

	// Use this for initialization
	void Start () {
        slider = ToolsComponent.FindChildCom<Slider>(transform, "Slider");
	}
	
    public void SetValue(float value)
    {
        if (slider != null)
        {
            slider.value = value;
        }
    }
}