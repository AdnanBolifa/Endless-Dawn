using UnityEngine;
using UnityEngine.UI;

public class SecondCard : MonoBehaviour
{
    private SwipeEffect swipeEffect; // Reference to the SwipeEffect script
    private GameObject firstCard; // Reference to the first card GameObject
    private Image image; // Reference to the Image component of this GameObject

    // Start is called before the first frame update
    void Start() {
        
        swipeEffect = FindAnyObjectByType<SwipeEffect>(); // Find and assign the SwipeEffect script
        image = GetComponent<Image>(); // Get the Image component attached to this GameObject

        image.color = Color.black; // Set the color of the Image to black

        firstCard = swipeEffect.gameObject; // Assign the first card GameObject to firstCard
        swipeEffect.cardMoved += CardMovedFront; // Subscribe to the cardMoved event of the SwipeEffect
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Set the local scale of this GameObject to (0.8, 0.8, 0.8)
    }

    // Update is called once per frame
    void Update() {
        float distanceMoved = firstCard.transform.localPosition.x; // Calculate the distance moved by the first card
        if (Mathf.Abs(distanceMoved) > 0) {
            // Calculate the scaling factor based on the distance moved
            float step = Mathf.SmoothStep(0.8f, 1, Mathf.Abs(distanceMoved) / (Screen.width / 2));
            transform.localScale = new Vector3(step, step, step); // Set the local scale of this GameObject
        }
    }

    void CardMovedFront() {
        gameObject.AddComponent<SwipeEffect>(); // Add a new SwipeEffect component to this GameObject
        Destroy(this); // Destroy the current SwipeEffect component
    }

}
