using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public BulletPool bulletPool;

	void OnEnable()
	{
		StartCoroutine(nameof(CheckIfAlive));
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			if(!GetComponent<ParticleSystem>().IsAlive(true))
			{
				bulletPool.RereaseHole(this);
				break;
			}
		}
	}
}
