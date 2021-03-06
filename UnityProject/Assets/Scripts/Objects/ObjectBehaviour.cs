﻿using Cupboards;
using Items;
using PlayGroup;
using UnityEngine;

/// <summary>
///     Object behaviour controls all of the basic features of an object
///     like being able to hide the obj, being able to set on fire, throwing etc
/// </summary>
public class ObjectBehaviour : PushPull
{
	[HideInInspector] public ClosetPlayerHandler closetHandlerCache;

	//Inspector is controlled by ObjectBehaviourEditor
	//Please expose any properties you need in there
	private PickUpTrigger pickUpTrigger;

	private PlayerScript playerScript;

	protected override void Awake()
	{
		base.Awake();
		//Determines if it is an item 
		pickUpTrigger = GetComponent<PickUpTrigger>();
		//Determines if it is a player
		playerScript = GetComponent<PlayerScript>();
	}

	public override void OnMouseDown()
	{
		if (PlayerManager.LocalPlayerScript.IsInReach(transform.position))
		{
			//If this is an item with a pick up trigger and player is
			//not holding control, then check if it is being pulled
			//before adding to inventory
			if (!Input.GetKey(KeyCode.LeftControl) && pickUpTrigger !=
			    null && pulledBy != null)
			{
				CancelPullBehaviour();
			}
		}
		base.OnMouseDown();
	}

	public override void OnVisibilityChange(bool state)
	{
		if (playerScript != null)
		{
			if (PlayerManager.LocalPlayerScript == playerScript)
			{
				//Local player, might be in a cupboard so add a cupboard handler. The handler will remove
				//itself if not needed
				//TODO turn the ClosetPlayerHandler into a more generic component to handle disposals bin,
				//coffins etc
				if (state)
				{
					if (closetHandlerCache)
					{
						//Set the camera to follow the player again
						if (!PlayerManager.LocalPlayerScript.playerNetworkActions.isGhost)
						{
							Camera2DFollow.followControl.target = transform;
						}
						Camera2DFollow.followControl.damping = 0f;
						Destroy(closetHandlerCache);
					}
				}
			}
		}
	}
}