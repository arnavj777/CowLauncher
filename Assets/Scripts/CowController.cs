﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cowController : MonoBehaviour 
{
	[Header ("Player")]
    [SerializeField]

	 
    private CapsuleCollider mPlayerCollider;

    [SerializeField]
    private Rigidbody mPlayerRigidBody;

	[Header ("Catapult Frames")]
    [SerializeField]
    private Rigidbody mCatapultHolder;

	[Header ("Catapult Belts")]
    [SerializeField]
    private LineRenderer mLeftCatapult;

    [SerializeField]
    private LineRenderer mRightCatapult;

	[Header ("Projectile Arc")]
	[SerializeField]
	private LineRenderer mArcLineRenderer;

	[Header ("Float Controls")]
    [SerializeField]
    private float mSensitivity = 0.1f;

    [SerializeField]
    private int mMaxStretch = 5;

	[SerializeField]
	private float mSpeed=10.0f;

	//
    private Vector3 mInitialMousePos= Vector3.zero;

    private Vector3 mInitialObjectPos= Vector3.zero;

	private Vector3 mInitialPlayerPosition= Vector3.zero;

    private Vector3 mLastFrameVelocity = Vector3.zero;

    private bool mMouseUpStatus;
   
    private Ray mRayToBelt;

	private Ray mRayFromBelt;

	private WaitForSeconds mWaitOf4Sec = new WaitForSeconds (1f);

	#region UNITY_FUNCTIONS
    void OnEnable()
    {
        InitializeCatapult();

		mInitialPlayerPosition = transform.position;
    }

	void OnMouseUp()
	{
		Vector3 distance = transform.position - mCatapultHolder.transform.position;


		if (distance.sqrMagnitude > 1) 
		{
			mPlayerRigidBody.velocity = CalculateVelocitySpeed () * CalculateVelocityDirection ();

			mMouseUpStatus = true;
		} 
		else 
		{
			mMouseUpStatus = false;

			ResetPositions ();

		}
	}

	void OnMouseDown()
	{
		mMouseUpStatus = false;

		mInitialMousePos = Input.mousePosition;

		mInitialObjectPos = transform.position;

		mPlayerRigidBody.isKinematic = false;
	}

	void Update()
	{
		if (!mMouseUpStatus) 
		{
			OnBallDragging ();
		} 

		CheckForCatapultAction();

	}

	/// <summary>
	/// Raises the collision enter event and resets the position after the ball has touched the ground
	/// </summary>
	/// <param name="inObj">In object.</param>
	void OnCollisionEnter(Collision inObj)
	{
		if (mMouseUpStatus && inObj.gameObject.tag == "Ground") 
		{
			StartCoroutine (DelayedCallToReset ());
		}
	}

	#endregion

	
    private void InitializeCatapult()
    {
        mLeftCatapult.positionCount = 2;

        mRightCatapult.positionCount = 2;

        mRayToBelt = new Ray(mCatapultHolder.position, Vector3.zero);

        ResetCatapult();

        UpdateBallBelt();
    }

	private void ResetCatapult()
    {
        mPlayerRigidBody.isKinematic = true;

		mMouseUpStatus = true;
    }

   
	private void UpdateBallBelt()
    {
        mLeftCatapult.enabled = true;

        mRightCatapult.enabled = true;

        Vector3 distance = transform.position - mCatapultHolder.transform.position;

        mRayFromBelt.direction = distance;

		Vector3 maxHold = mRayFromBelt.GetPoint (distance.magnitude + 7f);
            
        mLeftCatapult.SetPositions(new Vector3[]{ mLeftCatapult.transform.position, maxHold /*+ new Vector3(0,0,-1.2f)*/});

        mRightCatapult.SetPositions(new Vector3[]{ mRightCatapult.transform.position,maxHold /*+ new Vector3(0,0,-1.2f)*/});
    }

	
    void OnBallDragging()
    {
        Vector3 pos = Input.mousePosition;

        Vector3 diff = pos - mInitialMousePos;

        diff = diff * mSensitivity;

        pos = mInitialObjectPos + diff;

        Vector3 dist =  pos - mCatapultHolder.transform.position;


		if (dist.sqrMagnitude > (mMaxStretch * mMaxStretch)) 
		{
			mRayToBelt.direction = dist;

			pos = mRayToBelt.GetPoint (mMaxStretch);
		}

		if (pos.y < 0) 
		{
			transform.position = new Vector3 (pos.x, pos.z, pos.y);
		}
		else 
		{
			transform.position = new Vector3 (pos.x, pos.z, 0);
		}
    }


    void CheckForCatapultAction()
    {
		if (transform.position.z < 0)
		{
			DrawTrajectoryPath ();

			UpdateBallBelt ();
		}
		else 
		{
			mLeftCatapult.enabled = false;

			mRightCatapult.enabled = false;

			mArcLineRenderer.enabled = false;
		}
          
    }

	
	private float CalculateVelocitySpeed()
	{
		Vector3  distanceVector = mCatapultHolder.transform.position - transform.position;

		float vel = distanceVector.sqrMagnitude * mSpeed;

		return vel;
	}

	
	private Vector3 CalculateVelocityDirection()
	{
		Vector3  distanceVector = mCatapultHolder.transform.position - transform.position;

		Vector3 direction = distanceVector.normalized;

		return direction;
	}

	
	void DrawTrajectoryPath()
	{
		mArcLineRenderer.enabled=true;

		mArcLineRenderer.positionCount = mMaxStretch;

		Vector3 pos = transform.position;

		for (int i = 0; i < mArcLineRenderer.positionCount; i++)
		{
			mArcLineRenderer.SetPosition (i, pos);

			Vector3 startVel = CalculateVelocitySpeed () * CalculateVelocityDirection () ;

			startVel += Physics.gravity * Time.fixedDeltaTime + new Vector3(0,2,0);

			pos += startVel * Time.fixedDeltaTime;
		}
	}
		
	
	IEnumerator DelayedCallToReset()
	{
		yield return mWaitOf4Sec;

		ResetPositions ();
	}

	
	void ResetPositions()
	{
		transform.position = mInitialPlayerPosition;

		Vector3 pos = transform.position ;

		transform.position =  pos;

		pos.x = transform.position.x;

		pos.y = transform.position.y + 0.75f;

		pos.z = transform.position.z - 3;

		Camera.main.transform.position = pos;

		ResetCatapult ();
	}
}
