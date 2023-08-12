namespace QRCode.Editor.Integration
{
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using UnityEditor;
	using UnityEditor.Localization;
	using UnityEngine;
	using UnityEngine.Localization;
	using UnityEngine.Localization.Tables;

	public class AudioIntegrationWindowEditor : EditorWindow
	{
		private AssetTableCollection m_assetTableCollection;
		private string m_rootFilePath;

		[MenuItem("QRCode/Integration/AudioIntegration")]
		static void Init()
		{
			AudioIntegrationWindowEditor window = (AudioIntegrationWindowEditor)EditorWindow.GetWindow(typeof(AudioIntegrationWindowEditor));

			window.name = "Audio Integration Window";
			window.Show();
		}

		private void OnGUI()
		{
			GUILayout.Label("Audio Integration Window");
			m_assetTableCollection = (AssetTableCollection)EditorGUILayout.ObjectField(new GUIContent("Audio Table Collection", "Select the Table Collection to fill."), m_assetTableCollection, typeof(AssetTableCollection), true);
			m_rootFilePath = EditorGUILayout.TextField(new GUIContent("Root File Path", "Select the root file and copy/paste the path."), m_rootFilePath);

			if (GUILayout.Button("Generate"))
			{
				string[] subfolders = AssetDatabase.GetSubFolders(m_rootFilePath);
				Debug.Log($"{subfolders.Length} subfolders founded.");

				Dictionary<Locale, List<AudioClip>> dictionary = new Dictionary<Locale, List<AudioClip>>();

				for (int i = 0; i < subfolders.Length; i++)
				{
					string localeFileName = Path.GetFileName(subfolders[i]);
					string path = subfolders[i];
					string sanitizePath = path.Remove(0, 7); //Remove "Assets/"
					AudioClip[] clips = GetAtPath<AudioClip>(sanitizePath);

					//await AwaitLocalizationInit();

					// JMarin: Use editor settings instead of runtime settings to avoid initialization issues (maybe?)
					Locale locale = LocalizationEditorSettings.GetLocale(new LocaleIdentifier(localeFileName));

					if (locale != null)
					{
						dictionary.Add(locale, clips.ToList());

						for (int j = 0; j < clips.Length; j++)
						{
							SanitizeAudioClipName(subfolders + "/" + clips[j].name, clips[j], "_" + dictionary.ElementAt(i).Key.Identifier.Code);
						}

						Debug.Log($"{localeFileName} : {clips.Length} audio clips founded");
					}
					else
					{
						Debug.LogError("Error -> locale not founded " + localeFileName);
					}
				}

				for (int i = 0; i < dictionary.Count; i++)
				{
					for (int j = 0; j < dictionary.ElementAt(i).Value.Count; j++)
					{
						string key = dictionary.ElementAt(i).Value[j].name;
						string sanitizeKey = SanitizeKeyName(key);

						AudioClip clip = dictionary.ElementAt(i).Value[j];

						AssetTable assetTable = m_assetTableCollection.GetTable(dictionary.ElementAt(i).Key.Identifier) as AssetTable;
						m_assetTableCollection.AddAssetToTable(assetTable, sanitizeKey, clip, false);
					}
				}
			}

			if (GUILayout.Button("Clear Table Collection"))
			{
				m_assetTableCollection.ClearAllEntries();
			}

			AssetDatabase.SaveAssets();
		}

		private void SanitizeAudioClipName(string path, AudioClip a_audioClip, string a_suffix)
		{
			if (a_audioClip.name.EndsWith(a_suffix) == false)
			{
				string newName = a_audioClip.name + a_suffix;
				string assetPath = AssetDatabase.GetAssetPath(a_audioClip);
				AssetDatabase.RenameAsset(assetPath, newName);
			}
		}

		private string SanitizeKeyName(string key)
		{
			int index = key.LastIndexOf('_');

			if (index != -1)
			{
				key = key.Remove(index);
			}

			return key;
		}

		private T[] GetAtPath<T>(string path) where T : UnityEngine.Object
		{
			ArrayList arrayList = new ArrayList();
			string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

			foreach (string fileName in fileEntries)
			{
				string temp = fileName.Replace("\\", "/");
				int index = temp.LastIndexOf("/");
				string localPath = "Assets/" + path;

				if (index > 0)
					localPath += temp.Substring(index);

				T asset = AssetDatabase.LoadAssetAtPath<T>(localPath);

				if (asset != null)
					arrayList.Add(asset);
			}

			T[] result = new T[arrayList.Count];

			for (int i = 0; i < arrayList.Count; i++)
				result[i] = (T)arrayList[i];

			return result;
		}
	}
}
