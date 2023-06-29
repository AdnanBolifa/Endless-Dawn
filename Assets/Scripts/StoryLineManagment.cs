using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;


public class StoryLineManagment : MonoBehaviour
{
    public Text story; // Reference to the UI Text component for displaying the story line

    private CardBuilder cardBuilder; // Reference to the CardBuilder script

    public int count = 0; // Counter for tracking the current card

    private void Awake() {
        cardBuilder = FindAnyObjectByType<CardBuilder>(); // Find and assign the CardBuilder script to the cardBuilder variable
    }

    public void ChangeStoryLine() {
        // Increment the count to move to the next card
        count++;

        if (count < cardBuilder.allCard.Count) {
            // If there are more cards to process, display the name of the next card in the UI Text
            story.text = cardBuilder.allCard[count].name;
        } else {
            // If there are no more cards, indicate the end of the story
            story.text = "The End";
        }
    }

}



