﻿#pragma checksum "..\..\..\AnimatedTextBox.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F0FCA628D699FA0DD9E5CA4EBDB206EC207D577E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ReportFrontPage;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ReportFrontPage {
    
    
    /// <summary>
    /// AnimatedTextBox
    /// </summary>
    public partial class AnimatedTextBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\AnimatedTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox mainTextBox;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\AnimatedTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle roundedRectangle;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\AnimatedTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label placeHolder;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\AnimatedTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas viewPassCanvas;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\AnimatedTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label viewPassLabel;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\AnimatedTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Line viewPassLabelLine;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ReportFrontPage;component/animatedtextbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AnimatedTextBox.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.mainTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 16 "..\..\..\AnimatedTextBox.xaml"
            this.mainTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.MainTextBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\AnimatedTextBox.xaml"
            this.mainTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.MainTextBox_LostFocus);
            
            #line default
            #line hidden
            
            #line 18 "..\..\..\AnimatedTextBox.xaml"
            this.mainTextBox.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.MainTextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\AnimatedTextBox.xaml"
            this.mainTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.mainTextBox_TextChanged);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\AnimatedTextBox.xaml"
            this.mainTextBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.MainTextBox_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 2:
            this.roundedRectangle = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 3:
            this.placeHolder = ((System.Windows.Controls.Label)(target));
            
            #line 36 "..\..\..\AnimatedTextBox.xaml"
            this.placeHolder.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.PlaceHolder_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.viewPassCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 5:
            this.viewPassLabel = ((System.Windows.Controls.Label)(target));
            
            #line 46 "..\..\..\AnimatedTextBox.xaml"
            this.viewPassLabel.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ViewPassLabelClicked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.viewPassLabelLine = ((System.Windows.Shapes.Line)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
