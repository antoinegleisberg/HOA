using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class GenerateBuildMenu
{
    static int minimalSpaceBetweenBuildings = 100;

    [MenuItem("My scripts/Generate build menu")]
    static void CreateBuildMenu()
    {
        RectTransform parent = GameObject.Find("BuildMenu").GetComponent<RectTransform>();
        Vector2 parentDimensions = new Vector2(parent.rect.width, parent.rect.height);
        float parentScale = parent.lossyScale.x;
        GameObject BuildMenuItemPrefab = Resources.Load<GameObject>("UI/BuildMenuItem");
        RectTransform menuItemRT = BuildMenuItemPrefab.GetComponent<RectTransform>();
        Vector2 menuItemDimensions = new Vector2(menuItemRT.rect.width, menuItemRT.rect.height);
        int itemsPerLine = Mathf.FloorToInt((parentDimensions.x - minimalSpaceBetweenBuildings) / menuItemDimensions.x);
        float step = parentDimensions.x / itemsPerLine;
        List<Sprite> BuildMenuSprites = Resources.LoadAll<Sprite>("BuildingSprites").ToList();
        for (int i= 0; i < BuildMenuSprites.Count; i++)
        {
            Sprite sprite = BuildMenuSprites[i];
            float x = (-parentDimensions.x / 2 + step / 2 +(i%itemsPerLine) * step) * parentScale;
            float y = parentDimensions.y / 2 - step / 2 - (i / itemsPerLine) * step;
            x = 0;
            y = 0;
            string imageName = char.ToUpper(sprite.name[0]) + sprite.name.Substring(1);
            string menuItemName = imageName;
            if (parent.Find(menuItemName) == null)
            {
                GameObject MenuItem = GameObject.Instantiate(BuildMenuItemPrefab, new Vector3(x, y, 0), Quaternion.identity, parent);
                MenuItem.name = menuItemName;
                // Image menuItemImage = MenuItem.GetComponentInChildren<Image>();
                // menuItemImage.name = imageName;
                // menuItemImage.GetComponent<Image>().sprite = sprite;
            }
        }
    }
}
