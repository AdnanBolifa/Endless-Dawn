using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct CardEffect {
    public string effect;
    public string action;
}

public struct Card {
    public string name;
    public string tag;
    public List<CardEffect> leftSwipeEffects;
    public List<CardEffect> rightSwipeEffects;

    public Card(string name, string tag) {
        this.name = name;
        this.tag = tag;
        leftSwipeEffects = new List<CardEffect>();
        rightSwipeEffects = new List<CardEffect>();
    }
}

public class CardBuilder : MonoBehaviour {
    public List<Card> allCard;
    string jsonString;
    private void Start() {
        allCard = new List<Card>();

        string fileName = "cards";
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile != null) {
            jsonString = jsonFile.text;
            // Process the JSON data as needed
            Debug.Log("JSON file loaded: " + jsonString);
        } else {
            Debug.LogError("Failed to load JSON file: " + fileName);
        }

        LoadCardData();
        List<Card> excludedCards = new List<Card>();

        excludedCards.Add(allCard[0]);
        excludedCards.Add(allCard[2]);
        excludedCards.Add(allCard[4]);
        excludedCards.Add(allCard[6]);

        ShuffleList(allCard, excludedCards);

        // Pick cards until all have been picked
        foreach (Card card in allCard) {
            Debug.Log(card.name + " - " + card.tag);
        }

        // Optional: Display the first card's name in the storyline text
        StoryLineManagment storyLine = FindAnyObjectByType<StoryLineManagment>();
        storyLine.text.text = allCard[0].name;
    }

    private void LoadCardData() {
        // Read the card data file
        string jsonData = jsonString;
        if (jsonData == null) {
            Debug.Log("Could not find JSON card");
        }

        // Deserialize the JSON data into a wrapper object
        CardDataWrapper cardDataWrapper = JsonUtility.FromJson<CardDataWrapper>(jsonData);

        // Retrieve the list of cards from the wrapper object
        List<CardData> cardDataList = cardDataWrapper.cards;

        // Convert the card data to Card structs and add them to the allCard list
        foreach (CardData cardData in cardDataList) {
            Card newCard = new Card(cardData.name, cardData.tag);
            newCard.leftSwipeEffects = ConvertCardEffects(cardData.leftSwipeEffects);
            newCard.rightSwipeEffects = ConvertCardEffects(cardData.rightSwipeEffects);
            allCard.Add(newCard);

            // Optional: Display the added card's details
            Debug.Log("Added new card: " + newCard.name);
        }
    }

    private List<CardEffect> ConvertCardEffects(CardEffectData[] effectDataArray) {
        List<CardEffect> effects = new List<CardEffect>();

        // Convert each CardEffectData to CardEffect
        foreach (CardEffectData effectData in effectDataArray) {
            CardEffect effect = new CardEffect();
            effect.effect = effectData.effect;
            effect.action = effectData.action;
            effects.Add(effect);
        }

        return effects;
    }

    private void ShuffleList(List<Card> list, List<Card> excludedCards = null) {
        System.Random random = new System.Random();
        int n = list.Count;
        int groupSize = 5;

        // Shuffle each consecutive group of 10 elements separately
        for (int groupIndex = 0; groupIndex < n / groupSize; groupIndex++) {
            int startIndex = groupIndex * groupSize;
            int endIndex = Mathf.Min(startIndex + groupSize, n);

            for (int i = startIndex; i < endIndex - 1; i++) {
                if (excludedCards == null || !excludedCards.Contains(list[i])) {
                    int j = random.Next(i, endIndex);
                    Card temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }
    }





    // Other methods and fields...

    // Data structure for deserializing card data from JSON
    [System.Serializable]
    private class CardData {
        public string name;
        public string tag;
        public CardEffectData[] leftSwipeEffects;
        public CardEffectData[] rightSwipeEffects;
    }

    [System.Serializable]
    private class CardDataWrapper {
        public List<CardData> cards;
    }

    [System.Serializable]
    private class CardEffectData {
        public string effect;
        public string action;
    }
}
