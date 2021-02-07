using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class CameraController : Singleton<CameraController>
{

	private List<ICameraTarget> targets = new List<ICameraTarget>();

	private Vector3 focus { get { return GetCenterPosition(); } }

	[SerializeField, Range(1f, 40f)]
	float distance = 5f;

	[SerializeField, Min(0f)]
	float focusRadius = 5f;

	[SerializeField, Range(0f, 1f)]
	float focusCentering = 0.5f;

	[SerializeField, Range(1f, 360f)]
	float rotationSpeed = 90f;

	[SerializeField, Range(-89f, 89f)]
	float minVerticalAngle = -45f, maxVerticalAngle = 45f;

	[SerializeField, Min(0f)]
	float alignDelay = 5f;

	[SerializeField, Range(0f, 90f)]
	float alignSmoothRange = 45f;

	[SerializeField]
	LayerMask obstructionMask = -1;


	Camera camera;
	Camera Camera { get { return (camera == null) ? camera = GetComponent<Camera>() : camera; } }

	Vector3 focusPoint, previousFocusPoint;

	Vector2 orbitAngles = new Vector2(45f, 0f);

	float lastManualRotationTime;

	Vector3 CameraHalfExtends
	{
		get
		{
			Vector3 halfExtends;
			halfExtends.y =
				Camera.nearClipPlane *
				Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.fieldOfView);
			halfExtends.x = halfExtends.y * Camera.aspect;
			halfExtends.z = 0f;
			return halfExtends;
		}
	}

	void OnValidate()
	{
		if (maxVerticalAngle < minVerticalAngle)
		{
			maxVerticalAngle = minVerticalAngle;
		}
	}

	void Awake()
	{
		transform.localRotation = Quaternion.Euler(orbitAngles);
	}

	private void OnEnable()
	{
		if (Managers.Instance == null)
			return;
	}

	private void OnDisable()
	{
		if (LevelManager.Instance == null)
			return;
	}

	public void AddTarget(ICameraTarget cameraTarget)
	{
		if (!targets.Contains(cameraTarget))
			targets.Add(cameraTarget);
	}

	public void RemoveTarget(ICameraTarget cameraTarget)
	{
		if (targets.Contains(cameraTarget))
			targets.Remove(cameraTarget);
	}

	void LateUpdate()
	{
		if (focus == null)
			return;

		UpdateFocusPoint();
		Quaternion lookRotation;
		if (AutomaticRotation())
		{
			ConstrainAngles();
			//
			lookRotation = Quaternion.Euler(orbitAngles);
		}
		else
		{
			lookRotation = transform.localRotation;
		}

		Vector3 lookDirection = lookRotation * Vector3.forward;
		Vector3 lookPosition = focusPoint - lookDirection * distance;

		Vector3 rectOffset = lookDirection * Camera.nearClipPlane;
		Vector3 rectPosition = lookPosition + rectOffset;
		Vector3 castFrom = focus;
		Vector3 castLine = rectPosition - castFrom;
		float castDistance = castLine.magnitude;
		Vector3 castDirection = castLine / castDistance;

		if (Physics.BoxCast(
			castFrom, CameraHalfExtends, castDirection, out RaycastHit hit,
			lookRotation, castDistance, obstructionMask
		))
		{
			rectPosition = castFrom + castDirection * hit.distance;
			lookPosition = new Vector3(0, 0, 0);
		}

		transform.SetPositionAndRotation(lookPosition, lookRotation);
	}



	void UpdateFocusPoint()
	{
		previousFocusPoint = focusPoint;
		Vector3 targetPoint = focus;
		if (focusRadius > 0f)
		{
			float distance = Vector3.Distance(targetPoint, focusPoint);
			float t = 1f;
			if (distance > 0.01f && focusCentering > 0f)
			{
				t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
			}
			if (distance > focusRadius)
			{
				t = Mathf.Min(t, focusRadius / distance);
			}
			focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
		}
		else
		{
			focusPoint = targetPoint;
		}
	}

	bool ManualRotation()
	{
		Vector2 input = new Vector2(
			Input.GetAxis("Vertical"),
			Input.GetAxis("Horizontal")
		);
		const float e = 0.001f;
		if (input.x < -e || input.x > e || input.y < -e || input.y > e)
		{
			orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
			lastManualRotationTime = Time.unscaledTime;
			return true;
		}
		return false;
	}

	bool AutomaticRotation()
	{
		if (Time.unscaledTime - lastManualRotationTime < alignDelay)
		{
			return false;
		}

		Vector2 movement = new Vector2(
			focusPoint.x - previousFocusPoint.x,
			focusPoint.z - previousFocusPoint.z
		);
		float movementDeltaSqr = movement.sqrMagnitude;
		if (movementDeltaSqr < 0.0001f)
		{
			return false;
		}

		float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
		float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
		float rotationChange =
			rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
		if (deltaAbs < alignSmoothRange)
		{
			rotationChange *= deltaAbs / alignSmoothRange;
		}
		else if (180f - deltaAbs < alignSmoothRange)
		{
			rotationChange *= (180f - deltaAbs) / alignSmoothRange;
		}
		orbitAngles.y =
			Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
		return true;
	}

	void ConstrainAngles()
	{
		orbitAngles.x =
			Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);
		//orbitAngles.y = -30f;
		if (orbitAngles.y < -30f)
		{
			orbitAngles.y += 360f;
		}
		else if (orbitAngles.y >= 330f)
		{
			orbitAngles.y -= 360f;
		}
	}

	static float GetAngle(Vector2 direction)
	{
		float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
		return direction.x < 0f ? 360f - angle : angle;
	}

	public Vector3 GetCenterPosition()
	{
		if (targets.Count == 0)
			return Vector3.zero;

		var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
		for (var i = 1; i < targets.Count; i++)
			bounds.Encapsulate(targets[i].transform.position);


		return bounds.center;
	}

	private bool isShaking = false;

	[Button]
	public void ShakeCamera(float power = 2f, float duration = 0.2f, int vibro = 2, float elasitcy = 0.5f)
	{
		if (isShaking)
			return;

		isShaking = true;
		transform.DOPunchRotation(Vector3.forward * power, duration, vibro, elasitcy).OnComplete(() => isShaking = false);
		HapticManager.Haptic(HapticTypes.RigidImpact);
	}
}