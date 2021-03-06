using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Transform> seats;
    public List<Transform> playerSeats;
    public List<GameObject> allPlayersInScene;
    public GameObject playerPrefab, cardPrefab;
    public int gameMoney, indexDifferenceFromTableCenter;
    public Text timer;
    GameObject defaultObject, myPlayer, cardOutOfDeck;
    int RandomIndex;
    Transform playerPosition,  temp;
    [SerializeField] Transform deckPos;
    bool playerSelected, rearrangementTimer, cardDistributed, _1stCardDistributed;
    RaycastHit2D hit;
    float rearrangementTime = 5f, gameTime = 20f;
    int wholeNoTime;
    // Start is called before the first frame update
    void Start()
    {
        DefaultPlayerManager();
    }

    // Update is called once per frame
    void Update()
    {
        allPlayersInScene = GameObject.FindGameObjectsWithTag("Player").ToList();
        if(Input.GetMouseButtonDown(0))
        {
            if (!playerSelected)  // get the data 
            {
                playerSelected = true;
                GeneratePlayerAtClickPos();
            }
            Invoke("Rearrangement", 1f); // invoke the rearrange ment function after 2 sec
            rearrangementTimer = true;


        }
        if (rearrangementTimer)
        {
            rearrangementTime -= Time.deltaTime; // enable timer after the rearrangement of players
            wholeNoTime = System.Convert.ToInt32(rearrangementTime);
            timer.text = "Time : " + wholeNoTime.ToString();
        }
        if(rearrangementTime <= 0 )
        {
            if(!_1stCardDistributed)
            {
                _1stCardDistributed = true;
                distributeCardsToPlayerInScene();
            }
            // start game
        }
        if(cardDistributed)
        {
            moveCardToPlayer(temp);
        }
    }

    private void GeneratePlayerAtClickPos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject myPlayer = Instantiate(playerPrefab, hit.collider.gameObject.transform.position, Quaternion.identity);
            playerSeats.Add(hit.collider.gameObject.transform);
            indexDifferenceFromTableCenter = seats.Count - seats.IndexOf(hit.collider.gameObject.transform); // to get no of rotation for player coming in center
            Debug.Log("rotation times :" + indexDifferenceFromTableCenter);
            Player.instance._name.text = "you";
            Player.instance.money.text = gameMoney.ToString();
        }
    }

    void DefaultPlayerManager()
    {
        RandomIndex = Random.Range(0, seats.Count);
        playerPosition = seats[RandomIndex]; // select any random seat on the table
        playerSeats.Add(playerPosition); // add the player transform to a list to later compare it with require player
        GameObject defaultObject = Instantiate(playerPrefab, playerPosition.position, Quaternion.identity);
        Player.instance._name.text = "David";
        Player.instance.money.text = gameMoney.ToString();
    }

    // we rearrange the seats so our player will always be in the center of the table
    void Rearrangement()
    {
        int n = indexDifferenceFromTableCenter;
        for (int i = 0; i < n; i++)
        {
            int j; 
            Transform last;
            //Stores the last element of array  
            last = seats[seats.Count - 1];

            for (j = seats.Count - 1; j > 0; j--)
            {
                //Shift element of array by one  
                seats[j] = seats[j - 1];
            }
            //Last element of array will be added to the start of array.  
            seats[0] = last;
        }
        
        // to rearrange position of all the plyers active in the scene
        for (int i = 0; i < allPlayersInScene.Count; i++)
        {
            Transform initialTransform = playerSeats[i];
            Transform updateTransform = GameObject.Find("Seat_Pos (" + seats.IndexOf(initialTransform) + ")").transform;
            allPlayersInScene[i].transform.position = updateTransform.position;
        }
        /*GameObject maaKiChoot = GameObject.Find("Seat_Pos (" + seats.IndexOf(playerPosition) + ")");
        defaultObject.transform.position = maaKiChoot.transform.position ;*/
    }
    // method to distribute single card at a time
    void distributeCards(Transform cardPos)
    {
        cardOutOfDeck = Instantiate(cardPrefab, deckPos.position, Quaternion.identity);
        cardPrefab.GetComponentInChildren<TextMeshPro>().text = card.Instance.cardAsked();
        temp  = cardPos;
        cardDistributed = true;
    }
    // transfer card to alayer
    void moveCardToPlayer(Transform whereToMove)
    {
        cardOutOfDeck.transform.position = Vector2.MoveTowards(cardOutOfDeck.transform.position, whereToMove.position, 0.1f);
    }
    // distribute cards to  each active player
    void distributeCardsToPlayerInScene()
    {
        for(int i = 0; i < allPlayersInScene.Count; i++)
        {
            for(int cardToBeDistributed = 0; cardToBeDistributed < 2; cardToBeDistributed++)
            {
                distributeCards(allPlayersInScene[i].transform);

               /* while(Mathf.Abs(Mathf.Pow(allPlayersInScene[i].transform.position.x - cardPrefab.transform.position.x, 2) + Mathf.Pow(allPlayersInScene[i].transform.position.y - cardPrefab.transform.position.y, 2)) > 0.1f)
                {
                    moveCardToPlayer(temp);
                }*/
            }
        }
    }
}

