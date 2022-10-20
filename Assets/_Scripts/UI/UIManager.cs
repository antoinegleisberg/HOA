using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] private Canvas _menusCanvas;
    [SerializeField] private Dictionary<string, GameObject> _UIMenus;
    private GameObject _activeMenu;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _activeMenu = null;
        InitalizeMenus();
    }

    private void InitalizeMenus()
    {
        _UIMenus = new Dictionary<string, GameObject>();
        List<GameObject> UIMenusList = GetChildrenOf(_menusCanvas.gameObject);
        for (int i = 0; i < UIMenusList.Count; i++)
        {
            _UIMenus[UIMenusList[i].name] = UIMenusList[i];
            UIMenusList[i].SetActive(false);
        }
    }

    public void ToggleMenu(string name)
    {
        GameObject menu = _UIMenus[name];
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            GameEvents.instance.CloseUI();
            if (_activeMenu == menu) _activeMenu = null;
        }
        else
        {
            menu.SetActive(true);
            GameEvents.instance.OpenUI();
            _activeMenu = menu;
        }
    }

    public List<GameObject> GetChildrenOf(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.GetComponentInChildren<Transform>())
        {
            children.Add(child.gameObject);
        }
        return children;
    }
}
