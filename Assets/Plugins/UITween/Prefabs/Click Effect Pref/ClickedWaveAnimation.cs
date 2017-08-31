using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickedWaveAnimation : MonoBehaviour {

	public GameObject WaveObject;
	public GameObject CanvasMain;

	public int PoolSize;

	private Pool poolClass;

	void Start()
	{
		poolClass = gameObject.AddComponent<Pool>();
		poolClass.CreatePool(WaveObject, PoolSize);
	}

	void Update () 
	{
		if (Input.GetMouseButtonDown(0) 
#if UNITY_EDITOR
		    || Input.GetMouseButtonDown(1) 
#endif
		    )
		{
			GameObject hittedUIButton = UiHitted();

			if (hittedUIButton)
			{
				CreateWave(hittedUIButton.transform);
			}
		}
	}

	void CreateWave(Transform Parent)
	{
		GameObject wave = poolClass.GetObject();

		if (wave)
		{
			wave.transform.SetParent( CanvasMain.transform );
			wave.GetComponent<MaskableGraphic>().color = Parent.GetComponent<MaskableGraphic>().color - new Color(.1f, .1f, .1f);

			Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

			mousePos.x = mousePos.x * Screen.width - Screen.width / 2f;
			mousePos.y = mousePos.y * Screen.height - Screen.height / 2f;
			mousePos.z = 0f;

			wave.GetComponent<RectTransform>().localPosition = mousePos / CanvasMain.transform.localScale.x;
			wave.transform.SetParent( Parent );
			wave.GetComponent<EasyTween>().OpenCloseObjectAnimation();
		}
	}

	public GameObject UiHitted()
	{
		PointerEventData pe = new PointerEventData(EventSystem.current);
		pe.position =  Input.mousePosition;
		
		List<RaycastResult> hits = new List<RaycastResult>();
		EventSystem.current.RaycastAll( pe, hits );

		for (int i = 0; i < hits.Count ; i++)
		{
			if (hits[i].gameObject.GetComponent<Button>() && hits[i].gameObject.GetComponent<Mask>())
			{
				return hits[i].gameObject;
			}
		}

		return null;
	}
}

public class Pool : MonoBehaviour {

	private GameObject[] ObjectPool;
	private GameObject ObjectToPool;

	public void CreatePool(GameObject ObjectToPool, int numberOfObjects)
	{
		ObjectPool = new GameObject[numberOfObjects];
		this.ObjectToPool = ObjectToPool;

		for (int i = 0; i < ObjectPool.Length; i++)
		{
			ObjectPool[i] = Instantiate(ObjectToPool) as GameObject;
			ObjectPool[i].SetActive(false);
		}
	}

	public GameObject GetObject()
	{
		for (int i = 0; i < ObjectPool.Length; i++)
		{
			if (ObjectPool[i])
			{
				if (!ObjectPool[i].activeSelf)
				{
					ObjectPool[i].SetActive(true);
					return ObjectPool[i];
				}
			}
			else
			{
				ObjectPool[i] = Instantiate(ObjectToPool) as GameObject;
				ObjectPool[i].SetActive(false);
			}
		}

		return null;
	}
}