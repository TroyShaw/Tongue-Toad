using UnityEngine;
using System.Collections;

public class NewTongue : MonoBehaviour {

    private Player player;
    private PlayerSounds playerSounds;
    public bool tongueOut = false;
    public bool tongueRetractingEarly = false;
    AnimationState tongue;
    Transform toad;

    float tongueSpeed = 2f;


	// Use this for initialization
	void Start () {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        toad = p.transform;
        player = p.GetComponent<Player>();
        playerSounds = p.GetComponent<PlayerSounds>();
        tongue = animation["Tongue"];
        tongue.speed = tongueSpeed;
        tongue.wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Pause.isPaused || Time.timeScale == 0)
            return;
        bool shoot = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);
        
        if (shoot && !tongueOut && Player.State == PlayerAnimator.PlayerState.TongueIn)
        {
            Player.State = PlayerAnimator.PlayerState.TongueStarting;
        }
        else if (!shoot && Player.State == PlayerAnimator.PlayerState.TongueStarting)
        {
            Player.State = PlayerAnimator.PlayerState.TongueEnding;
        }
        else if (shoot && !tongueOut && Player.State == PlayerAnimator.PlayerState.TongueOut)
        {
            tongueOut = true;
            playerSounds.tongueStartSource.Play();
            playerSounds.tongueStretchSource.Play();
            animation.Play("Tongue");    
        }
        else if (tongueOut && !tongueRetractingEarly && shoot)
        {
            tongue.speed = tongueSpeed * TripMode.bonuses[TripMode.attackSpeed];
        }
        else if (tongueOut && !tongueRetractingEarly  && !shoot && tongue.time<=1.00f)
        {
            //Tongue back in early
            tongue.speed = -tongueSpeed * TripMode.bonuses[TripMode.attackSpeed];
            tongueRetractingEarly = true;
        }
        else if (tongueOut && !animation.isPlaying)
        {
            //Tongue back in
            tongueOut = false;
            tongueRetractingEarly = false;
            Player.State = PlayerAnimator.PlayerState.TongueEnding;
        }
        else if (!tongueOut && !shoot)
        {
            playerSounds.tongueStretchSource.Stop();

        }

    }
	
    void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.tag == "Enemy")
        {
            //get the enemy
            Enemy enemy = o.GetComponent<Enemy>();

            if (enemy.getState() != Enemy.EnemyState.IDLE) return;

            //direction of impact
			if(enemy.GetComponent<TetsudoAnimator>()==null){
            	Vector3 dir = Vector3.Normalize(-(toad.position - transform.position));
            	//fling the enemy back
                if (o.GetComponent<PlaneMovement>() == null)
                    o.GetComponent<Rigidbody>().velocity = (1.0f * dir);
                else
                    o.GetComponent<Rigidbody>().AddForce(0, 0, 10f);
			}

            //attack the enemy... death, animations, etc, are handled there.
            enemy.attack(player.getTongueDamage());

            //now tell the player they've attacked an enemy. That stuff is handled there
            player.attackEnemy(enemy);
        }
        else if (o.tag == "Friendly")
        {
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().activateSpin();
            player.tripMode.lickToad(o.transform.position);
            Destroy(o);
        }
        else if (o.tag == "Terrain")
        {
            tongueRetractingEarly = true;
            tongue.speed = -tongueSpeed * TripMode.bonuses[TripMode.attackSpeed];
        }
    }
}
