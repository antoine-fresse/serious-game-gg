using System;
using DG.Tweening.Core;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FloatingText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.SetParent(GameObject.Find("Canvas").transform);
		//transform.localScale = Vector3.one;

		transform.DOLocalMove(transform.localPosition + new Vector3(0, 50, 0), 1.3f);
		transform.DOShakeRotation(0.7f, new Vector3(0f,0f,90f));
		transform.DOScale(new Vector3(2f, 2f, 2f), 3f).OnComplete(() => Destroy(gameObject));
	}

	
}
