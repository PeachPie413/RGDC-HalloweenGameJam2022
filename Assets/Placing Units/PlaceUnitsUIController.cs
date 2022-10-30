using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaceUnitsUIController : MonoBehaviour
{
    [SerializeField] GameObject coreAlienPF;
    [SerializeField] int numberCoreAliens;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] LayerMask wallLayers;
    [SerializeField] float minDistFromEnemies, minDistFromWall;
    UIDocument uiDoc;
    bool uiOpen;
    List<GameObject> placedUnits = new List<GameObject>();
    Label unitsLeftLabel;


    private void Start()
    {
        uiDoc = GetComponent<UIDocument>();
        uiDoc.rootVisualElement.Q<Button>("start-Button").clicked += StartGame;
        unitsLeftLabel = uiDoc.rootVisualElement.Q<Label>("number-units-left");

        ShowUI();
        StopGame();
    }


    private void Update()
    {
        if (uiOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryPlaceUnitWhereClicked();
            }
        }
    }


    void TryPlaceUnitWhereClicked()
    {
        //if player has units left
        if(numberCoreAliens <= 0)
        {
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //check if spot is a viable place to put unit
        if (!CanPlaceUnitAtPoint(mousePos)) return;

        //create unit and disable pathfinding so they dont do anything
        GameObject createdUnit = Instantiate(coreAlienPF, mousePos, Quaternion.identity);
        createdUnit.GetComponent<MoveToNearestTarget>().enabled = false;

        numberCoreAliens--;

        UpdateUI();
    }


    public bool CanPlaceUnitAtPoint(Vector2 point)
    {
        return (!MouseOverStartButton() && !EnemiesNearby(point) && !WallNearby(point));
    }


    bool MouseOverStartButton()
    {
        Vector2 mousePos = Input.mousePosition;

        return uiDoc.rootVisualElement.Q<VisualElement>("start-Button").worldBound.Contains(mousePos);
    }


    public bool EnemiesNearby(Vector2 placePos)
    {
        return Physics2D.CircleCast(placePos, minDistFromEnemies, Vector2.up, 0, enemyLayers);
    }

    public bool WallNearby(Vector2 placePos)
    {
        return Physics2D.CircleCast(placePos, minDistFromWall, Vector2.up, 0, wallLayers);
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
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideUI()
    {
        uiDoc.rootVisualElement.visible = false;
        uiOpen = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        unitsLeftLabel.text = $"{numberCoreAliens} units left!";
    }
}
