﻿using UnityEngine;

public class TankMovement : MonoBehaviour{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;


    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         

    private void Awake(){
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable (){
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable (){
        m_Rigidbody.isKinematic = true;
    }


    private void Start(){
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
		

    private void Update(){
        // Store the player's input and make sure the audio for the engine is playing.
		m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

		//Audio checker, called every fram
		EngineAudio ();
    }


    private void EngineAudio(){
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
		if(Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f){
			//Tank is stationary here. LESS than 0.1 on either side (abs)
			if (m_MovementAudio.clip == m_EngineDriving) {
				m_MovementAudio.clip = m_EngineIdling;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);

				//Play function needs to be re-played after a change has been made.
				m_MovementAudio.Play ();
			}
		}
		else{
			//Tank is moving here
			if (m_MovementAudio.clip == m_EngineIdling) {
				m_MovementAudio.clip = m_EngineDriving;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);

				//Play function needs to be re-played after a change has been made.
				m_MovementAudio.Play ();
			}
		}
    }


    private void FixedUpdate(){
		Move ();
		Turn ();
    }


    private void Move(){
		//1) Calculate how far and what direction the tank will move
		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

		//2) Move it
		//Addition here is key 
		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);

    }


    private void Turn(){
		//1) Calculate degree of turn
		float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
		//Unity deals with turning in terms of Quaternions - It stores a rotation
		Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

		//2) Rotate the tank
		//Adding quaternions does nothing. They need to be multiplied to transform
		m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}