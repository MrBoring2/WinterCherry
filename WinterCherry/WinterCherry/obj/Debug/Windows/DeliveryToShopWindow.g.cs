﻿#pragma checksum "..\..\..\Windows\DeliveryToShopWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "21989184A0A95B132B5FC34AC44D490EE26B18DBAD32C3045635E31348B9E181"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Converters;
using MaterialDesignExtensions.Model;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using WinterCherry.Windows;


namespace WinterCherry.Windows {
    
    
    /// <summary>
    /// DeliveryToShopWindow
    /// </summary>
    public partial class DeliveryToShopWindow : WinterCherry.Windows.BaseWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 54 "..\..\..\Windows\DeliveryToShopWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ToFirst;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Windows\DeliveryToShopWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Back;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\Windows\DeliveryToShopWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Forward;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Windows\DeliveryToShopWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ToLast;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Windows\DeliveryToShopWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddToDelivery;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\Windows\DeliveryToShopWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateDelivery;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\Windows\DeliveryToShopWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveFromDelivery;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WinterCherry;component/windows/deliverytoshopwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\DeliveryToShopWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ToFirst = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\Windows\DeliveryToShopWindow.xaml"
            this.ToFirst.Click += new System.Windows.RoutedEventHandler(this.ToFirst_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Back = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\..\Windows\DeliveryToShopWindow.xaml"
            this.Back.Click += new System.Windows.RoutedEventHandler(this.Back_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Forward = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\Windows\DeliveryToShopWindow.xaml"
            this.Forward.Click += new System.Windows.RoutedEventHandler(this.Forward_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ToLast = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\..\Windows\DeliveryToShopWindow.xaml"
            this.ToLast.Click += new System.Windows.RoutedEventHandler(this.ToLast_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.AddToDelivery = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\Windows\DeliveryToShopWindow.xaml"
            this.AddToDelivery.Click += new System.Windows.RoutedEventHandler(this.AddToDelivery_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CreateDelivery = ((System.Windows.Controls.Button)(target));
            
            #line 89 "..\..\..\Windows\DeliveryToShopWindow.xaml"
            this.CreateDelivery.Click += new System.Windows.RoutedEventHandler(this.CreateDelivery_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.RemoveFromDelivery = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\..\Windows\DeliveryToShopWindow.xaml"
            this.RemoveFromDelivery.Click += new System.Windows.RoutedEventHandler(this.RemoveFromDelivery_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

