using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Story : MonoBehaviour
{
    [Header("Story UI References")]
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private Image storyImage;
    [SerializeField] private TextMeshProUGUI storyText;
    
    [Header("Story Settings")]
    [TextArea(5, 10)]
    [SerializeField] private string storyContent = "A group of boys are playing cricket beside an abandoned, mysterious house when a powerful shot sends the ball inside; the older brother enters to retrieve it but never returns. Hours later, worried and determined, his younger brother steps into the house and discovers a shifting labyrinth of strange rooms where every space changes reality through altered controls, physics, traps, and interactive objects. As he navigates deeper, opening paired doors and choosing paths through unpredictable environments, he realizes the house is alive and testing him. Following clues and surviving increasingly bizarre challenges, he eventually finds his brother trapped in a cage, only for a witch — the entity controlling the house — to appear. Instead of fighting directly, the younger brother must use the house's own traps and strange mechanics against her, ultimately breaking her control, freeing his brother, and escaping the collapsing structure back into the real world.";
    
    [SerializeField] private float letterDelay = 0.05f;
    [SerializeField] private float imageFadeDuration = 1f;
    [SerializeField] private float storyDisplayDuration = 3f;
    [SerializeField] private bool canSkip = true;
    [SerializeField] private bool useHorizontalWipe = true;
    
    private bool isPlayingStory = false;
    private bool storyComplete = false;
    private Coroutine storyCoroutine;
    private System.Action completionCallback;
    
    private void Start()
    {
        // Hide story panel at start
        if (storyPanel != null)
        {
            storyPanel.SetActive(false);
        }
    }
    
    private void Update()
    {
        // Allow skipping with Space or Enter
        if (isPlayingStory && canSkip && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            SkipStory();
        }
    }
    
    public void PlayStory(System.Action onComplete)
    {
        completionCallback = onComplete;
        
        if (storyCoroutine != null)
        {
            StopCoroutine(storyCoroutine);
        }
        storyCoroutine = StartCoroutine(StorySequence(onComplete));
    }
    
    private IEnumerator StorySequence(System.Action onComplete)
    {
        isPlayingStory = true;
        storyComplete = false;
        
        // Show story panel
        if (storyPanel != null)
        {
            storyPanel.SetActive(true);
        }
        
        // Setup horizontal wipe if enabled
        if (useHorizontalWipe && storyImage != null)
        {
            storyImage.type = Image.Type.Filled;
            storyImage.fillMethod = Image.FillMethod.Horizontal;
            storyImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            Color imageColor = storyImage.color;
            imageColor.a = 1f;
            storyImage.color = imageColor;
        }
        
        // Fade in the image
        if (storyImage != null)
        {
            yield return StartCoroutine(FadeImage(0f, 1f, imageFadeDuration));
        }
        
        // Clear text initially
        if (storyText != null)
        {
            storyText.text = "";
        }
        
        // Display text letter by letter
        yield return StartCoroutine(TypewriterEffect());
        
        // Wait for a bit after story is complete
        yield return new WaitForSeconds(storyDisplayDuration);
        
        // Fade out
        if (storyImage != null)
        {
            yield return StartCoroutine(FadeImage(1f, 0f, imageFadeDuration));
        }
        
        // Hide story panel
        if (storyPanel != null)
        {
            storyPanel.SetActive(false);
        }
        
        isPlayingStory = false;
        storyComplete = true;
        
        // Call the completion callback
        onComplete?.Invoke();
    }
    
    private IEnumerator TypewriterEffect()
    {
        if (storyText == null) yield break;
        
        storyText.text = "";
        
        foreach (char letter in storyContent)
        {
            storyText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }
    
    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        if (storyImage == null) yield break;
        
        if (useHorizontalWipe)
        {
            // Use horizontal wipe effect with fillAmount
            float elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float fillAmount = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                storyImage.fillAmount = fillAmount;
                yield return null;
            }
            
            storyImage.fillAmount = endAlpha;
        }
        else
        {
            // Use alpha fade effect
            float elapsedTime = 0f;
            Color imageColor = storyImage.color;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                imageColor.a = alpha;
                storyImage.color = imageColor;
                yield return null;
            }
            
            imageColor.a = endAlpha;
            storyImage.color = imageColor;
        }
    }
    
    private void SkipStory()
    {
        if (storyCoroutine != null)
        {
            StopCoroutine(storyCoroutine);
        }
        
        // Complete the text immediately
        if (storyText != null)
        {
            storyText.text = storyContent;
        }
        
        // Start fade out immediately
        StartCoroutine(SkipToEnd());
    }
    
    private IEnumerator SkipToEnd()
    {
        // Quick fade out
        if (storyImage != null)
        {
            if (useHorizontalWipe)
            {
                yield return StartCoroutine(FadeImage(storyImage.fillAmount, 0f, 0.5f));
            }
            else
            {
                yield return StartCoroutine(FadeImage(storyImage.color.a, 0f, 0.5f));
            }
        }
        
        if (storyPanel != null)
        {
            storyPanel.SetActive(false);
        }
        
        isPlayingStory = false;
        storyComplete = true;
        
        // Call the completion callback to enable player movement
        completionCallback?.Invoke();
    }
}
