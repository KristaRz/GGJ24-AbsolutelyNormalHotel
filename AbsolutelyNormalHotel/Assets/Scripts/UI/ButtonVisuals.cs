/// Created 12.20.2022 by Krista Plagemann ///
/// Switches sprites on button click from selected to deselected.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Tooltip("How the button will look like when selected.")][SerializeField] private bool startingState = false;

    [Tooltip("How the button will look like when selected.")] public Material defaultSprite;
    [Tooltip("How the button will look like when selected.")] public Material selectedSprite;
    [Tooltip("How the button will look like while hovering.")] public Material hoverSprite;
    [Tooltip("How the button will look like when pressing.")] public Material pressedSprite;
    [Tooltip("How the button will look like when deselected.")] public Material deselectedSprite;
    [Tooltip("How the button will look like when disabled.")] public Material notInteractableSprite;
    [Tooltip("If empty, this object's image will be used.")][SerializeField] private MeshRenderer targetRenderer;
    [Tooltip("Can you deselect this item?")][SerializeField] private bool allowDeselect = true;
    [Tooltip("Keep this item selected when the user unhovers?")][SerializeField] private bool selectionStays = false;

    [Tooltip("All the buttons belonging together to deselect others on select.")][SerializeField] private ButtonVisuals[] buttonGroup;

    private Button _button;
    private Renderer _renderer;

    private bool selected = false;

    private void OnEnable()
    {
        if (_button.interactable) _renderer.material = defaultSprite;
        selected = startingState;
        if (selected && selectedSprite != null)
            _renderer.material = selectedSprite;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (targetRenderer == null) _renderer = GetComponent<MeshRenderer>();
        else _renderer = targetRenderer;

        defaultSprite = _renderer.material;
    }

    private bool _buttonIsInteractable;
    private Color _backgroundColor;
    private void Update()
    {
        if (!_button.interactable && !_buttonIsInteractable)
        {
            _buttonIsInteractable = true;
            _renderer.material = notInteractableSprite;
        }

        if (_button.interactable && _buttonIsInteractable)
        {
            _buttonIsInteractable = false;
            _renderer.material = deselectedSprite;
        }
    }

    public Action<bool> buttonStateChanged;
    // Switches sprites on select and also calls a deselct on other buttons. //
    public void SelectSpriteSwap()
    {
        if (!_button.interactable)
            return;
        if (selected && !allowDeselect)
            return;
        selected = !selected;
        switch (selected)
        {
            case true:
                if (selectedSprite != null)
                    _renderer.material = selectedSprite;
                foreach (var item in buttonGroup)
                    item.DeselectSprite();
                break;
            case false:
                if (deselectedSprite != null)
                    _renderer.material = deselectedSprite;
                break;
        }

        buttonStateChanged?.Invoke(selected);
    }

    // Exectues on hover and changes sprites. Stores current sprite to switch back if we don't select. //
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable)
            return;

        if (!selected)
        {
            if (hoverSprite != null)
                _renderer.material = hoverSprite;
        }
    }
    // Executes on press. //
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable)
            return;

        if (pressedSprite != null)
            _renderer.material = pressedSprite;
    }
    // Exectues on end hover. //
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.interactable)
            return;

        if (selectionStays && selected)
            return;

        if (defaultSprite != null)
            _renderer.material = defaultSprite;
    }

    // Deselcts this sprite if another in the group gets selected. //
    public void DeselectSprite()
    {
        if (!_button.interactable)
            return;

        if (deselectedSprite != null)
            _renderer.material = deselectedSprite;
        selected = false;
    }
}
