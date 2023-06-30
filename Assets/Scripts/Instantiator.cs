using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Instantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab; // Prefab for the card GameObject
    [SerializeField]
    private Text textPrefab; // Prefab for the text GameObject
    [SerializeField]
    private GameObject[] cards; // Array of available card GameObjects
    [SerializeField]
    private GameObject[] deathCards; // Array of available Death cards GameObjects

    private GameObject newCard; // Reference to the newly instantiated card GameObject

    private StoryLineManagment storyManagement; // Reference to the StoryLineManagment script
    private HealthBarManager health; // Reference to the HealthBarManager script

    private void Start() {
        storyManagement = FindAnyObjectByType<StoryLineManagment>(); // Find and assign the StoryLineManagment script
        health = FindAnyObjectByType<HealthBarManager>(); // Find and assign the HealthBarManager script
    }

    public void InstantiateCard() {
        int randomValue;
        do {
            randomValue = Mathf.RoundToInt(Random.Range(0, cards.Length)); // Generate a random index for the cards array
        } while (newCard != null && cards[randomValue] == newCard.gameObject);

        newCard = Instantiate(cards[randomValue], transform, false); // Instantiate a new card GameObject using the randomly selected prefab
        newCard.transform.SetAsFirstSibling(); // Set the new card as the first child in the hierarchy
        InstantiateText(); // Instantiate the text for the new card
    }

    private void InstantiateDeathCard() {
        int bar;
        if (health.deathCard.name == "HealthBarFoodd") {
            bar = 0;
        }
        else 
            bar = 1;
        newCard = Instantiate(deathCards[bar], transform, false); // Instantiate a new card GameObject using the randomly selected prefab
        newCard.transform.SetAsFirstSibling(); // Set the new card as the first child in the hierarchy
    }
    public void InstantiateText() {
        Text newText = Instantiate(textPrefab, transform, false); // Instantiate a new text GameObject using the textPrefab
        newText.transform.SetParent(newCard.transform); // Set the new text as a child of the new card
    }

    private int deadCardCount = 0;
    void Update() {
        // Check if there is only one child in the hierarchy, the story hasn't ended, and the player is not dead
        if (transform.childCount < 2 && !health.isDead)
            InstantiateCard(); // Instantiate a new card 
        else if(health.isDead && deadCardCount < 1) {
            deadCardCount++;
            InstantiateDeathCard();
        }
    }

}
