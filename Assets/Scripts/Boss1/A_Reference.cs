using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Reference : MonoBehaviour
{
    [SerializeField] private MeleeBoss meleeBossScript;
	[SerializeField] private BossState bossStateScript;
	[SerializeField] private BossMirrorAttack bossMirrorAttackScript;
	
	public void DoHorizontal()
	{
		meleeBossScript.HorizontalShader();
	}
	public void DoVertical()
	{
		meleeBossScript.VerticalShader();
	}
	
	public void DoMirrors()
	{
		bossMirrorAttackScript.ActivateMirrors();
	}
}
