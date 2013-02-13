using UnityEngine;
using System.Collections;


public class LogoBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		//gameObject.renderer.enabled = false;
		//foreach(Component _component in gameObject.GetComponents(typeof(MeshRenderer))){
		//	Debug.Log("_component = " + _component.name);
		//}
		//transform.gameObject.renderer.material.SetColor("", Color.clear);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	/*
	IEnumerable TweenToAlpha(float duration, float toAlpha) {
    	Hashtable colorProperty = new Hashtable();
    	Hashtable alphaProperty = new Hashtable();
    	ArrayList propList = new ArrayList();
		
		colorProperty.Add("colorName", "_Color");
		alphaProperty.Add("a", toAlpha);
		propList.Add(colorProperty);
		propList.Add(alphaProperty);
		
		foreach(Renderer _renderer in gameObject.GetComponentsInChildren(typeof(Renderer))) {
    		foreach (Material _material in _renderer.materials) {
				Ani.
				Ani.Mate.To(_material, duration, propList);
			}
		}
		yield return WaitForSeconds(duration);
	  }*/

}
