using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class OutlineFlicker : MonoBehaviour {

	
	public float Min = 3f; 
	public float Max = 4f;

	private float _value = 3f;
	private Outline _outline ;
	private bool _swap;
	// Use this for initialization
	void Start () {
		_outline = GetComponent<Outline>();
		Flicker();
	}
	
	// Update is called once per frame
	void Update () {
		_outline.effectDistance = new Vector2(-_value, _value);
	}

	void Flicker() {
		_swap = !_swap;
		DOTween.To(() => _value, value => _value = value, _swap ? Min : Max, 1f).OnComplete(Flicker);
	}
}
