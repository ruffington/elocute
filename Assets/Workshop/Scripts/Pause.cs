using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Pause : MonoBehaviour {
	bool paused = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (this.gameObject.name=="Button_Pause");
		Debug.Log(EventSystem.current.name);
		if (EventSystem.current.currentSelectedGameObject != null) {
			Debug.Log("YAAS");
			Debug.Log (EventSystem.current.currentSelectedGameObject.name);
			if (EventSystem.current.currentSelectedGameObject.name.Equals ("Button_Pause"))
				paused = togglePause ();
		}
		//if (EventSystem.current.currentSelectedGameObject.name.Equals("Button_Pause")) {
		//	paused = togglePause();
		//}
	}

	void OnPointerClick() {
		Debug.Log("YAS BISH");
		//if(this.gameObject.name == "Button_Pause")
			//paused = togglePause ();
			//Debug.Log("YAS BISH");
	}

	void OnGUI() {
		if (paused) {
			GUILayout.Label ("Paused!");
			if(GUILayout.Button("Click me to unpause :-)")) {
				paused = togglePause();
			}
		}
	}

	bool togglePause() {
		if (Time.timeScale == 0f) {
			Time.timeScale = 1f;
			return(false);
		}
		Time.timeScale = 0f;
		return(true);
	}
}