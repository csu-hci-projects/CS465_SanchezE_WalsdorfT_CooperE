using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    public Sprite[] Slides;
    public float SlideDuration = 1f;

    private Image _image;

    private int _currentSlideIndex = 0;
    private float _timeSinceLastSlide = 0f;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _currentSlideIndex = 0;
        _timeSinceLastSlide = 0f;

        if (Slides.Length > 0)
        {
            _image.sprite = Slides[_currentSlideIndex];
        }
    }

    private void Update()
    {
        if (Slides.Length == 0)
        {
            return;
        }
        _timeSinceLastSlide += Time.deltaTime;
        if (_timeSinceLastSlide >= SlideDuration)
        {
            _timeSinceLastSlide = 0f;
            _currentSlideIndex = (_currentSlideIndex + 1) % Slides.Length;
            _image.sprite = Slides[_currentSlideIndex];
        }
    }
}
