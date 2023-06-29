using UnityEngine;
using UnityEngine.UI;
using static HealthBarManager;

public class HealthBarManager : MonoBehaviour
{
    // Define sliders for different types: shield, medic, power, and food.
    public Slider shieldSlider;
    public Slider medicSlider;
    public Slider powerSlider;
    public Slider foodSlider;

    // Enum to represent the different slider types.
    public enum SliderType {
        Shield,
        Medic,
        Power,
        Food
    }

    // Method to modify a specified slider based on the given slider type and damage amount.
    public void ModifySlider(SliderType sliderType, int damage) {
        // Use a switch statement to determine which slider to modify.
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
                // Log a warning if the slider type is unknown.
                Debug.LogWarning("Unknown slider type: " + sliderType);
                break;
        }
    }

    // Variable to track if the character is dead.
    public bool isDead = false;

    // Method to apply damage to a slider.
    void ApplyDamage(Slider slider, int damage) {
        // Increase the value of the slider by the damage amount.
        slider.value += damage;

        // Check if the slider value is below or equal to 2.
        if (slider.value <= 2) {
            // Log a warning indicating a lack of the specific slider type.
            Debug.LogWarning("lack of " + slider.name);

            // Set the isDead flag to true.
            isDead = true;
        }
    }

}
