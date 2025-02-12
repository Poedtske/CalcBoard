using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CalcBoardLayout : MonoBehaviour
{

    private UIDocument uiDocument;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

        uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found! Make sure this script is attached to the same GameObject as UIDocument.");
            return;
        }



        // Get root from UI Document
        VisualElement root = uiDocument.rootVisualElement;

        root.style.justifyContent = Justify.Center;
        root.style.alignItems = Align.Center;

        // Find the container for grid items
        VisualElement gridContainer = new VisualElement();
        gridContainer.AddToClassList("grid-container");

        // Create an ArrayList with 24 entries
        ArrayList items = new ArrayList();
        for (int i = 0; i < 24; i++)
        {
            items.Add(gameObject.AddComponent<ElektroTile>());
        }

        // Loop over the ArrayList and create elements
        foreach (var item in items)
        {
            Button gridItem = new Button();
            gridItem.AddToClassList("grid-item");
            gridItem.text = item.ToString(); // Set button text to identify it
            gridItem.clicked += () => Debug.Log($"Clicked on: {item}"); // Add click event

            gridContainer.Add(gridItem);
        }

        // Add the grid to the UI
        root.Add(gridContainer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
