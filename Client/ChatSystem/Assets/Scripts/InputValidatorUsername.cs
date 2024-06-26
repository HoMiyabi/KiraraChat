using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputValidatorUsername : MonoBehaviour
{
    private InputField input;
    private void Awake()
    {
        input = GetComponent<InputField>();
        input.onValidateInput = InputValidateUsername;
    }

    private char InputValidateUsername(string text, int charIndex, char addedChar)
    {
        if (('a' <= addedChar && addedChar <= 'z') ||
            ('A' <= addedChar && addedChar <= 'Z') ||
            ('0' <= addedChar && addedChar <= '9') || addedChar == '_')
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }
}
