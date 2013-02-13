using UnityEngine;
using System.Collections;

public class PhotoGrabber : MonoBehaviour
{
    public string url = null;

    IEnumerator Start()
    {
        if (DataModel.Instance.filePath == "")
        {
            // Load from Resources

           // Debug.Log("Loading: " + url + " from Resources");
            Texture2D texture = Resources.Load(url) as Texture2D;

            if (texture == null)
            {
                //texture = Resources.Load("images/logo_words_300.jpg") as Texture2D;
                texture = Resources.Load("images/alt_photo.jpg") as Texture2D;
            }

            renderer.material.mainTexture = texture;
        }
        else
        {
			
			Debug.Log("***loading photo: "+ url);
            WWW www = new WWW(url);
		
            yield return www;
	
			renderer.material.mainTexture = www.texture;
			
			if(www.error != null) {
				//Texture2D texture = Resources.Load("images/weave_logo_words_300.jpg") as Texture2D;
                Texture2D texture = Resources.Load("images/alt_photo.jpg") as Texture2D;
				renderer.material.mainTexture = texture;
			}
	  
        }

    }
   
	public bool UseLocalAsset ()
	{
		string[] elements = url.Split (new char[] {'/'});
		string assetName = elements [elements.Length - 1];
		bool returnValue = false;
		Texture2D texture2D;

		if (assetName.Contains (".jpeg")) {
			assetName = assetName.Remove(assetName.IndexOf(".jpeg"));
		} else if (assetName.Contains (".jpg")) {
			assetName = assetName.Remove(assetName.IndexOf(".jpg"));
		} else if (assetName.Contains (".png")) {
			assetName = assetName.Remove(assetName.IndexOf(".png"));
		}

		texture2D = (Texture2D)Resources.Load (assetName);

		if (texture2D != null) {
			Debug.Log("weave: loading asset locally");
			renderer.material.mainTexture = texture2D;
			returnValue = true;
		}

		return returnValue;
	}
}