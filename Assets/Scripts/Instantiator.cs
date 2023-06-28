using UnityEngine;
using UnityEngine.UI;

public class Instantiator : MonoBehaviour
{
    public GameObject cardPrefab;
    public Text textPrefab;      
    public GameObject[] cards;
    private GameObject newCard;

    private StoryLineManagment story;
    private HealthBarManager health;

    private void Start()
    {
        story = FindAnyObjectByType<StoryLineManagment>();
        health = FindAnyObjectByType<HealthBarManager>();
    }

    public void InstantiateCard()
    {
        int randomValue = Mathf.RoundToInt(Random.Range(0, cards.Length));
        newCard = Instantiate(cards[randomValue], transform, false);
        newCard.transform.SetAsFirstSibling();
        InstantiateText();
    }
    public void InstantiateText()
    {
        Text newText = Instantiate(textPrefab, transform, false);
        newText.transform.SetParent(newCard.transform);
    }
    void Update()
    {
        if(transform.childCount < 2 &&  story.text.text != "The End" && !health.isDead)
            InstantiateCard();
        
    }
}
