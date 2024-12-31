using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadialMenu : MonoBehaviour
{
    public int numberOfButtons = 8; // Number of buttons in the circle
    public float radius = 200f; // Radius of the circle
    public GameObject buttonPrefab; // Prefab for circular buttons

    void Start()
    {
        CreateRadialMenu();
    }

    void CreateRadialMenu()
    {
        for (int i = 0; i < numberOfButtons; i++)
        {
            // Calculate angle for this button
            float angle = i * Mathf.PI * 2f / numberOfButtons;

            // Position the button in a circle
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            Vector3 buttonPosition = new Vector3(x, y, 0);

            // Instantiate the button
            GameObject newButton = Instantiate(buttonPrefab, transform);
            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition = buttonPosition;

            // Optional: Rotate button to face outward
            buttonTransform.localRotation = Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg);

            // Optional: Customize button label or appearance
            Text buttonText = newButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = $"Button {i + 1}";
            }
        }
    }
}
