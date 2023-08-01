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
    void Awake()
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

	public void SetGunItemSlotView(GunModel.GunSlotSubjectData gunSlotSubjectData, IntReactiveProperty bulletSize)
	{
		var instance = Instantiate(gunItemSlotView, bannerGrid);
		instance.button.image.sprite = gunSlotSubjectData.gunItemData.itemeIcon;
		gunSlotSubjectData.gunItem.magazineSize.Subscribe(size => instance.magazineSize.text = size.ToString()).AddTo(this);
		
		bulletSize.Subscribe(size => instance.bulletSize.text = size.ToString()).AddTo(this);
		var toggle = Instantiate(pagePrefab, paginationGrid);
		toggle.group = paginationGrid.GetComponent<ToggleGroup>();
		horizontalScrollSnap.Initialize();
	}

	private void ReplaceGunItemSlotView(GunModel.GunSlotSubjectData gunSlotSubjectData, IntReactiveProperty bulletSize, int index)
	{
		GunItemSlotView gunItemSlotView = bannerGrid.GetChild(index).GetComponent<GunItemSlotView>();
		gunItemSlotView.button.image.sprite = gunSlotSubjectData.gunItemData.itemeIcon;
		gunSlotSubjectData.gunItem.magazineSize.Subscribe(size => {
			gunItemSlotView.magazineSize.text = size.ToString();
			Debug.Log(index + " 番目のSliderのマガジンサイズを " + size);
		}).AddTo(this);
		bulletSize.Subscribe(size => gunItemSlotView.bulletSize.text = size.ToString()).AddTo(this);
		horizontalScrollSnap.Initialize();
	}

	public void SetGunItemSlotViewLogic(bool isFirst,GunModel.GunSlotSubjectData gunSlotSubjectData, IntReactiveProperty bulletSize, int index)
	{
        switch (gunSlotSubjectData.gunSlotSubjectBehaviour)
        {
            case GunSlotSubjectBehaviour.Add:
				if (isFirst) ReplaceGunItemSlotView(gunSlotSubjectData, bulletSize, index); 
				else SetGunItemSlotView(gunSlotSubjectData, bulletSize);
                break;
            case GunSlotSubjectBehaviour.Replace:
				ReplaceGunItemSlotView(gunSlotSubjectData, bulletSize, index);
				break;
            case GunSlotSubjectBehaviour.Remove:
                break;
            default:
                break;
        }
    }

	public void SwitchMagazineSizeSubscribe()
	{
		//GunItemSlotView gunItemSlotView = bannerGrid.GetChild(index).GetComponent<GunItemSlotView>();
	}

}
