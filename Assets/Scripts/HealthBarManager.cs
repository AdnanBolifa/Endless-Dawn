using UnityEngine;
using UnityEngine.UI;
using static HealthBarManager;

public class HealthBarManager : MonoBehaviour
{
    public Slider shieldSlider;
    public Slider medicSlider;
    public Slider powerSlider;
    public Slider foodSlider;

    public enum SliderType {
        Shield,
        Medic,
        Power,
        Food
    }

    public void ModifySlider(SliderType sliderType, int damage) {
        switch (sliderType) {
            case SliderType.Shield:
                ApplyDamage(shieldSlider, damage);
                break;
            case SliderType.Medic:
                ApplyDamage(medicSlider, damage);
                break;
            case SliderType.Power:
                ApplyDamage(powerSlider, damage);
                break;
            case SliderType.Food:
                ApplyDamage(foodSlider, damage);
                break;
            default:
                Debug.LogWarning("Unknown slider type: " + sliderType);
                break;
        }
    }
    public bool isDead = false;
    void ApplyDamage(Slider slider, int damage) {
        slider.value += damage;
        if (slider.value <= 2) {
            Debug.LogWarning("lack of " + slider.name);
            isDead = true;
        }
    }
}
