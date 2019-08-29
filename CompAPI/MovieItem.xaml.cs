using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
        private CompositionAnimation _blurShadowIn;
        private CompositionAnimation _blurShadowOut;
        private CompositionAnimation _opacityShadow;
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
            StartAnimation(true, -8, 0.8f, 40);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            StartAnimation(false, 0, 0, 0);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            StartAnimation(true, 0, 0.4f, 24);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            StartAnimation(true, -8, 0.6f, 40);
        }

        private void StartAnimation(bool pointerOver, float translation, float shadowOpacity, float blurRadius)
        {
            _propertySet.InsertScalar("TranslateTo", translation);
            var visual = ElementCompositionPreview.GetElementVisual(this);
            visual.StartAnimation("Translation.Y", _translationY);

            _propertySet.InsertScalar("Opacity", shadowOpacity);
            _propertySet.InsertScalar("BlurRadius", blurRadius);
            _animationGroup.RemoveAll();
            _animationGroup.Add(pointerOver ? _blurShadowIn : _blurShadowOut);
            _animationGroup.Add(_opacityShadow);
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
            translationAnimation.Duration = TimeSpan.FromSeconds(0.6);
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

            var blurRadiusAnimationIn = compositor.CreateScalarKeyFrameAnimation();
            blurRadiusAnimationIn.InsertExpressionKeyFrame(1f, "props.BlurRadius");
            blurRadiusAnimationIn.SetReferenceParameter("props", _propertySet);
            blurRadiusAnimationIn.StopBehavior = AnimationStopBehavior.SetToFinalValue;
            blurRadiusAnimationIn.Duration = TimeSpan.FromSeconds(0.6);
            blurRadiusAnimationIn.Target = "BlurRadius";
            _blurShadowIn = blurRadiusAnimationIn;

            var blurRadiusAnimationOut = compositor.CreateScalarKeyFrameAnimation();
            var easing = compositor.CreateCubicBezierEasingFunction(new Vector2(0.27f, 0.72f), new Vector2(0.82f, 0.29f));
            blurRadiusAnimationOut.InsertExpressionKeyFrame(1f, "props.BlurRadius");
            blurRadiusAnimationOut.SetReferenceParameter("props", _propertySet);
            blurRadiusAnimationOut.StopBehavior = AnimationStopBehavior.SetToFinalValue;
            blurRadiusAnimationOut.Duration = TimeSpan.FromSeconds(0.6);
            blurRadiusAnimationOut.Target = "BlurRadius";
            _blurShadowOut = blurRadiusAnimationOut;

            var opacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityAnimation.InsertExpressionKeyFrame(1f, "props.Opacity");
            opacityAnimation.SetReferenceParameter("props", _propertySet);
            opacityAnimation.StopBehavior = AnimationStopBehavior.SetToFinalValue;
            opacityAnimation.Duration = TimeSpan.FromSeconds(0.6);
            opacityAnimation.Target = "Opacity";
            _opacityShadow = opacityAnimation;
        }

    }
}
