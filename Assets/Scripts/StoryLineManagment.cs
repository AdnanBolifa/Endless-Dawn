using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;


public class StoryLineManagment : MonoBehaviour
{
    public Text text;

    private CardBuilder cardBuilder;

    public int count = 0;
    private void Awake() {
        cardBuilder = FindAnyObjectByType<CardBuilder>();
       
    }
    public void ChangeStoryLine() {
        // Process the cards
        count++;
        if (count < cardBuilder.allCard.Count)
            text.text = cardBuilder.allCard[count].name;
        else
            text.text = "The End";
    }
}



