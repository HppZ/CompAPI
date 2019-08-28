using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CompAPI
{
    public sealed partial class MovieItem : UserControl
    {
        private CompositionAnimation _translationY;
        private CompositionAnimationGroup _animationGroup;
        private CompositionPropertySet _propertySet;
        private DropShadow _shadow;

        public MovieItem()
        {
            this.InitializeComponent();
            CreateAnimation();
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            StartAnimation(-8, 1.0f, 40);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            StartAnimation(0, 0, 0);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
        }

        private void StartAnimation(float translation, float shadowOpacity, float blurRadius)
        {
            _propertySet.InsertScalar("TranslateTo", translation);
            var visual = ElementCompositionPreview.GetElementVisual(this);
            visual.StartAnimation("Translation.Y", _translationY);

            _propertySet.InsertScalar("Opacity", shadowOpacity);
            _propertySet.InsertScalar("BlurRadius", blurRadius);
            _shadow.StartAnimationGroup(_animationGroup);
        }

        private void CreateAnimation()
        {
            ElementCompositionPreview.SetIsTranslationEnabled(this, true);

            var compositor = Window.Current.Compositor;
            _animationGroup = compositor.CreateAnimationGroup();
            _propertySet = compositor.CreatePropertySet();
            var visual = ElementCompositionPreview.GetElementVisual(this);

            // translation
            var translationAnimation = compositor.CreateScalarKeyFrameAnimation();
            translationAnimation.InsertExpressionKeyFrame(1f, "props.TranslateTo");
            translationAnimation.SetReferenceParameter("props", _propertySet);
            translationAnimation.StopBehavior = AnimationStopBehavior.SetToFinalValue;
            translationAnimation.Duration = TimeSpan.FromSeconds(1);
            _translationY = translationAnimation;

            // shadow
            var shadowVisual = compositor.CreateSpriteVisual();
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.BlurRadius = 0;
            shadowVisual.Shadow = dropShadow;
            ElementCompositionPreview.SetElementChildVisual(xamlShadow, shadowVisual);
            _shadow = dropShadow;

            // sync size
            var imgVisual = ElementCompositionPreview.GetElementVisual(xamlImg);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", imgVisual);
            shadowVisual.StartAnimation("Size", bindSizeAnimation);

            var blurRadiusAnimation = compositor.CreateScalarKeyFrameAnimation();
            blurRadiusAnimation.InsertExpressionKeyFrame(1f, "props.BlurRadius");
            blurRadiusAnimation.SetReferenceParameter("props", _propertySet);
            blurRadiusAnimation.StopBehavior = AnimationStopBehavior.SetToFinalValue;
            blurRadiusAnimation.Duration = TimeSpan.FromSeconds(1);
            blurRadiusAnimation.Target = "BlurRadius";

            var opacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityAnimation.InsertExpressionKeyFrame(1f, "props.Opacity");
            opacityAnimation.SetReferenceParameter("props", _propertySet);
            opacityAnimation.StopBehavior = AnimationStopBehavior.SetToFinalValue;
            opacityAnimation.Duration = TimeSpan.FromSeconds(1);
            opacityAnimation.Target = "Opacity";

            _animationGroup.Add(blurRadiusAnimation);
            // _animationGroup.Add(opacityAnimation);
        }

    }
}
