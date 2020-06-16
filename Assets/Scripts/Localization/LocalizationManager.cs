using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class LocalizationManager : MonoBehaviour {

	public static LocalizationManager Instance { get; private set; }

	private readonly Dictionary<string, string> _localizedText = new Dictionary<string, string>();
	private const string localizedTextMissingString = "Error";

	public bool IsReady { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		}
		else {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadLocalizedTextFile(string fileName) {
		Debug.Log("Loading text file " + fileName + "...");

		switch (Application.platform) {
			case RuntimePlatform.Android:
				StartCoroutine(C_LoadAndroid(fileName));
				break;
			case RuntimePlatform.IPhonePlayer: break;
			case RuntimePlatform.Switch:       break;
			default:
				LoadStandard(fileName);
				break;
		}
	}

	private void LoadStandard(string fileName) {
		string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

		if (File.Exists(filePath)) {
			FinishLoading(File.ReadAllText(filePath));
		}
	}

	private IEnumerator C_LoadAndroid(string fileName) {
		string filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets", fileName);
		UnityWebRequest request = UnityWebRequest.Get(filePath);
		request.SendWebRequest();

		yield return new WaitUntil(() => request.isDone);
		FinishLoading(request.downloadHandler.text);
	}

	private void FinishLoading(string jsonData) {
		if (!jsonData.Equals("")) {
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(jsonData);

			foreach (LocalizationItem item in loadedData.items) {
				_localizedText.Add(item.key, item.value);
			}

			Debug.Log("Localized text file was loaded! (" + loadedData.items.Length + " entries)");
		}
		else {
			Debug.Log("Localized text file can not be found!");
		}

		IsReady = true;
	}

	public string GetLocalizedText(string key) {
		return (_localizedText != null && _localizedText.ContainsKey(key)) ? _localizedText[key] : localizedTextMissingString;
	}

	/// <summary>
	/// Only used for test purposes!
	/// </summary>
	/// <param name="index">Index of the text snippet.</param>
	public string GetLocalizedText(int index) {
		List<string> values = new List<string>();

		foreach (string item in _localizedText.Values) {
			values.Add(item);
		}
		
		return index < values.Count ? values[index] : localizedTextMissingString;
	}
}