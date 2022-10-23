using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Vector2 _position;

    public void Init(Vector2 pos) { _position = pos; _renderer.color = ((pos.x + pos.y) % 2 == 0) ? _baseColor : _offsetColor; }

    private void OnMouseEnter() { _highlight.SetActive(true); }

    private void OnMouseExit() { _highlight.SetActive(false); }

    private void OnMouseDown() { GameManager.instance.HandleClickOnTile(InputManager.instance.mouseWorldPosition); }

    public void Highlight(Color color)
    {
        color.a = 0.5f;
        _renderer.color = _baseColor;
        _highlight.GetComponent<SpriteRenderer>().material.color = color;
        _highlight.SetActive(true);
    }

    public void DeactivateHightlight()
    {
        _renderer.color = ((_position.x + _position.y) % 2 == 0) ? _baseColor : _offsetColor;
        _highlight.SetActive(false);
        _highlight.GetComponent<SpriteRenderer>().material.color = Color.white;
    }

    public bool IsOccupied()
    {
        return GridManager.instance.IsOccupied(_position);
    }
}