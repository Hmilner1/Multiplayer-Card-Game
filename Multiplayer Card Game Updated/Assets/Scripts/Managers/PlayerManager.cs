using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Card;
    private GameObject Hand;

    private void Start()
    {
        Hand = GameObject.Find("Player Hand");
        for (int i = 0; i < 5; i++)
        { 
            Instantiate(Card,Hand.transform);
        }
    }
}
