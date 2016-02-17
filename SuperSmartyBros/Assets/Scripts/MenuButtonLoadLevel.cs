using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButtonLoadLevel : MonoBehaviour {

	public void loadLevel(string leveltoLoad)
	{
		SceneManager.LoadScene(leveltoLoad);
	}
}
