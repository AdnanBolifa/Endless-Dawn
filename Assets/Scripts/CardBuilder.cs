using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct CardEffect {
    public string effect;
}

public struct Card {
    public string name;
    public string tag;
    public string leftAction;
    public string rightAction;
    public int id;
    public List<CardEffect> leftSwipeEffects;
    public List<CardEffect> rightSwipeEffects;

    public Card(string name, string tag, int id, string leftAction, string rightAction) {
        this.name = name;
        this.tag = tag;
        this.id = id;
        this.leftAction = leftAction;
        this.rightAction = rightAction;
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

        // Add some cards to the excludedCards list (optional)
        excludedCards.Add(allCard[0]);
        excludedCards.Add(allCard[2]);
        excludedCards.Add(allCard[4]);
        //excludedCards.Add(allCard[14]);

        ShuffleList(allCard, excludedCards);

        // Display the name and tag of each card in allCard list
        foreach (Card card in allCard) {
            Debug.Log(card.id +": "+card.name + " - " + card.tag);
        }

        // Optional: Display the first card's name in the storyline text
        StoryLineManagment storyLine = FindAnyObjectByType<StoryLineManagment>();
        storyLine.story.text = allCard[0].name;
    }

    private void LoadCardData() {
        // Read the card data from the JSON string
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
            Card newCard = new Card(cardData.name, cardData.tag, cardData.id, cardData.leftAction, cardData.rightAction);
            newCard.leftSwipeEffects = ConvertCardEffects(cardData.leftSwipeEffects);
            newCard.rightSwipeEffects = ConvertCardEffects(cardData.rightSwipeEffects);
            allCard.Add(newCard);

            // Optional: Display the added card's details
            //Debug.Log("Added new card: " + newCard.name);
        }
        Debug.Log(cardDataList.Count + ": Cards has been added.");
    }

    private List<CardEffect> ConvertCardEffects(CardEffectData[] effectDataArray) {
        List<CardEffect> effects = new List<CardEffect>();

        // Convert each CardEffectData to CardEffect
        foreach (CardEffectData effectData in effectDataArray) {
            CardEffect effect = new CardEffect();
            effect.effect = effectData.effect;
            effects.Add(effect);
        }

        return effects;
    }

    private void ShuffleList(List<Card> list, List<Card> excludedCards = null) {
        System.Random random = new System.Random();
        int n = list.Count;
        int groupSize = 10;

        // Shuffle each consecutive group of 10 elements separately
        for (int groupIndex = 0; groupIndex < n / groupSize; groupIndex++) {
            int startIndex = groupIndex * groupSize;
            int endIndex = Mathf.Min(startIndex + groupSize, n);

            List<Card> cardsToShuffle = new List<Card>();

            // Collect the cards in the group that are not excluded
            for (int i = startIndex; i < endIndex; i++) {
                if (excludedCards == null || !excludedCards.Contains(list[i])) {
                    cardsToShuffle.Add(list[i]);
                }
            }

            // Shuffle the collected cards
            int cardsToShuffleCount = cardsToShuffle.Count;
            for (int i = 0; i < cardsToShuffleCount; i++) {
                int j = random.Next(i, cardsToShuffleCount);
                Card temp = cardsToShuffle[i];
                cardsToShuffle[i] = cardsToShuffle[j];
                cardsToShuffle[j] = temp;
            }

            // Assign the shuffled cards back to the original list
            for (int i = startIndex, shuffledIndex = 0; i < endIndex; i++) {
                if (excludedCards == null || !excludedCards.Contains(list[i])) {
                    list[i] = cardsToShuffle[shuffledIndex];
                    shuffledIndex++;
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
        public int id;
        public string leftAction;
        public string rightAction;
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
