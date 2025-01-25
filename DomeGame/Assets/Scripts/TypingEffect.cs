using UnityEngine;

public struct TypingEffect
{
    public string fullText, currentText;
    public float typingTimer;
    public int currentCharIndex;
    public bool typingComplete;

    public static void HandleTypingEffect(ref TypingEffect typingEffect, float typingSpeed)
    {
        if (string.IsNullOrWhiteSpace(typingEffect.fullText)) return;
        if (typingEffect.typingComplete) return;
        typingEffect.typingTimer += Time.deltaTime; // update time used
        if (typingEffect.typingTimer < typingSpeed) return;
        if (typingEffect.currentCharIndex > typingEffect.fullText.Length) return;

        typingEffect.currentText = typingEffect.fullText.Substring(0, typingEffect.currentCharIndex);
        typingEffect.currentCharIndex++;
        typingEffect.typingTimer = 0f;

        if (typingEffect.currentCharIndex > typingEffect.fullText.Length) {
            typingEffect.typingComplete = true;
        }
    }
}