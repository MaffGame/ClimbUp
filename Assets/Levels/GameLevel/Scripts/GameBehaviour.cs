using UnityEngine;
using System;
using TMPro;

public class GameBehaviour : MonoBehaviour
{
    public GameObject player;
    public GameObject tree;
    public GameObject treeItem;
    public Canvas gameInterface;
    public float startGameSpeed;
    public int gameDificult;
    public bool runGame;

    private EventHandler gameEvent;
    private Animator playerAnimator;
    private float gameTime = 0;

    [SerializeField]
    private float points = 0;
    private float lastUpdateTime = 0;

    [SerializeField]
    private float gameSpeed;

    void Start() 
    {
        tree = Instantiate(tree, new Vector3(0, 0, 0), Quaternion.identity);        
        player = Instantiate(player, new Vector3(0, 0, -14.7f), Quaternion.Euler(-100, 0, 0));
        gameInterface = Instantiate(gameInterface);
        gameInterface.worldCamera = player.GetComponentInChildren<Camera>();
        playerAnimator = player.GetComponent<Animator>();
        gameEvent += ScaleGameSpeed;
        gameEvent += ScalePoints;
        gameEvent += MoveLevel;
        gameEvent += HandleInput;
        gameEvent += Continue;
        gameEvent += MovePlayer;
        runGame = true;
    }

    void FixedUpdate()
    {
        Pause();
        if(runGame)
            gameEvent.Invoke(this, EventArgs.Empty);
        else 
        {
            playerAnimator.Play("Idle", 0);
        }
    }

    void ScaleGameSpeed(object sender, EventArgs e)
    {
        gameSpeed = startGameSpeed + gameDificult * (float)Math.Sqrt(gameTime += Time.deltaTime);
    }

    void ScalePoints(object sender, EventArgs e)
    {
        var pointsField = gameInterface.GetComponentInChildren<TextMeshProUGUI>(); 
        points += gameSpeed * gameDificult * Time.deltaTime / 10;
        pointsField.text = ((int)points).ToString();
    }

    void MoveLevel(object sender, EventArgs e)
    {
        tree.transform.Translate(Vector3.down * gameSpeed * Time.deltaTime);
    }

    void MovePlayer(object sender, EventArgs e)
    {
        playerAnimator.Play("Run Forward In Place", 0);
    }

    

    void HandleInput(object sender, EventArgs e)
    {
        if(Input.GetKey(KeyCode.D))
        {
            // if(Input.GetKeyDown(KeyCode.Space))
            // {
            //     gameEvent -= MovePlayer;
            //     playerAnimator.Play("Strafe Right In Place", 0, 1);
            //     tree.transform.Rotate(new Vector3(0, gameSpeed, 0));
            //     gameEvent += MovePlayer;
            // }
            // else
                tree.transform.Rotate(new Vector3(0, gameSpeed/20, 0));
        }
        if(Input.GetKey(KeyCode.A))
        {
            // if(Input.GetKeyDown(KeyCode.Space))
            // {
            //     gameEvent -= MovePlayer;
            //     playerAnimator.Play("Strafe Left In Place", 0, 1);
            //     tree.transform.Rotate(new Vector3(0, -gameSpeed, 0));
            //     gameEvent += MovePlayer;
            // }
            // else
                tree.transform.Rotate(new Vector3(0, -gameSpeed/20, 0));
        }
    }

    void Continue(object sender, EventArgs e)
    {
        lastUpdateTime += Time.deltaTime;
        if(lastUpdateTime > 60 / gameSpeed)
        {
            lastUpdateTime = 0;
            Destroy(tree.transform.GetChild(0).gameObject);
            Instantiate(
                treeItem,
                new Vector3(0, 180 - gameSpeed * Time.deltaTime, 0),
                Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0),
                tree.transform
            );
        }
    }

    void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            runGame = !runGame;
        }
    }
}
