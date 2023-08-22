using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public ObjectPools objectPool;

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
				objectPool.RereaseHole(this);
				break;
			}
		}
	}
}
