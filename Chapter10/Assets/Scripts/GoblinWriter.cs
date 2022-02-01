using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinWriter : MonoBehaviour {

    public TMPro.TextMeshProUGUI textMesh;
    public List<string> goblinJobs;
    public int goblinMaxAge = 200;

    // Start is called before the first frame update
    void Start()
    {
        UpdateString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UpdateString();
        }
    }

    void UpdateString() {
        string goblinName = GoblinNameGenerator.RandomGoblinName();
        string goblinAge = Random.Range(20, goblinMaxAge).ToString();
        string goblinJob = goblinJobs[Random.Range(0, goblinJobs.Count)];
        textMesh.text = $"{goblinName} is a {goblinAge} years old goblin {goblinJob}.";
    }
}
