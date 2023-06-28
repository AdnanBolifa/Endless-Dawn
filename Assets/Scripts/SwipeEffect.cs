using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SwipeEffect : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public int rotationDegree = 15;
    public float dragLimit = 0.2f;
    public float faddingValue = 0.7f;
    private float distanceMoved;
    private bool swipeLeft;

    public Text textTest;

    public event Action cardMoved;
    private Vector3 initialPosition;
    private Image fadding;
    private CardBuilder cardBuilder;
    private StoryLineManagment scenatrio;
    private HealthBarManager healthBarManager;

    public void Start()
    {
        healthBarManager = FindAnyObjectByType<HealthBarManager>();
        cardBuilder = FindAnyObjectByType<CardBuilder>();
        fadding = GetComponent<Image>();
        fadding.color = Color.white;

        textTest = GetComponentInChildren<Text>();
        scenatrio = FindAnyObjectByType<StoryLineManagment>();
    }
    public void OnDrag(PointerEventData eventData) {
        // Move the object based on the drag movement
        // eventData represents the change in the Y/X-coordinate of the drag.
        transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y + eventData.delta.y);

        // Check if the object is dragged to the right of its initial position
        if (transform.localPosition.x > initialPosition.x) {
            // Rotate the object based on the drag distance to the right
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(0, -rotationDegree,
                (initialPosition.x + transform.localPosition.x) / (Screen.width / 2)));
        } else {
            // Rotate the object based on the drag distance to the left
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(0, rotationDegree,
                (initialPosition.x - transform.localPosition.x) / (Screen.width / 2)));
        }

        // Perform the necessary handling for the choices after the drag
        ChoicesHandler();
    }

    private string[] status = new string[10];

    private void ChoicesHandler() {
        int count = 0;
        bool isRightSwipe = transform.localPosition.x > initialPosition.x;
        bool hasMoreCards = scenatrio.count < cardBuilder.allCard.Count;
        Card currentCard = cardBuilder.allCard[scenatrio.count];

        if (hasMoreCards) {
            // Determine the swipe effects based on the swipe direction
            var swipeEffects = isRightSwipe ? currentCard.rightSwipeEffects : currentCard.leftSwipeEffects;

            // Iterate through the swipe effects of the current card
            foreach (var card in swipeEffects) {
                // Set the displayed action text to the action of the current card
                textTest.text = card.action;

                // Store the effect of the current card in the status array and increment the count
                status[count++] = card.effect;
            }
        } else {
            // If there are no more cards to display, indicate the end
            textTest.text = "End..";
        }

        // Set the fading color using the specified faddingValue
        fadding.color = Color.HSVToRGB(0, 0, faddingValue);
    }



    public void OnBeginDrag(PointerEventData eventData) {
        // Store the initial position of the object before dragging
        initialPosition = transform.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        // Calculate the distance moved during the drag
        distanceMoved = Mathf.Abs(transform.localPosition.x - initialPosition.x);

        // Check if the distance moved is below the drag limit
        if (distanceMoved < dragLimit * Screen.width) {
            // Reset the object's position, rotation, and visual properties
            ResetCard();
        } else {
            // Determine the swipe direction based on the movement
            if (transform.localPosition.x > initialPosition.x)
                swipeLeft = false;
            else
                swipeLeft = true;

            // Invoke the cardMoved event and start a coroutine for additional actions
            cardMoved?.Invoke();
            StartCoroutine(MovedCard());
        }
    }
    private void ResetCard() {
        transform.localPosition = initialPosition;
        transform.localEulerAngles = Vector3.zero;
        fadding.color = Color.HSVToRGB(0, 0, 1f);
        textTest.text = "";
    }
    
    
    /**
    * Coroutine for moving the card and fading out its image.
    */
    private IEnumerator MovedCard() {
        float time = 0;

        // Continue the loop until the image color becomes transparent (alpha = 0)
        while (GetComponent<Image>().color != new Color(1, 1, 1, 0)) {
            time += Time.deltaTime;

            if (swipeLeft) {
                // Smoothly move the card to the left
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x - Screen.width, time), transform.localPosition.y, 0);
            } else {
                // Smoothly move the card to the right
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x + Screen.width, time), transform.localPosition.y, 0);
            }

            // Smoothly fade out the image
            GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));

            // Wait for the next frame
            yield return null;
        }

        // Once the image becomes transparent, call the StatusChanger method
        // to handle the status changes and destroy the game object
        StatusChanger(status);
        Destroy(gameObject);
    }


    private int damage = 20;
    private void StatusChanger(string[] status) {

         const string SHIELD_POS = "Shield+";
         const string FOOD_POS = "Food+";
         const string POWER_POS = "Power+";
         const string MEDIC_POS = "Medic+";
         const string SHIELD_NEG = "Shield-";
         const string FOOD_NEG = "Food-";
         const string POWER_NEG = "Power-";
         const string MEDIC_NEG = "Medic-";

        for (int i = 0; i < status.Length; i++) {
            
                switch (status[i]) {
                    case SHIELD_POS:
                        healthBarManager.ModifySlider(HealthBarManager.SliderType.Shield, damage);
                        break;
                    case FOOD_POS:
                        healthBarManager.ModifySlider(HealthBarManager.SliderType.Food, damage);
                        break;
                    case POWER_POS:
                        healthBarManager.ModifySlider(HealthBarManager.SliderType.Power, damage);
                        break;
                    case MEDIC_POS:
                        healthBarManager.ModifySlider(HealthBarManager.SliderType.Medic, damage);
                        break;
                    case SHIELD_NEG:
                        healthBarManager.ModifySlider(HealthBarManager.SliderType.Shield, -damage);
                        break;
                    case FOOD_NEG:
                        healthBarManager.ModifySlider(HealthBarManager.SliderType.Food, -damage);
                        break;
                    case POWER_NEG:
                        healthBarManager.ModifySlider(HealthBarManager.SliderType.Power, -damage);
                        break;
                    case MEDIC_NEG:
                        healthBarManager.ModifySlider(HealthBarManager.SliderType.Medic, -damage);
                        break;
                }
            }
        if (healthBarManager == null)
            return;
        if (healthBarManager.isDead == false)
            scenatrio.ChangeStoryLine();
        else {
            scenatrio.text.text = "DEAD!";
        }
    }
}
