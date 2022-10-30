using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaceUnitsUIController : MonoBehaviour
{
    [SerializeField] GameObject coreAlienPF;
    [SerializeField] int numberCoreAliens;
    GameObject currentlySelectedUnitPF;
    UIDocument uiDoc;
    bool uiOpen;
    List<GameObject> placedUnits = new List<GameObject>();


    private void Start()
    {
        uiDoc = GetComponent<UIDocument>();
        uiDoc.rootVisualElement.Q<Button>("start-Button").clicked += StartGame;
        ShowUI();
        StopGame();
    }


    private void Update()
    {
        if (uiOpen)
        {
            if (Input.GetMouseButtonDown(0) && !MouseOverButton())
            {
                PlaceUnitWhereClicked();
            }
        }
    }


    void PlaceUnitWhereClicked()
    {
        if(numberCoreAliens <= 0)
        {
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //create unit and disable pathfinding so they dont do anything
        GameObject createdUnit = Instantiate(coreAlienPF, mousePos, Quaternion.identity);
        createdUnit.GetComponent<MoveToNearestTarget>().enabled = false;

        numberCoreAliens--;

        UpdateUI();
    }



    bool MouseOverButton()
    {
        Vector2 mousePos = Input.mousePosition;

        foreach (VisualElement visualElement in uiDoc.rootVisualElement.Children())
        {
            if (visualElement.worldBound.Contains(mousePos))
            {
                return true;
            }
        }

        return false;
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        HideUI();
    }

    public void ShowUI()
    {
        uiDoc.rootVisualElement.visible = true;
        uiOpen = true;

        UpdateUI();

        placedUnits.Clear();
    }

    public void HideUI()
    {
        uiDoc.rootVisualElement.visible = false;
        uiOpen = false;
        uiDoc.rootVisualElement.Q<VisualElement>("unitButtons").Clear();
    }

    public void UpdateUI()
    {
        VisualElement buttonContainer = uiDoc.rootVisualElement.Q<VisualElement>("unitButtons");

        buttonContainer.Clear();

        Button coreAlienButton = new Button();
        coreAlienButton.AddToClassList("UnitButton");
        coreAlienButton.text = $"Alien, {numberCoreAliens} remaining";

        buttonContainer.Add(coreAlienButton);
    }
}
