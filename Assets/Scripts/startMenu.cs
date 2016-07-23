using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class startMenu : MonoBehaviour 
{
	public Canvas MainCanvas;
	public Canvas InstructionsCanvas;

	void Awake()
	{
		InstructionsCanvas.GetComponent<Canvas> ().enabled = false;
		InstructionsCanvas.transform.position = new Vector3 (80f, 0.5f, 84.5f);
	}

	public void InstructionsOn()
	{
		InstructionsCanvas.GetComponent<Canvas> ().enabled = true;
		InstructionsCanvas.transform.position = new Vector3 (0f, 0.5f, 84.5f);
		MainCanvas.GetComponent<Canvas> ().enabled = false;
		MainCanvas.transform.position = new Vector3 (80f, 0.5f, 84.5f);
	}
	public void BackOn()
	{
		InstructionsCanvas.GetComponent<Canvas> ().enabled = false;
		InstructionsCanvas.transform.position = new Vector3 (80f, .5f, 84.5f);
		MainCanvas.GetComponent<Canvas> ().enabled = true;
		MainCanvas.transform.position = new Vector3 (0f, 0.5f, 84.5f);
	}

	public void LoadOn()
	{
		Application.LoadLevel (1);
	}
	public void QuitOn()
	{
		Application.Quit ();
	}

}
