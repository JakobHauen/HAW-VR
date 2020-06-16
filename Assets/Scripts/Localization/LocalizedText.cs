using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(TextMesh))]
public class LocalizedText : MonoBehaviour {

	[SerializeField]
	private string _key;
	private TextMesh _text;
	
	private void Awake() {
		_text = GetComponent<TextMesh>();
	}

	private void Start() {
		LoadText();
	}

	public void LoadText() {
		_text.text = LocalizationManager.Instance.GetLocalizedText(_key);
	}

	public void LoadText(string key) {
		_key = key;
		LoadText();
	}

	public void Empty() {
		_text.text = "";
	}
}