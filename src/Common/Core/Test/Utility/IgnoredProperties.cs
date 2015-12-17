﻿using System.Collections.Generic;

namespace Microsoft.Common.Core.Test.Utility {
    internal static class IgnoredProperties {
        private static HashSet<string> _hashset;

        public static bool IsIgnoredProperty(string name) {
            Init();
            return _hashset.Contains(name);
        }

        private static void Init() {
            if (_hashset == null) {
                _hashset = new HashSet<string>();
                foreach (var s in _propertyNames) {
                    _hashset.Add(s);
                }
            }
        }

        private static readonly string[] _propertyNames = {
        "HasContent",
        "OverridesDefaultStyle",
        "UseLayoutRounding",
        "Language",
        "LayoutTransform",
        "FlowDirection",
        "FocusVisualStyle",
        "ForceCursor",
        "AllowDrop",
        "RenderTransform",
        "RenderTransformOrigin",
        "IsMouseDirectlyOver",
        "IsMouseOver",
        "IsStylusOver",
        "IsKeyboardFocusWithin",
        "IsMouseCaptured",
        "IsMouseCaptureWithin",
        "IsStylusDirectlyOver",
        "IsStylusCaptured",
        "IsStylusCaptureWithin",
        "IsKeyboardFocused",
        "Opacity",
        "Uid",
        "ClipToBounds",
        "SnapsToDevicePixels",
        "IsHitTestVisible",
        "Focusable",
        "IsManipulationEnabled",
        "AreAnyTouchesOver",
        "AreAnyTouchesDirectlyOver",
        "AreAnyTouchesCapturedWithin",
        "AreAnyTouchesCaptured",
        "IsInputMethodEnabled",
        "IsInputMethodSuspended",
        "PreferredImeState",
        "PreferredImeConversionMode",
        "PreferredImeSentenceMode",
        "HasDropShadow",
        "PlacementRectangle",
        "Placement",
        "ShowOnDisabled",
        "IsOpen",
        "ShowDuration",
        "InitialShowDelay",
        "BetweenShowDelay",
        "HorizontalOffset",
        "VerticalOffset",
        "IsFocusScope",
        "EdgeMode",
        "BitmapScalingMode",
        "ClearTypeHint",
        "CachingHint",
        "CacheInvalidationThresholdMinimum",
        "CacheInvalidationThresholdMaximum",
        "TextHintingMode",
        "CultureSource",
        "Substitution",
        "StandardLigatures",
        "ContextualLigatures",
        "DiscretionaryLigatures",
        "HistoricalLigatures",
        "AnnotationAlternates",
        "ContextualAlternates",
        "HistoricalForms",
        "Kerning",
        "CapitalSpacing",
        "CaseSensitiveForms",
        "StylisticSet1",
        "StylisticSet2",
        "StylisticSet3",
        "StylisticSet4",
        "StylisticSet5",
        "StylisticSet6",
        "StylisticSet7",
        "StylisticSet8",
        "StylisticSet9",
        "StylisticSet10",
        "StylisticSet11",
        "StylisticSet12",
        "StylisticSet13",
        "StylisticSet14",
        "StylisticSet15",
        "StylisticSet16",
        "StylisticSet17",
        "StylisticSet18",
        "StylisticSet19",
        "StylisticSet20",
        "Fraction",
        "SlashedZero",
        "MathematicalGreek",
        "EastAsianExpertForms",
        "Variants",
        "Capitals",
        "NumeralStyle",
        "NumeralAlignment",
        "EastAsianWidths",
        "EastAsianLanguage",
        "StandardSwashes",
        "ContextualSwashes",
        "StylisticAlternates",
        "TextFormattingMode",
        "TextRenderingMode",
        "ZIndex",
        "TabNavigation",
        "ControlTabNavigation",
        "DirectionalNavigation",
        "AcceptsReturn",
        "BaseUri",
        "XmlnsDictionary",
        "XmlnsDefinition",
        "IsHyphenationEnabled",
        "LineHeight",
        "LineStackingStrategy",
        "BaselineOffset",
        "AlternationIndex",
        "IsVirtualizingWhenGrouping",
        "CacheLength",
        "CacheLengthUnit",
        "IsContainerVirtualizable",
        "IsSelectionActive",
        "IsSelected",
        "NameScope",
        "CanContentScroll",
        "IsDeferredScrollingEnabled",
        "PanningMode",
        "PanningDeceleration",
        "PanningRatio",
        "Column",
        "Row",
        "ColumnSpan",
        "RowSpan",
        "IsSharedSizeScope",
        "AutomationId",
        "Name",
        "HelpText",
        "AcceleratorKey",
        "AccessKey",
        "ItemStatus",
        "ItemType",
        "IsColumnHeader",
        "IsRowHeader",
        "IsRequiredForForm",
        "IsOffscreenBehavior",
        "IsOverflowItem",
        "OverflowMode",
        "Errors",
        "ErrorTemplate",
        "VisualStateGroups",
        "PlacementTarget",
        "XmlNamespaceManager",
        "LabeledBy",
        "TargetName",
        "TargetProperty",
        "Target",
        "DesiredFrameRate",
        "HasError",
        "ValidationAdornerSite",
        "ValidationAdornerSiteFor",
        "CustomVisualStateManager",
        "OpacityMask",
        "BitmapEffect",
        "Effect",
        "BitmapEffectInput",
        "CacheMode",
        "XmlSpace",
        "XmlNamespaceMaps",
        "CultureOverride",
        "BindingGroup",
        "Tag",
        "InputScope",
        "Delay",
        "Interval",
        "CommandParameter",
        "CommandTarget",
        "ContentStringFormat",
        "DisplayMemberPath"
        };
    }
}
