using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private StoryLineManagment storyLine;
    private HealthBarManager health; // Reference to the HealthBarManager script

    private void Start() {
        health = FindAnyObjectByType<HealthBarManager>(); // Find and assign the HealthBarManager script
        storyLine = FindAnyObjectByType<StoryLineManagment>();
    }

    private static int previousValue = -1;  // Initialize previousValue to a value that is not a valid index
    public void InstantiateCard() {
        int randomValue;

        do {
            randomValue = Mathf.RoundToInt(Random.Range(0, cards.Length)); // Generate a random index for the cards array
        } while (randomValue == previousValue);

        previousValue = randomValue; // Store the current randomValue for the next iteration


        // The currentValue will not be the same as the previousValue in the next iteration



        newCard = Instantiate(cards[randomValue], transform, false); // Instantiate a new card GameObject using the randomly selected prefab
        newCard.transform.SetAsFirstSibling(); // Set the new card as the first child in the hierarchy
        InstantiateText(); // Instantiate the text for the new card

        Debug.Log("New Card Name: " + newCard.name); // Debug print statement
    }


    private void InstantiateDeathCard() {
        int bar;
        if (health.deathCard.name == "HealthBarFood") {
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

    public int deadCardCount = 0;
    void Update() {
        // Check if there is only one child in the hierarchy, the story hasn't ended, and the player is not dead
        if (transform.childCount < 2 && !health.isDead && storyLine.story.text != "The End")
            InstantiateCard(); // Instantiate a new card 
        else if(health.isDead && deadCardCount < 1) {
            deadCardCount++;
            InstantiateDeathCard();
        }
        if (transform.childCount == 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
