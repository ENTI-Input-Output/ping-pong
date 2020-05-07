using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRandomizer : MonoBehaviour {
    public GameObject[] tables;

    private int currentTable;
    private int oldTable;

    void Start() { // Default table as first
        currentTable = 0;
        oldTable = 0;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.W)) ChangeTable(); // Input for testing
    }

    public void ChangeTable() {
        System.Random rand = new System.Random();
        int nextTable;
        do {
            nextTable = rand.Next(tables.Length);
        } while (nextTable == oldTable);

        oldTable = nextTable;
        UpdateTables(nextTable);
        Debug.Log(nextTable);
    }

    private void UpdateTables(int chosenTable) {
        for (int i = 0; i < tables.Length; i++) {
            if (i == chosenTable) tables[i].SetActive(true);
            else tables[i].SetActive(false);
        }
    }
}
