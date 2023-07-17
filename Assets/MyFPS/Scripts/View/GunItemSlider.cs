using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using Custom = Assets.SimpleSlider.Scripts;

public class GunItemSlider : MonoBehaviour
{

	[Header("Settings")]
	[HideInInspector] public List<GunItemSlotView> gunItemSlot = new();
	public bool Elastic = true;

	[Header("UI")]
	public GunItemSlotView gunItemSlotView;
	public Transform bannerGrid;
	public Transform paginationGrid;
	public Toggle pagePrefab;
	public Custom.HorizontalScrollSnap horizontalScrollSnap;


    public void OnValidate()
    {
        GetComponent<ScrollRect>().content.GetComponent<GridLayoutGroup>().cellSize = GetComponent<RectTransform>().sizeDelta;
    }

    // Start is called before the first frame update
    void Start()
	{
		foreach (Transform child in bannerGrid)
		{
			gunItemSlot.Add(child.GetComponent<GunItemSlotView>());
			var toggle = Instantiate(pagePrefab, paginationGrid);
			toggle.group = paginationGrid.GetComponent<ToggleGroup>();
		}

		horizontalScrollSnap.Initialize();
		horizontalScrollSnap.GetComponent<ScrollRect>().movementType = Elastic ? ScrollRect.MovementType.Elastic : ScrollRect.MovementType.Clamped;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetGunItemSlotView(GunItemData gunItemData, GunItem gunItem, int bulletSize)
	{
		var instance = Instantiate(gunItemSlotView, bannerGrid);
		instance.button.image.sprite = gunItemData.itemeIcon;
		gunItem.magazineSize.Subscribe(size => instance.magazineSize.text = size.ToString()).AddTo(this);
		//instance.magazineSize.text = gunItem.magazineSize.Value.ToString();
		instance.bulletSize.text = bulletSize.ToString();
		var toggle = Instantiate(pagePrefab, paginationGrid);
		toggle.group = paginationGrid.GetComponent<ToggleGroup>();
		horizontalScrollSnap.Initialize();
	}

	public void ReplaceGunItemSlotView(int index, GunItemData gunItemData, GunItem gunItem, int bulletSize)
	{
		GunItemSlotView gunItemSlotView = bannerGrid.GetChild(index).GetComponent<GunItemSlotView>();
		gunItemSlotView.button.image.sprite = gunItemData.itemeIcon;
		gunItem.magazineSize.Subscribe(size => gunItemSlotView.magazineSize.text = size.ToString()).AddTo(this);
		//gunItemSlotView.magazineSize.text = gunItem.magazineSize.ToString();
		gunItemSlotView.bulletSize.text = bulletSize.ToString();
		horizontalScrollSnap.Initialize();
	}

}
