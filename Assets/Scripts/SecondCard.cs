using UnityEngine;
using UnityEngine.UI;

public class SecondCard : MonoBehaviour
{
    private SwipeEffect swipeEffect;
    private GameObject firstCard;
    private Image image;
    

    // Start is called before the first frame update
    void Start()
    {
        swipeEffect = FindAnyObjectByType<SwipeEffect>();
        image = GetComponent<Image>();
        image.color = Color.black;

        firstCard = swipeEffect.gameObject;
        swipeEffect.cardMoved += CardMovedFront;
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); 
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = firstCard.transform.localPosition.x;
        if (Mathf.Abs(distanceMoved) > 0)
        {
            float step = Mathf.SmoothStep(0.8f, 1, Mathf.Abs(distanceMoved) / (Screen.width / 2));
            transform.localScale = new Vector3 (step, step, step);
        }
        
    }
    void CardMovedFront()
    {
        gameObject.AddComponent<SwipeEffect>();
        Destroy(this);
    }
}
