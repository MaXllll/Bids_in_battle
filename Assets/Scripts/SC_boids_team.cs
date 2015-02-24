using UnityEngine;
using System.Collections;
using System.Threading;

public class SC_boids_team : MonoBehaviour {

	[SerializeField]
	private int _i_nb_boids;
	[SerializeField]
	private GameObject _Prefab_boid;
	private SC_boid[] _boids;

	[SerializeField]
	private float _f_distance_separation = 3;
	[SerializeField]
	private float _f_factor_separation = 3;
	
	[SerializeField]
	private float _f_distance_alignment = 6;
	[SerializeField]
	private float _f_factor_alignment = 2;
	
	[SerializeField]
	private float _f_distance_aggregation = 9;
	[SerializeField]
	private float _f_factor_aggregation = 2;

	public Vector3 _V3_destination = Vector3.zero;
	private Vector3 _V3_center_of_mass = Vector3.zero;
	public Vector3 _V3_destination_direction { get { return _V3_destination - _V3_center_of_mass; } }

	private bool _b_is_calcul_target_finish = true;


	void Start()
	{
		_boids = new SC_boid[_i_nb_boids];
		for(int i = 0; i < _i_nb_boids; ++i)
		{
			GameObject GO_tmp = Instantiate(_Prefab_boid, new Vector3(Random.value * 20 - 10, 0f, Random.value * 20 - 10), Quaternion.Euler(new Vector3(0f, Random.value * 360, 0f))) as GameObject;
			_boids[i] = GO_tmp.GetComponent<SC_boid>();
			_boids[i]._boids_team = this;
		}
	}

	void Update()
	{
		if (_b_is_calcul_target_finish)
		{
			Vector3 V3_center_of_mass = Vector3.zero;
			for(int i = 0; i < _boids.Length; ++i)
			{
				V3_center_of_mass += _boids[i]._T_boid.position;
				_boids[i].UpdateThreadInfo();
			}
			_V3_center_of_mass = V3_center_of_mass / _boids.Length;

			_b_is_calcul_target_finish = false;
			Thread thread = new Thread(UpdateBoidsTarget);
			thread.Start();
		}
	}

	private void UpdateBoidsTarget()
	{
		for (int i = 0; i < _boids.Length; ++i)
		{
			Vector3 V3_target_separation = Vector3.zero;
			Vector3 V3_target_alignment = Vector3.zero;
			Vector3 V3_target_aggregation = Vector3.zero;
//			for (int j = 0; j < _i_nb_boids; ++j)
//			{
//				float f_distance = Vector3.Distance(_boids[i]._V3_thread_position, _boids[j]._V3_thread_position);
//				if (f_distance > 0 && f_distance < _f_distance_separation)
//				{
//					V3_target_separation += (_boids[j]._V3_thread_position - _boids[i]._V3_thread_position).normalized * (f_distance - _f_distance_separation) / _f_distance_separation;
//				}
//				
//				if (f_distance > 0 && f_distance < _f_distance_alignment)
//				{
//					V3_target_alignment += _boids[j]._V3_thread_velocity.normalized * (f_distance - _f_distance_separation) / _f_distance_separation;
//				}
//				
//				if (f_distance > 0 && f_distance < _f_distance_aggregation)
//				{
//					V3_target_aggregation += (_boids[i]._V3_thread_position - _boids[j]._V3_thread_position).normalized * (f_distance - _f_distance_aggregation) / _f_distance_aggregation;
//				}
//			}



			int i_nb_near_boids_separation = 0;
			int i_nb_near_boids_alignment = 0;
			int i_nb_near_boids_aggregation = 0;
			for (int j = 0; j < _i_nb_boids; ++j)
			{
				float f_distance = Vector3.Distance(_boids[i]._V3_thread_position, _boids[j]._V3_thread_position);
				if (f_distance > 0 && f_distance < _f_distance_separation)
				{
					++i_nb_near_boids_separation;
					V3_target_separation += (_boids[j]._V3_thread_position - _boids[i]._V3_thread_position).normalized * (f_distance - _f_distance_separation) / _f_distance_separation;
				}
				
				if (f_distance > 0 && f_distance < _f_distance_alignment)
				{
					++i_nb_near_boids_alignment;
					V3_target_alignment += _boids[j]._V3_thread_velocity.normalized * (f_distance - _f_distance_separation) / _f_distance_separation;
				}
				
				if (f_distance > 0 && f_distance < _f_distance_aggregation)
				{
					++i_nb_near_boids_aggregation;
					V3_target_aggregation += (_boids[i]._V3_thread_position - _boids[j]._V3_thread_position).normalized * (f_distance - _f_distance_aggregation) / _f_distance_aggregation;
				}
			}
			
			if (i_nb_near_boids_separation > 0)
				V3_target_separation /= i_nb_near_boids_separation;
			V3_target_separation.Normalize();
			
			if (i_nb_near_boids_alignment > 0)
				V3_target_alignment /= i_nb_near_boids_alignment;
			V3_target_alignment.Normalize();
			
			if (i_nb_near_boids_aggregation > 0)
				V3_target_aggregation /= i_nb_near_boids_aggregation;
			float f_distance_factor = Vector3.Distance(_boids[i]._V3_thread_position, V3_target_aggregation) / _f_distance_aggregation;
			V3_target_aggregation -= _boids[i]._V3_thread_position;
			V3_target_aggregation.Normalize();
			V3_target_aggregation *= f_distance_factor;



			Vector3 V3_target = Vector3.zero;
			V3_target += V3_target_separation * _f_factor_separation;
			V3_target += V3_target_alignment * _f_factor_alignment;
			V3_target += V3_target_aggregation * _f_factor_aggregation;

			if (V3_target.magnitude < 0.4f)
				V3_target = Vector3.zero;
			else
				V3_target.Normalize();
			_boids[i]._V3_target = V3_target;
		}

		_b_is_calcul_target_finish = true;
	}
}
