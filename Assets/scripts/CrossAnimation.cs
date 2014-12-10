using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrossAnimation : MonoBehaviour {

	public AudioClip Slice;

	private AudioSource Source;
	private Image _A, _B;
	// Use this for initialization
	void Start () {
		Source = GetComponent<AudioSource>();
		_A = transform.GetChild(0).GetComponent<Image>();
		_B = transform.GetChild(1).GetComponent<Image>();

		DOTween.Sequence()
			.Append(
			DOTween.To(() => _A.fillAmount, value => _A.fillAmount = value, 1.0f, .3f).OnStart(() => Source.PlayOneShot(Slice)))
			.Append(
			DOTween.To(() => _B.fillAmount, value => _B.fillAmount = value, 1.0f, .3f).OnStart(() => Source.PlayOneShot(Slice)))
			.Append(
			DOTween.ToAlpha(()=>_A.color, value => { _A.color = value; _B.color = value; }, 0f,2f))
			.OnComplete(()=>Destroy(gameObject, 0.5f));
	}

}
