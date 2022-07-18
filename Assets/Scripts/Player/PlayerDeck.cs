using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : Pack
{
	private WIN_STATUS _win_status;

	public WIN_STATUS win_status
	{
		get => _win_status;
		
		set
		{
			_win_status = value;
		}
	}

	public override void initializePack()
	{
		base.initializePack();
		_win_status = WIN_STATUS.NONE;
	}

	public override void playerStayed()
	{
		throw new System.NotImplementedException();
	}

	protected override void checkWinStatus()
	{
		throw new System.NotImplementedException();
	}
}
