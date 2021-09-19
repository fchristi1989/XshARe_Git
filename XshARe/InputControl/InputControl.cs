using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine;

public class InputControl
{
    protected List<TouchGesture> touchGestures;
    protected List<HMDGesture> glassGestures;

    
    private ModelLogic modelLogic = null; 

    
    public InputControl(ARRaycastManager arrm)
    {
        touchGestures = new List<TouchGesture>();
        glassGestures = new List<HMDGesture>();

        modelLogic = Components.ModelLogic;

        

        TapTG tTG = new TapTG(arrm, modelLogic);
        DoubleTapTG dtTG = new DoubleTapTG(arrm, modelLogic);
        PressTG pTG = new PressTG(arrm, modelLogic);
        DragTG dTG = new DragTG(arrm, modelLogic);
        PinchSpreadTG psTG = new PinchSpreadTG(arrm, modelLogic);

        touchGestures.Add(tTG);
        touchGestures.Add(dtTG);
        touchGestures.Add(pTG);
        touchGestures.Add(dTG);
        touchGestures.Add(psTG);
    }

    public void Update()
    {
        foreach (TouchGesture tg in touchGestures)
        {
            tg.Perform();
        }

        foreach(HMDGesture gg in glassGestures)
        {
            //model.PerformOperation(gg.GetOpIfPerformed(model));
        }
    }

    

    
}
