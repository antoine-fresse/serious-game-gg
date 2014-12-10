using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager Instance { get; private set; }
	private AudioClip[] _cardPlace;
	private AudioClip[] _cardShove;
	private AudioClip[] _cardSlide;
	private AudioClip[] _cardHit;
	private AudioClip[] _cardFlip;
	public AudioSource Source;

	void Awake() {
		Instance = this;

		_cardPlace = Resources.LoadAll<AudioClip>("Place");
		_cardShove = Resources.LoadAll<AudioClip>("Shove");
		_cardSlide = Resources.LoadAll<AudioClip>("Slide");
		_cardHit = Resources.LoadAll<AudioClip>("Hit");
		_cardFlip = Resources.LoadAll<AudioClip>("Flip");
	}

	void Start() {
		if (!Source)
			Source = GetComponent<AudioSource>();
	}

	public void PlayCardPlace() {
		var n = Random.Range(0, _cardPlace.Length - 1);
		Source.PlayOneShot(_cardPlace[n]);
	}
	public void PlayCardShove() {
		var n = Random.Range(0, _cardShove.Length - 1);
		Source.PlayOneShot(_cardShove[n]);
	}
	public void PlayCardSlide() {
		var n = Random.Range(0, _cardSlide.Length - 1);
		Source.PlayOneShot(_cardSlide[n]);
	}
	public void PlayCardHit() {
		var n = Random.Range(0, _cardHit.Length - 1);
		Source.PlayOneShot(_cardHit[n]);
	}

	public void PlayCardFlip() {
		var n = Random.Range(0, _cardFlip.Length - 1);
		Source.PlayOneShot(_cardFlip[n]);
	}
}
