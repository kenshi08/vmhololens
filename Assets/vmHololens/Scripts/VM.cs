using UnityEngine;
using System;
using UnityEngine.UI;
using IFocusable = HoloToolkit.Unity.InputModule.IFocusable;
using IInputHandler = HoloToolkit.Unity.InputModule.IInputHandler;
using HoloToolkit.Unity.InputModule;

[Serializable]
public class VM : MonoBehaviour, IFocusable, IInputHandler
{
    public Renderer rend;
    public GameObject infoCanvas;
    public Text info;

    public Material onMat;
    public Material offMat;
    public Sprite UIPowerOff;
    public Sprite UIPowerOn;
    public Button powerBtn;

    public Sprite UIDragOn;
    public Sprite UIDragOff;
    public Button dragBtn;
    
    private Host parentHost;

    private vapitypes.Summary aboutThisVM;

    private string on = "POWERED_ON";
    private string off = "POWERED_OFF";

    private bool isTogglePowerStateRequested = false;

    public vapitypes.Summary AboutThisVM
    {
        get
        {
            return aboutThisVM;
        }
        set
        {
            aboutThisVM = value;
        }
    }


    private Vector3 initialPosition;
    public Vector3 InitialPosition
    {
        get
        {
            return initialPosition;
        }
        set
        {
            initialPosition = value;
        }
    }

    private Vector3 initialRotation;
    public Vector3 InitialRotation
    {
        get
        {
            return initialRotation;
        }
        set
        {
            initialRotation = value;
        }
    }

    private bool isVMDroppped = false;
    public bool IsVMDropped
    {
        get
        {
            return isVMDroppped;
        }
        set
        {
            isVMDroppped = value;
        }
    }


    public void Init(vapitypes.Summary aboutThisVM, Host parentHost)
    {
        AboutThisVM = aboutThisVM;
        this.parentHost = parentHost;

        info.text = "vm : " + AboutThisVM.vm +
            "\nName : " + AboutThisVM.name +
            "\nPower State : " + AboutThisVM.power_state +
            "\nCPU Count : " + AboutThisVM.cpu_count +
            "\nMemory Size : " + AboutThisVM.memory_size_MiB;

        GetComponent<HandDraggable>().StoppedDragging += StoppedDragging;
        GetComponent<HandDraggable>().StartedDragging += StartedDragging;
        initialPosition = transform.position;
        //check power state so that it can assign textures
        SetUIAccordingToPowerState();
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Dustbin")
        {
            Debug.Log("Destroy this VM");
        }
    }

    void StoppedDragging()
    {

        iTween.MoveTo(gameObject, InitialPosition, 1f);
        iTween.RotateTo(gameObject, InitialRotation, 1f);
    }

    void StartedDragging()
    {
    }

    public void OnFocusEnter()
    {
    }

    public void OnFocusExit()
    {
    }

    public void OnInputDown(InputEventData eventData) {
        ShowUI(true);
        isVMDroppped = true;
    }

    public void OnInputUp(InputEventData eventData)
    {
        isVMDroppped = false;
    }

    public void ToggleDragScript()
    {
        var isEnable = GetComponent<HandDraggable>().enabled;
        if(isEnable)
        {
            ToggleDragScript(false);
        }
        else
        {
            ToggleDragScript(true);
        }
    }

    private void ToggleDragScript(bool enable)
    {
        if (enable)
        {
            GetComponent<HandDraggable>().enabled = true;
            dragBtn.image.sprite = UIDragOn;
        }
        else
        {
            GetComponent<HandDraggable>().enabled = false;
            dragBtn.image.sprite = UIDragOff;
        }
    }

    public void CloseUI()
    {
        ShowUI(false);
    }

    
    public void ToggleVMPowerState()
    {
        //already requested
        if(isTogglePowerStateRequested)
        {
            return;
        }

        isTogglePowerStateRequested = true;

        bool isPowerOn = CheckPowerState();


        if (isPowerOn)
        {
            ConnectionManager.instance.TurnOffVM(this);
        }else
        {
            ConnectionManager.instance.TurnOnVM(this);
        }

        ShowUI(false);
    }

    /// <summary>
    /// call back when power state is changed
    /// </summary>
    public void PowerStateChanged(bool isOn)
    {
        if(isOn)
        {
            aboutThisVM.power_state = on;
        }else
        {
            aboutThisVM.power_state = off;
        }
        

        SetUIAccordingToPowerState();

        // now again user can interact with power button
        isTogglePowerStateRequested = false;
    }

    /// <summary>
    /// callback when VM is deletd
    /// </summary>
    public void DeleteThisVM()
    {
        if(parentHost.vm.Contains(this))
        {
            parentHost.vm.Remove(this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks power state
    /// </summary>
    /// <returns></returns>
    private bool CheckPowerState()
    {
        var isPowerOn = false;

        if (AboutThisVM.power_state.Equals(on)) // Pehle ye tha abi change kiya to test
        {
            isPowerOn = true;
        }
        else if (AboutThisVM.power_state == off)
        {
            isPowerOn = false;
        }
        return isPowerOn;
    }

    private void SetUIAccordingToPowerState()
    {
        if (CheckPowerState())
        {
            powerBtn.image.sprite = UIPowerOff;
            rend.material = onMat;
        }
        else
        {
            powerBtn.image.sprite = UIPowerOn;
            rend.material = offMat;
        }
    }

    private void ShowUI(bool arg)
    {
        infoCanvas.SetActive(arg);
    }
}
