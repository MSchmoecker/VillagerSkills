using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CustomButton : MonoBehaviour {
	public Image Image;

	public bool ButtonEnabled = true;

	private RectTransform rectTransform;

	private bool isDown;

	public Func<bool> IsSelectableAction;

	private RectTransform parentScreen;

	public bool SetColor = true;

	public AudioClip CustomSound;

	private Color startColor;

	public string TooltipText;

	private ScrollRect parentScrollRect;

	private TextMeshProUGUI tmPro;

	public PointerEventData LastEventData;

	private Color targetColor = Color.white;

	public bool ScrollToInRect = true;

	private bool tryFindParentScrollRect = true;

	[HideInInspector]
	public event Action Clicked;

	[HideInInspector]
	public event Action<Vector2> StartDragging;
}
