using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

	public static ParticleManager self;
	private GameObject particles_parent;

	public GameObject dust;
	
	void Awake() {
		self = this;
		particles_parent = new GameObject();
		particles_parent.name = "ParticlesParent";
	}

	void Start () {
		
	}
	
	void Update () {
		
	}

	public GameObject Spawn(string name, Vector3 position) {
		GameObject particle_to_instantiate = null;
		switch(name) {
			case "dust":
				particle_to_instantiate = dust;
				break;
		}
		return Instantiate(particle_to_instantiate, position, Quaternion.identity, particles_parent.transform);
	}
}
