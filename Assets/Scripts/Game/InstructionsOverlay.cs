using UnityEngine;
using System.Collections;

//class handles doing the overlay at the start for game instructions
public class InstructionsOverlay : MonoBehaviour
{
	public GUISkin skin;
	
	public Texture2D[] instructions;
	private int currentIndex;
	private Pause pause;
	private bool display; 
	
	// Use this for initialization
	void Start ()
	{
		if (!MainGui.doTutorial) {
			Destroy (this);
			return; 
		}
		
		//if (!TransitionCamera.isTransitioning) 
		displayInstructions();
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		if (!display)
			return;
		
		GUI.skin = skin;
		
		GUI.depth = -1;
		Time.timeScale = 0;
		//display the instructions texture and a button
		//when the button is clicked, enable Pause script, destroy this, then set timestep back to 1
		int w = Screen.width;
		int h = Screen.height;
		
		GUI.depth = -1;
		
		GUI.DrawTexture (new Rect (w * 0.1f, h * 0.1f, w * 0.8f, h * 0.8f), instructions [currentIndex]);
		
		Rect r = new Rect ((w - 200) / 2.0f, h * 0.8f, 200.0f, 30.0f);
		
		if (currentIndex == instructions.Length - 1) {
			//display only "start game"
			if (GUI.Button (r, "Start Game!"))
				gotoGame ();
		} else {
			//display "skip to game" "next" buttons"
			if (GUI.Button (r, "Next..."))
				currentIndex++;
		}
		
	}
	
	private void skipToGame()
	{
		GameObject.FindWithTag("EnemySpawn").GetComponent<EnemySpawn>().skipTutorial();
		gotoGame ();
	}

    private void gotoGame()
    {
        pause.enabled = true;
        Time.timeScale = 1;

        Destroy(this);
    }
	
	public void displayInstructions ()
	{
		display = true;
		GameObject gui = GameObject.FindGameObjectWithTag ("Gui");
		pause = gui.GetComponent<Pause> ();
		
		//stop them going into pause mode cause it could get screwy otherwise
		pause.enabled = false;
		
		//stop the time-step, otherwise enemies will move
		Time.timeScale = 0;
	}
				
}
