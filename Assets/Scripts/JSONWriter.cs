using System.IO;
using UnityEngine;

public class JSONWriter : MonoBehaviour {
    private void Awake() {
        // Create an example JSON object
        CardData[] cards =
        {
            new CardData
            {
                name = "Help a group of survivors who are trapped by a horde of infected.",
                tag = "Hero",
                leftSwipeEffects = new EffectData[]
                {
                    new EffectData { effect = "Shield+", action = "Help!" },
                    new EffectData { effect = "Shield-", action = "Help!" },
                    new EffectData { effect = "Medic-", action = "Help!" }
                },
                rightSwipeEffects = new EffectData[]
                {
                    new EffectData { effect = "Power-", action = "Ignore!" }
                }
            },
            new CardData
            {
                name = "Explore a nearby abandoned hospital in search of medical supplies.",
                tag = "Explor",
                leftSwipeEffects = new EffectData[]
                {
                    new EffectData { effect = "Medic+", action = "Enter!" },
                    new EffectData { effect = "Shield-", action = "Enter!" },
                    new EffectData { effect = "Food-", action = "Enter!" }
                },
                rightSwipeEffects = new EffectData[]
                {
                    new EffectData { effect = "null", action = "Leave" }
                }
            }
            // Add more cards here...
        };

        // Create a wrapper object to hold the card data
        CardWrapper cardWrapper = new CardWrapper { cards = cards };

        // Convert the card data to a JSON string
        string jsonString = JsonUtility.ToJson(cardWrapper, true);

        // Define the file path
        string fileName = "cards.json";
        string filePath = Path.Combine(Application.dataPath, fileName);

        // Write the JSON data to the file
        File.WriteAllText(filePath, jsonString);

        Debug.Log("JSON file created.");
    }

    [System.Serializable]
    public struct CardData {
        public string name;
        public string tag;
        public EffectData[] leftSwipeEffects;
        public EffectData[] rightSwipeEffects;
    }

    [System.Serializable]
    public struct EffectData {
        public string effect;
        public string action;
    }

    [System.Serializable]
    public class CardWrapper {
        public CardData[] cards;
    }
}
