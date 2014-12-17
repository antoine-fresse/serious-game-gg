using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using DG.Tweening;

public class EndScreen : MonoBehaviour {

	public Text CorruptionText;
	public Text SexismeText;
	public Text FinalWord;


	public void ShowPanel() {
		gameObject.SetActive(true);

		var seq = DOTween.Sequence();

		var c = GameManager.instance.localPlayer.corruption;
		var s = GameManager.instance.localPlayer.sexisme;
		var word = "";
		
		if (GameManager.instance.LocalHasLost == true) {
			if (c < 10 && s < 10)
				word = "Vous jouez fair-play, c'est une victoire en soi !";
			else if (c - s > 10)
				word = "Les coups bas se retournent vite contre vous !";
			else if (s - c > 10)
				word = "L'\u00E9galit\u00E9 des sexes c'est important, la prochaine fois pensez-y !";
			else
				word = "Ripoux et sexiste ? La recette pour un d\u00E9sastre !";
		}
		else {
			if (c < 10 && s < 10)
				word = "Gagner en jouant fair-play ! Bravo vous aimez les challenges !";
			else if (c - s > 10)
				word = "Aucun coup n'est trop bas pour vous tant qu'il produit des r\u00E9sultats !";
			else if (s - c > 10)
				word = "L'\u00E9galit\u00E9 des sexes ne vous int\u00E9resse pas, l'important c'est de gagner !";
			else
				word = "Vous ne reculez devant rien pour obtenir r\u00E9ussir !";
		}

		
		FinalWord.text = word;
		seq.Append(
			DOTween.To(() => 0, value => CorruptionText.text = "Corruption : " + value, 
				GameManager.instance.localPlayer.corruption, 3f).SetEase(Ease.OutCubic)
		).Append(
			DOTween.To(() => 0, value => SexismeText.text = "Sexisme : " + value, 
				GameManager.instance.localPlayer.sexisme, 3f).SetEase(Ease.OutCubic)
		).Append(
			FinalWord.transform.DOLocalMoveX(0,1.5f).SetEase(Ease.OutBounce)
		);



	}
}
