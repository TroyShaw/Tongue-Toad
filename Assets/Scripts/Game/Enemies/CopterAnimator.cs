using UnityEngine;
using System.Collections;

/// <summary>
/// Class handles animations for the flying snails.
/// </summary>
public class CopterAnimator : MonoBehaviour {

	private Enemy enemy;
	
	private AnimationState idle, death;
    private string idleAnim = "";
	private string[] idleString = {"flyingIdle1","flyingIdle2"};
	//private string deathString = "Death1";
	
	bool playedDeath = false;
	// Use this for initialization
	void Start () {
		enemy = GetComponent<Enemy>();
        idleAnim = idleString[Random.Range(0,idleString.Length)];
		idle = animation[idleAnim];
		//death = animation[deathString];
		//idle.time = Random.Range(0.0f, idle.length);

		//death.wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void Update () {
		if (enemy.getState() == Enemy.EnemyState.IDLE)
		{
            animation.Play(idleAnim);
		} else if (enemy.getState () == Enemy.EnemyState.DYING && !playedDeath)
		{
            animation.Play(idleString[Random.Range(0, idleString.Length)]);	
			playedDeath = true;
		}
	}
		
}