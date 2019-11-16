using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour {
    
    // Engine objects
    private List<GameObject> targets;

    public GameObject target;
    public GameObject targetH;
    public GameObject targetV;
    public GameObject randomPointButton;
    public GameObject randomLineHVButton;
    public GameObject lockButton;
    public GameObject helpButton;
    public GameObject pickRandoObjectButton;
    public GameObject clearObjectsButton;
    public GameObject addObjectButton;
    public GameObject pointText;
    public GameObject VHlineText;
    public GameObject lockingText;
    public GameObject addingText;
    public GameObject pickingText;
    public GameObject clearText;
    public GameObject pointPanel;
    public GameObject VHlinePanel;
    public GameObject lockingPanel;
    public GameObject addingPanel;
    public GameObject pickingPanel;
    public GameObject clearPanel;
    public GameObject helpObject;

    // Materials
    public Material redMat;
    public Material greenMat;

    // Transition Controls
    private bool helpShown;
    private bool drawSomeObjects;

    void Start () {
        // Set position of the buttons
        float xPostition = 40;
        float yPostition = 50; // Height of rows + position
        float zPostition = 3;

        randomPointButton.GetComponent<Transform>().position = new Vector3(xPostition, yPostition*8, zPostition);
        randomLineHVButton.GetComponent<Transform>().position = new Vector3(xPostition, yPostition*7, zPostition);

             // SKIP A ROW IN Y POSITION

        lockButton.GetComponent<Transform>().position = new Vector3(xPostition, yPostition*5, zPostition);
        helpButton.GetComponent<Transform>().position = new Vector3(xPostition, yPostition*4, zPostition);

        addObjectButton.GetComponent<Transform>().position = new Vector3(xPostition, yPostition*3, zPostition);
        pickRandoObjectButton.GetComponent<Transform>().position = new Vector3(xPostition, yPostition*2, zPostition);
        clearObjectsButton.GetComponent<Transform>().position = new Vector3(xPostition, yPostition, zPostition);

        // Set transition controls
        drawSomeObjects = false;
        helpShown = false;

        // List initiation
        targets = new List<GameObject>();
    }

    private void Update()
    {
        // Place point on the screen
        if (Input.GetMouseButtonDown(0) && drawSomeObjects)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            spawnPosition.z = 0.0f;
            if(spawnPosition.x > minXM)
            {
                targets.Add(Instantiate(target, spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0))));
            }
        }

        // Exit app
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Move text help into picture
        if (helpShown)
        {
            // Positions to control speed of transitions
            int xGT900 = -50;
            int xGT1800 = -1000;
            int xLT4000 = 50;
            if (pointText.GetComponent<Transform>().localPosition.x > 900)
            {
                pointText.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                VHlineText.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                lockingText.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                addingText.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                pickingText.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                clearText.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);

                pointPanel.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                VHlinePanel.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                lockingPanel.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                addingPanel.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                pickingPanel.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
                clearPanel.GetComponent<Transform>().localPosition += new Vector3(xGT900, 0, 0);
            }

            if (pointText.GetComponent<Transform>().localPosition.x > 1800)
            {
                pointText.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                VHlineText.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                lockingText.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                addingText.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                pickingText.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                clearText.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);

                pointPanel.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                VHlinePanel.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                lockingPanel.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                addingPanel.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                pickingPanel.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
                clearPanel.GetComponent<Transform>().localPosition += new Vector3(xGT1800, 0, 0);
            }
        }
        else
        {
            if(pointText.GetComponent<Transform>().localPosition.x < 4000)
            {
                pointText.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                VHlineText.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                lockingText.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                addingText.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                pickingText.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                clearText.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);

                pointPanel.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                VHlinePanel.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                lockingPanel.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                addingPanel.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                pickingPanel.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
                clearPanel.GetComponent<Transform>().localPosition += new Vector3(xLT4000, 0, 0);
            }
        }
    }

    public void showHelp()
    {
        if (helpShown)
        {
            helpShown = false;
        }
        else
        {
            helpShown = true;
        }
    }

    public void activateObjectAdding()
    {
        drawSomeObjects = true;
    }

    public void deactivateObjectAdding()
    {
        drawSomeObjects = false;
    }

    public void selectOneOftheCreatedObjects()
    {
        if (drawSomeObjects)
        {
            deactivateObjectAdding();
        }
        
        int luckyWinner = Random.Range(0, targets.Count);
        for(int i = 0;  i < targets.Count; i++)
        {
            if(i == luckyWinner)
            {
                targets[i].GetComponent<Renderer>().material = greenMat;
                //add effect to show the choice
                targets[i].GetComponent<Animator>().Play("Picked");
            }
            else
            {
                targets[i].GetComponent<Renderer>().material = redMat;
            }
        }
        Debug.Log(luckyWinner);
    }

    public void clearScreen()
    {
        deactivateObjectAdding();
        for (int i = targets.Count - 1; i >= 0 ; i--)
        {
            Destroy(targets[i]);
            targets.RemoveAt(i);
        }
    }
    
    public void pointSelection()
    {
        clearScreen();

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        targets.Add(Instantiate(target, new Vector3(x, y, 0.0f), Quaternion.Euler(new Vector3(0, 0, 0))));
    }

    private bool horizontal;
    public void rowSelection()
    {
        clearScreen();
        float minX = 100;
        float maxX = 700;
        float minY = 100;
        float maxY = 400;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        if (horizontal)
        {
            targets.Add(Instantiate(targetH, new Vector3(390, y, 0.0f), Quaternion.Euler(new Vector3(0, 0, 0))));
            horizontal = false;
        }
        else
        {
            targets.Add(Instantiate(targetV, new Vector3(x, 250, 0.0f), Quaternion.Euler(new Vector3(0, 0, 90))));
            horizontal = true;
        }
    }
}
