using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[DisallowMultipleComponent]
public class Player : MonoBehaviour{

	[SerializeField]private float upForce = 50;

	[SerializeField]private float  rotValue = 5;

	[Range(0.0f,1.0f)][SerializeField]private float reactive = 0.03f;
	
	
	[SerializeField]private float maxSpeed = 10;


	[SerializeField] private GameObject deathParticles;
	
	// Use this for initialization
	void Start (){
		rb = GetComponent<Rigidbody>();
	
	}


	private void thurst(){
		
		if (Input.GetKey(KeyCode.W)){
			rb.AddRelativeForce(new Vector3(0, upForce, 0));
		}

	}


	private void rotate(){



		
		if (Input.GetKey(KeyCode.D)){
			rb.AddTorque(0, 0, -rotValue);

		}

		if (Input.GetKey(KeyCode.A)){
			rb.AddTorque(0, 0, rotValue);

		}


		
	}

	private void correct(){
		float len = rb.velocity.magnitude;
		Vector3 velocity = rb.velocity;
		Vector3 proj = Vector3.Project(velocity, transform.up);
		
		
		//exponential correction factor
		rb.AddForce(-reactive * (velocity - proj), ForceMode.Impulse);
		Debug.DrawLine(rb.position, rb.position + rb.velocity, Color.black, 0.0f, false);

	}

	private void limit(){
		
		//limit velocity
		Vector3 vel = rb.velocity;

		if (vel.sqrMagnitude > maxSpeed * maxSpeed){

			float velLen = vel.magnitude;
			rb.AddForce(vel.normalized*(maxSpeed-velLen),ForceMode.Impulse);
			
		}

	}
	
	
	// using fixedUpdate
	private void FixedUpdate(){
		
		thurst();
		rotate();
		correct();
		limit();
		
		
		
		
		
		


		if (Debug.isDebugBuild){

			if (Input.GetKeyDown(KeyCode.K)){
				Utils.LoadNextScene();
			}
			if (Input.GetKeyDown(KeyCode.R)){
				Utils.LoadCurrentScene();
			}
			
		}


	}

	private void OnCollisionEnter(Collision other){




		if (other.gameObject.layer == LayerMask.NameToLayer("EndPad")){
		
			Debug.Log("collider with end pad,go to next level");
			
			Utils.LoadNextScene();
			
			
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
			Debug.Log("collided with ground");
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle")){
			
			Debug.Log("collided with obastacle");


			StartCoroutine(_killPlayer(5));
			
			
		}
		
	}

	IEnumerator _killPlayer(float seconds){


		float currentTime = 0;


		ParticleSystem dp = deathParticles.GetComponent<ParticleSystem>();
		
		dp.Play();
		while (currentTime < seconds){
	
			Debug.Log("here in kill");
			currentTime += Time.deltaTime;
			
			yield return null;
			
		}
		
		dp.Stop();
		
		Utils.LoadCurrentScene();

	}
	
	

	private Rigidbody rb;



}
