using UnityEngine;
using System.Collections;

public class TongueBody : MonoBehaviour {
	
	Player player;
    Tongue tongue;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        tongue = player.GetComponentInChildren<Tongue>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
		return;
        GameObject o = c.gameObject;
        //Debug.Log("TongueBody collided with " + o.tag);
        if (o.tag == "Enemy" )
        {
            //get the enemy
            Enemy enemy = o.GetComponent<Enemy>();
			
			if (enemy.getState() != Enemy.EnemyState.IDLE) return;

            //direction of impact
			Vector3 dir = Vector3.Normalize(o.transform.position - transform.position);
            //Debug.Log (dir);
            //fling the enemy back
            o.GetComponent<Rigidbody>().velocity = (3.0f * dir);

            //attack the enemy... death, animations, etc, are handled there.
            enemy.attack(player.getTongueDamage());

            //now tell the player they've attacked an enemy. That stuff is handled there
            player.attackEnemy(enemy);
        }
        else if (o.tag == "Friendly")
        {
            //Debug.Log("TongueBody collided with " + o.tag);

            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().activateSpin();

            player.tripMode.lickToad(o.transform.position);

            Destroy(o);
        }
    }
}
