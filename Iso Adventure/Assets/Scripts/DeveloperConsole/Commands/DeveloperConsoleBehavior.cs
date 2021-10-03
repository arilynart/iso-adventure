using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Arilyn.DeveloperConsole.Commands;

namespace Arilyn.DeveloperConsole.Behavior
{
    public class DeveloperConsoleBehavior : MonoBehaviour
    {
        [SerializeField]
        private string prefix = string.Empty;
        [SerializeField]
        private ConsoleCommand[] commands = new ConsoleCommand[0];

        [Header("UI")]
        [SerializeField]
        private GameObject uiCanvas = null;
        [SerializeField]
        private TMP_InputField inputField = null;

        private float pausedTimeScale;

        private static DeveloperConsoleBehavior INSTANCE;
        public static PlayerController PLAYER;

        private DeveloperConsole developerConsole;

        private DeveloperConsole DeveloperConsole
        {
            get
            {
                if (developerConsole != null) return developerConsole;
                return developerConsole = new DeveloperConsole(prefix, commands);
            }
        }

        private void Awake()
        {
            if (INSTANCE != null && INSTANCE != this)
            {
                Destroy(gameObject);
                return;
            }

            INSTANCE = this;

            DontDestroyOnLoad(gameObject);
        }

        public void Toggle(InputAction.CallbackContext context)
        {
            if (!context.action.triggered) return;
            UIToggle();
        }

        public void UIToggle()
        {

            if (uiCanvas.activeSelf)
            {
                PLAYER.controls.Gameplay.Enable();
                Time.timeScale = pausedTimeScale;
                uiCanvas.SetActive(false);
            }
            else
            {
                PLAYER.controls.Gameplay.Disable();
                pausedTimeScale = Time.timeScale;
                Time.timeScale = 0;
                uiCanvas.SetActive(true);
                inputField.ActivateInputField();
            }
        }

        public void ProcessCommand(string inputValue)
        {
            DeveloperConsole.ProcessCommand(inputValue);

            inputField.text = string.Empty;
            UIToggle();
        }
    }
}
