/*
    Copyright (C) 2016 FusionEd

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using UnityEngine;
using System.Collections;

public class ArrowManager : MonoBehaviour {

	public static ArrowManager Instance;

	public SteamVR_TrackedObject trackedObj;

	private GameObject currentArrow;

	public GameObject stringAttachPoint;
	public GameObject arrowStartPoint;
	public GameObject stringStartPoint;

	public GameObject arrowPrefab;

	private bool isAttached = false;

	void Awake() {
		if (Instance == null)
			Instance = this;
	}

	void OnDestroy() {
		if (Instance == this)
			Instance = null;
	}

	// Use this for initialization
	void Start () {
	
	}


	void Update() {
		AttachArrow ();
		PullString ();
	}

	private void PullString() {
		if (isAttached) {
			float dist = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
			stringAttachPoint.transform.localPosition = stringStartPoint.transform.localPosition  + new Vector3 (5f* dist, 0f, 0f);

			var device = SteamVR_Controller.Input((int)trackedObj.index);
			if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
				Fire ();
			}
		}
	}

	private void Fire() {
		float dist = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;

		currentArrow.transform.parent = null;
		currentArrow.GetComponent<Arrow> ().Fired ();

		Rigidbody r = currentArrow.GetComponent<Rigidbody> ();
		r.velocity = currentArrow.transform.forward * 25f * dist;
		r.useGravity = true;

		currentArrow.GetComponent<Collider> ().isTrigger = false;

		stringAttachPoint.transform.position = stringStartPoint.transform.position;

		currentArrow = null;
		isAttached = false;
	}

	private void AttachArrow() {
		if (currentArrow == null) {
			currentArrow = Instantiate (arrowPrefab);
			currentArrow.transform.parent = trackedObj.transform;
			currentArrow.transform.localPosition = new Vector3 (0f, 0f, .342f);
			currentArrow.transform.localRotation = Quaternion.identity;
		}
	}

	public void AttachBowToArrow() {
		currentArrow.transform.parent = stringAttachPoint.transform;
		currentArrow.transform.position = arrowStartPoint.transform.position;
		currentArrow.transform.rotation = arrowStartPoint.transform.rotation;

		isAttached = true;
	}
}
