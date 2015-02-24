using UnityEngine;
using System.Collections;

public class SC_boid : MonoBehaviour {
	
	[SerializeField]
	public Transform _T_boid;
	[SerializeField]
	private Transform _T_graphic;
	[SerializeField]
	private Animator _animator;

	public SC_boids_team _boids_team;

	[SerializeField]
	private int _i_hp = 3;
	[SerializeField]
	private int _i_attack = 1;
	[SerializeField]
	private float _f_max_speed = 10;
	[SerializeField]
	private float _f_acceleration = 20;

	public Vector3 _V3_target = Vector3.zero;
	private Vector3 _V3_velocity = Vector3.zero;

	public Vector3 _V3_thread_position;
	public Vector3 _V3_thread_velocity;


	public void UpdateThreadInfo()
	{
		_V3_thread_position = _T_boid.position;
		_V3_thread_velocity = _V3_velocity;
	}

	public void Update()
	{
		Vector3 V3_velocity_target = _boids_team._V3_destination_direction;
		if (V3_velocity_target.magnitude < 5)
			V3_velocity_target = Vector3.zero;
		else
			V3_velocity_target.Normalize();

		if (V3_velocity_target != Vector3.zero)
		{
			V3_velocity_target += _V3_target;
			V3_velocity_target.Normalize();
		}

		_V3_velocity = Vector3.MoveTowards(_V3_velocity, V3_velocity_target * _f_max_speed, _f_acceleration * Time.deltaTime);
		_V3_velocity.y = 0f;

		if (_V3_velocity != Vector3.zero)
		{
			_T_graphic.rotation = Quaternion.Lerp(_T_graphic.rotation, Quaternion.LookRotation(_V3_velocity), Time.deltaTime * 4);
			_T_boid.position += _V3_velocity * Time.deltaTime;
			if (_animator != null)
				_animator.SetBool("Run", true);
		}
		else if (_animator != null)
			_animator.SetBool("Run", false);
	}
}
