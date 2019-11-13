﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardSprite
{
    public Sprite unlocked;
    public Sprite locked;
}
public class PuzzleCreator : MonoBehaviour
{
    // Card objects    
    [SerializeField] GameObject cardPrefab;
    List<GameObject> cards = new List<GameObject>();
    // Puzzle parameters    
    [SerializeField] [Range(5, 35)] int pairs;
    [SerializeField] int columns;
    [SerializeField] Vector2 spacing;
    // Assets
    [HideInInspector] public List<CardSprite> sprites;
    Sprite[] unlocked;
    Sprite[] locked;
    
    void Start()
    {
        RetrieveAssets();
        CreateCards();
        PlaceCards();
    }
    private void RetrieveAssets()
    {
        unlocked = Resources.LoadAll<Sprite>("Sprites/Cards");
        locked = Resources.LoadAll<Sprite>("Sprites/Locked");
        // Get sprites depending on desired number of pairs, max 35
        for (int i = 0; i < pairs; i++)
        {
            var sprite = new CardSprite();
            sprite.unlocked = unlocked[i];
            sprite.locked = locked[i];
            sprites.Add(sprite);
        }
    }
    private void CreateCards()
    {
        for(int i = 0; i < pairs; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject newCard = Instantiate(cardPrefab, transform);
                CardController controller = newCard.GetComponent<CardController>();
                controller.card.type = i;
                controller.visible = sprites[i].unlocked;
                controller.disabled = sprites[i].locked;                
                cards.Add(newCard);
            }
        }
        // Random order
        ListExtensions.Shuffle(cards);
    }
    private void PlaceCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 position = new Vector3(i % columns, i / columns);
            position *= spacing;
            cards[i].transform.localPosition = position;
        }
        Vector3 offset = new Vector3((columns - 1) / 2.0f, (cards.Count - 1) / columns / 2.0f);
        offset *= spacing;
        transform.position -= offset;
    }
}