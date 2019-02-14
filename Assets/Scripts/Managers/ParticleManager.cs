using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

	public static ParticleManager self;
	private GameObject particles_parent;
    private Dictionary<string, GameObject> particles;

	void Awake() {
		self = this;
		particles_parent = new GameObject();
		particles_parent.name = "Particles";

		particles = new Dictionary<string, GameObject>();
        GameObject[] particles_to_add = Resources.LoadAll<GameObject>("Particles");
        foreach (GameObject particle in particles_to_add) {
            particles.Add(particle.name, particle);
        }

	}

	void Start () {
		
	}
	
	void Update () {
		
	}

	public GameObject Spawn(string name, Vector3 position) {
		return Instantiate(particles[name], position, Quaternion.identity, particles_parent.transform);
	}
}
