using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class card : MonoBehaviour
{
    public static card Instance;
    List<string> cards;
    string[] cardClass = new string[]
    { 
        "spades",
        "hearts",
        "diamonds",
        "clubs"
    };

    string[] cardName = new string[]
    {
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
        "ten",
        "jack",
        "queen",
        "king",
        "ace"
    };
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        cards = new List<string>();

        // for generating a deck of card
        for(int cardType = 0; cardType < 4; cardType++)
        {
            string _cardClass = cardClass[cardType];

            for(int cardValue = 0; cardValue < 13; cardValue++)
            {
                string _cardName = cardName[cardValue];
                string fullCard = _cardClass + " " + _cardName;
                cards.Add(fullCard);
            }
        }
    }

    [ContextMenu("Get a Card")]
    public string cardAsked()
    {
        int randomIndex = Random.Range(0, cards.Count);
        string cardGot = cards[randomIndex];
        cards.RemoveAt(randomIndex);
        Debug.Log("arrived card : " + cardGot);

        return cardGot;
    }
}
