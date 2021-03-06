using UnityEngine;
using System.Collections;

public class TripMode : MonoBehaviour
{

    public const int attackSpeed = 0;
    public const int shellDropMultiplier = 1;
    public const int pointsMultiplier = 2;
    public const int comboMultiplier = 3;
    public const int movementMultiplier = 4;
    public const int enemySpeed = 5;

    public GameObject doubleScore, doubleShell, extraSpeed, fasterAttack, slowMotion, tripleCombo;

    public static float[] bonuses;

    float[] timesLeft = new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    bool[] activated = new bool[] { false, false, false, false, false, false };
    public bool blur = false;

    CameraControl cam;

    bool cameraSpinning = false;

    public int toadsLicked = 0;
    private int tripMode = 0;

    private Player player;

    public bool showGUI = false;

    public void Start()
    {
        player = GetComponent<Player>();
        bonuses = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
    }

    //returns the players trip mode. Is either between 0-3
    public int getTripMode()
    {
        return Mathf.Clamp(tripMode, 0, 3);

    }

    public void lickToad(Vector3 toadPosition)
    {
        player.setTripping(true);
        if (toadsLicked == 0 || toadsLicked == 1)
        {
            toadsLicked++;
            tripMode = toadsLicked;

            if (toadsLicked == 1)
            {
                cam.pulseDirection = 1;
            }
            else if (toadsLicked == 2)
            {
                blur = true;
            }

            int i = Random.Range(0, bonuses.Length);
            switch (i)
            {
                case attackSpeed:
                    create(fasterAttack, toadPosition);
                    break;
                case movementMultiplier:
                    create(extraSpeed, toadPosition);
                    bonuses[i] *= 1.5f;
                    break;
                case shellDropMultiplier:
                    create(doubleShell, toadPosition);
                    break;
                case pointsMultiplier:
                    create(doubleScore, toadPosition);
                    bonuses[i] *= 2;
                    break;
                case comboMultiplier:
                    create(tripleCombo, toadPosition);
                    bonuses[i] *= 3;
                    break;
                case enemySpeed:
                    create(slowMotion, toadPosition);
                    bonuses[i] *= 0.8f;
                    break;
            }
            timesLeft[i] += 10.0f;
            activated[i] = true;
        }
        else if (toadsLicked == 2)
        {
            toadsLicked++;
            cam.pulseDirection = 0;
            tripMode = 3;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().activateSpin();
            cameraSpinning = true;
        }
    }

    private void create(GameObject o, Vector3 pos)
    {
        pos.y += 0.5f;
        Instantiate(o, pos, Quaternion.Euler(90, -180, 0));
    }

    public void OnGUI()
    {
        if (showGUI)
        {
            GUI.Label(new Rect(400, 5, 300, 30), " Attack Speed: " + bonuses[0] + " Time: " + timesLeft[0]);
            GUI.Label(new Rect(400, 35, 300, 30), "   Shell Drop: " + bonuses[1] + " Time: " + timesLeft[1]);
            GUI.Label(new Rect(400, 65, 300, 30), " Points Multi: " + bonuses[2] + " Time: " + timesLeft[2]);
            GUI.Label(new Rect(400, 95, 300, 30), "  Combo Multi: " + bonuses[3] + " Time: " + timesLeft[3]);
            GUI.Label(new Rect(400, 125, 300, 30), "   Move Multi: " + bonuses[4] + " Time: " + timesLeft[4]);
            GUI.Label(new Rect(400, 155, 300, 30), "  Enemy Speed: " + bonuses[5] + " Time: " + timesLeft[5]);
            GUI.Label(new Rect(400, 185, 300, 30), "Toads Licked : " + toadsLicked);
        }
    }

    public void finishCameraSpin()
    {
        cameraSpinning = false;
        

    }

    public void Update()
    {
        int deactivated = 0;
        for (int i = 0; i < timesLeft.Length; i++)
        {
            if (activated[i])
            {
                float f = timesLeft[i];
                f -= Time.deltaTime;
                timesLeft[i] = f;

                if (f <= 0.0f)
                {
                    bonuses[i] = 1.0f;
                    timesLeft[i] = 0.0f;
                    activated[i] = false;
                    toadsLicked--;
                    if(!cameraSpinning)
                        tripMode = toadsLicked;
                }
            }
            else
                deactivated++;
        }
        if (deactivated == timesLeft.Length && !cameraSpinning)
        {
            player.setTripping(false);
            toadsLicked = 0;
            tripMode = 0;
            cam.pulseDirection = 0;
            blur = false;
        }
    }
}
