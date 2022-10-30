using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUnitsMouseOverlay : MonoBehaviour
{
    PlaceUnitsUIController placeUnitsUIController;
    SpriteRenderer spriteRenderer;


    private void Start()
    {
        placeUnitsUIController = gameObject.GetComponentInParent<PlaceUnitsUIController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        //set pos to mouse pos
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);

        //set color to red if cant place unit, green if can place unit
        if (placeUnitsUIController.CanPlaceUnitAtPoint(transform.position))
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }
}
